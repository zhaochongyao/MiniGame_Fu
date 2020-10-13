using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

/// <summary>
/// 更新日志 2020.10.10 更新人:赵崇尧
/// 新增param material 用于战争迷雾,按需要删改
/// </summary>

/// <summary>
/// 
/// 主角移动脚本
/// 动画状态机的控制暂未编写，只写了下蹲和站立状态的切换
/// </summary>
public class PlayerMove : moveObject
{
    /// <summary>
    /// 更新部分
    /// </summary>
    /// 
    public Material material;               //光影材质  用于灯光跟随主角
    float original_x;               //人物的起始坐标
    float original_y;
    /// <summary>
    /// 更新部分
    /// </summary>
    private Animator animator;          //动画控制器
    public float gravity = 5.0f;        //重力大小
    private Transform stairTransform;       //可以攀爬的梯子
    public float climbSpeed;      //爬梯子的速度
    public float hanging = 1;    //1.无梯子 2.在梯子范围内  3.在梯子上
    public float test_upWall_length = 1f;    //向上检测墙壁的射线长度
    public float test_stair_length = 0.5f;  //检测左右楼梯的射线长度
    private float walkSpeed;      //行走速度

    /// <summary>
    /// stairsLayer代表梯子层级,用于射线检测梯子
    /// </summary>
    public LayerMask stairsLayer;
    public bool isCrouch = false;   //是否下蹲
    // Start is called before the first frame update
    private void Start()
    {
        //更新部分
        original_x = transform.position.x;
        original_y = transform.position.y;
        //更新部分
        base.init();
        Animator[] ts = gameObject.GetComponentsInChildren<Animator>();
        animator = ts[0];
    }
    /// <summary>
    /// 检测是否有可以攀爬的梯子
    /// 更新光影中的光照位置,使它始终跟随主角移动
    /// </summary>
    private void Update()
    {
        checkStairs();
        Animation();
        //更新部分
        material.SetFloat("_XScrollLength", original_x-transform.position.x );
        material.SetFloat("_YScrollLength", original_y - transform.position.y);
        //更新部分
    }
    /// <summary>
    /// 攀爬楼梯函数
    /// 只有在hanging = 3时执行,即在楼梯上时执行
    /// </summary>
    /// <param name="direction_y">
    /// 在楼梯上的行进方向
    /// </param>
    void climb(float direction_y)
    {
        if (hanging != 3)
            return;
        Vector3 move = new Vector3(0, direction_y * climbSpeed, 0);
        transform.Translate(move * Time.deltaTime);
    }
    /// <summary>
    /// 主角移动函数
    /// 当不在梯子上时,可以自由水平移动
    /// 在梯子有效区域内进行垂直移动可以爬上梯子
    /// 在梯子上重力为0,水平速度为0
    /// 
    /// 
    /// 爬到顶点和底部离开梯子的逻辑尚未编写
    /// 目前只能通过跳跃离开梯子
    /// </summary>
    /// <param name="direction_x"></param>
    /// 水平移动方向
    /// <param name="direction_y"></param>
    /// 垂直移动方向
    public void walk(float direction_x,float direction_y)
    {
        if (hanging!=3)
        {
            base.walk(direction_x);
            walkSpeed = Mathf.Abs(direction_x);
            if (direction_y != 0 && hanging == 2 && isJump == false)       //当进行垂直移动且在梯子附近且未在跳跃状态可以进入梯子
            {
                hanging = 3;
                transform.position = new Vector2(stairTransform.position.x,transform.position.y);
                rig.gravityScale = 0;
                rig.velocity = new Vector2(0,0);
            }    
        }
        climb(direction_y);
    }
    /// <summary>
    /// 下蹲函数
    /// 下蹲后改变碰撞体的位置和大小
    /// 暂定为缩小一半
    /// </summary>
    public void crouch()
    {
        if (isGround&&hanging!=3)
        {
            if (!isCrouch)
            {
                isCrouch = true;
                collider2D.size = new Vector2(collider2D.size.x, collider2D.size.y / 2);
                collider2D.offset = new Vector2(0, -(collider2D.size.y / 2));
            }

        }

    }
    /// <summary>
    /// 若头上没有墙则结束下蹲,站立
    /// 回复碰撞体的大小和位置
    /// 若头上有墙继续下蹲状态
    /// </summary>
    public void overCrouch()
    {
        if (upIsWall())
        {
            return;
        }
        if (isCrouch)
        {
            isCrouch = false;
            collider2D.size = new Vector2(collider2D.size.x, collider2D.size.y * 2);
            collider2D.offset = new Vector2(0, 0);
            animator.SetBool("isCrouch", false);
        }

    }
    /// <summary>
    /// 跳跃函数
    /// 给刚体施加向上的力使其跳跃
    /// 跳跃时脱离梯子状态并回复正常重力
    /// </summary>
    public void jump()
    {
        
        if ((isGround && !isCrouch)||hanging==3)
        {
            if (hanging == 3)
            {
                hanging = 2;
                rig.gravityScale = gravity;
                animator.SetBool("Climb", false);
            }
            
            isJump = true;
            rig.AddForce(new Vector2(0, jumpForce));
        }
    }
    /// <summary>
    /// 用射线检测蹲下时头上有无墙
    /// 
    /// </summary>
    /// <returns></returns>
    bool upIsWall()
    {
        Vector2 position1 = transform.position;
        Vector2 direction = Vector2.up;
        float distance = test_upWall_length;
        RaycastHit2D hit = Physics2D.Raycast(position1, direction, distance, groundLayer);
        UnityEngine.Debug.DrawRay(position1, new Vector2(0, distance), Color.green);
        if (hit.transform != null)
        {
            UnityEngine.Debug.Log(hit.transform.gameObject.name);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 用两条射线检测身体周围有无可以攀爬的梯子
    /// 检测原理同地面检测，具体查看moveObject.isGrounded()
    /// </summary>
    void checkStairs()
    {
        Vector2 position1 = transform.position;

        float distance = test_stair_length;
        // Vector2 position2 = new Vector2(position1.x - (c.bounds.size.x / 2), position1.y);

        RaycastHit2D hit1 = Physics2D.Raycast(position1, Vector2.left, distance, stairsLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(position1, Vector2.right, distance, stairsLayer);

        UnityEngine.Debug.DrawRay(position1, new Vector2(distance,0), Color.green);
        UnityEngine.Debug.DrawRay(position1, new Vector2(-distance, 0), Color.green);

        //当检测到梯子时切换hanging状态到2,同时设置stairTransform为当前可攀爬的梯子
        if ((hit1.collider != null || hit2.collider != null) && hanging == 1)
        {
            hanging = 2;
            if (hit1.collider != null)
            {
                stairTransform = hit1.collider.transform;
            }
            else
            {
                stairTransform = hit2.collider.transform;
            }
        }
        //为检测到梯子时回复重力
        else if (hit1.collider == null && hit2.collider == null && hanging > 1)
        {
            stairTransform = null;
            hanging = 1;
            rig.gravityScale = gravity;
        }

    }

    /// <summary>
    /// 随时改变Animation
    /// </summary>
    void Animation()
    {
        if (isGround)
            animator.SetBool("Jump", false);
        else
            animator.SetBool("Jump", true);
        if (hanging == 3)
            animator.SetBool("Climb", true);
        if (isCrouch)
            animator.SetBool("Crouch", true);
        else
            animator.SetBool("Crouch", false);

        animator.SetFloat("Walk", walkSpeed);
    }
}
