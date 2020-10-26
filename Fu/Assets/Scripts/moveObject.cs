using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基础移动脚本
/// 使用添加刚体速度来进行平滑移动,可根据需要修改
/// 更新日志 2020.10.10 更新人:赵崇尧
/// 修改移动为从0速度开始加速移动
/// </summary>
public class moveObject : MonoBehaviour
{
    /// <summary>
    /// 摩擦力材质
    /// p1为在地面的摩擦力材质,可根据需要自定义材质或者不设置材质
    /// p2为在空中的摩擦力材质,为防止角色卡在墙体边缘,需要设置0摩擦力物理材质
    /// </summary>
    public PhysicsMaterial2D p1;    
    public PhysicsMaterial2D p2;
    /// <summary>
    /// groundLayer代表地面层级,用于射线地面检测
    /// </summary>
    public LayerMask groundLayer;
    public float test_ground_Length=1.2f;   //向下检测地面的射线长度
    [Header("水平移动最大速度")]
    public float max_MoveSpeed = 5.0f;      //水平移动最大速度
    [Header("水平移动加速度")]
    public float move_acceleration = 10f;         //水平移动加速度
    public float cur_move_speed = 0f;       //当前水平移动速度
    protected bool running = false;         //是否在奔跑 

    public bool checkGround = true;     //是否进行地面检测
    public bool isGround = false;           //是否在地面
    public bool moveAble = true;            //是否可以移动
    public bool face = false;               //是否面朝右边
    public bool isJump = false;             //是否处于跳跃状态
    protected BoxCollider2D collider2D ;    //物体的碰撞器
    protected Rigidbody2D rig;              //物体的刚体
    public float jumpForce = 500.0f;        //起跳速度            
    

    /// <summary>
    /// 初始化组件
    /// </summary>
    public void init()
    {
        rig = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<BoxCollider2D>();
        if (rig == null && collider2D == null)
        {
            throw new System.Exception("缺失刚体或碰撞体");
        }
    }
    // Update is called once per frame
    /// <summary>
    /// 根据当前位置切换物理材质
    /// </summary>
    private void FixedUpdate()
    {
        if (isGround && rig != null)
        {
            rig.sharedMaterial = p1;
        }
        if (!isGround&&rig!=null)
        {
            rig.sharedMaterial = p2;
        }
    }
   
    /// <summary>
    /// 水平移动函数
    /// 根据face来判断朝向,通过修改scale来修改物体朝向
    /// </summary>
    /// <param name="direction">
    ///     行走方向
    /// </param>
    public void walk(float direction)
    {

        float moveSpeed = cur_move_speed;
        if (!moveAble)
        {
            moveSpeed = 0;
        }
        if (direction > 0 && moveAble)
        {
            if (!face&&isGround)                                  //改变方向且不在空中时,速度立即置0
            {
                cur_move_speed = 0;
                face = true;
                transform.localScale = new Vector2(-1f, 1f);
            }
           
        }
        else if (direction < 0 && moveAble)
        {
            if (face&&isGround)                                  //改变方向且不在空中时,速度立即置0
            {
                cur_move_speed = 0;
                face = false;
                transform.localScale = new Vector2(1f, 1f);
            }
        }
        if (direction != 0&&moveAble)      
        {
            if(!running && Mathf.Abs(cur_move_speed) < max_MoveSpeed / 2)//非跑步加速
            {
                cur_move_speed = cur_move_speed + (face ? move_acceleration : -move_acceleration) * Time.deltaTime;
            }else if (running && Mathf.Abs(cur_move_speed) < max_MoveSpeed)
            {
                cur_move_speed = cur_move_speed + (face ? move_acceleration : -move_acceleration) * Time.deltaTime;
            }
           
        }else if (direction == 0 && isGround)                   //松开按键且不在空中速度立即置0
        {
           
            cur_move_speed = 0;
        }

        if (rig != null)
        {
            rig.velocity = (new Vector2(cur_move_speed, rig.velocity.y));
           
        }
            
        isGround = isGrounded();
        
           
            
        
    }
   
   
    /// <summary>
    /// 用三条射线判断角色是否在地面上
    /// </summary>
    /// <returns></returns>
    public bool isGrounded()
    {
        if (!checkGround)
        {
            return true;
        }
        //三条射线的起始点: 左边,中间,右边
        Vector2 position1 = transform.position;                                                     
        Vector2 position2 = new Vector2(position1.x - (collider2D.bounds.size.x / 2), position1.y);
        Vector2 position3 = new Vector2(position1.x + (collider2D.bounds.size.x / 2), position1.y);
        //使用Raycast函数向下检测是否触碰groundLayer层
        Vector2 direction = Vector2.down;
        float distance = test_ground_Length;
        RaycastHit2D hit1 = Physics2D.Raycast(position1, direction, distance, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(position2, direction, distance, groundLayer);
        RaycastHit2D hit3 = Physics2D.Raycast(position3, direction, distance, groundLayer);
        //使用Debug绘制三条射线,方便调试
        UnityEngine.Debug.DrawRay(position1, new Vector2(0,-distance), Color.green);
        UnityEngine.Debug.DrawRay(position2, new Vector2(0,-distance), Color.green);
        UnityEngine.Debug.DrawRay(position3, new Vector2(0,-distance), Color.green);

        if (hit1.collider != null || hit2.collider != null || hit3.collider != null)
        {
            UnityEngine.Debug.Log("踩着:"+hit2.transform.name);
            isJump = false;
            return true;
        }
        else
        {
            UnityEngine.Debug.Log("脚下没有东西");
        }
        return false;
    }
    public void changeRunning(bool running)
    {
        this.running = running;
        if (!running &&Mathf.Abs(cur_move_speed)>max_MoveSpeed/2)
        {
            cur_move_speed = max_MoveSpeed / 2 * (face?1:-1);
        }
    }
}
