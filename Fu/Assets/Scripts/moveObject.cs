using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 基础移动脚本
/// 使用添加刚体速度来进行平滑移动,可根据需要修改
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
    public float move_speed = 5.0f;         //水平移动速度
   
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
        if (isGround)
        {
            rig.sharedMaterial = p1;
        }
        if (!isGround)
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

        float moveSpeed = move_speed * direction;
        if (!moveAble)
        {
            moveSpeed = 0;
        }
        if (moveSpeed > 0 && moveAble)
        {
            face = true;
            transform.localScale = new Vector2(1f, 1f);

        }
        else if (moveSpeed < 0 && moveAble)
        {
            face = false;
            transform.localScale = new Vector2(-1f, 1f);
        }
        
        if(rig!=null)
            rig.velocity = (new Vector2(moveSpeed, rig.velocity.y));
        isGround = isGrounded();
    }
   
   
    /// <summary>
    /// 用三条射线判断角色是否在地面上
    /// </summary>
    /// <returns></returns>
    bool isGrounded()
    {
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
            isJump = false;
            return true;
        }
        return false;
    }
   
}
