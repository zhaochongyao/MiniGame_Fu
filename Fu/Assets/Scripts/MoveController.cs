using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 更新日志 2020.10.10 赵崇尧
/// 增加使用道具的操作
/// </summary>

/// </summary>
/// Windows版本的主角控制器
/// </summary>
public class MoveController : MonoBehaviour
{
    /// <summary>
    /// 主角移动脚本
    /// </summary>
    PlayerMove playerMove;
    gleaner glean;
    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        glean = GetComponent<gleaner>();
    }
    /// <summary>
    /// 接收输入信号
    /// </summary>
    void CheckInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        playerMove.walk(x,y);
        if (Input.GetKeyDown(KeyCode.K))
        {
            playerMove.jump();
        }
        if (Input.GetKey(KeyCode.S))
        {
            playerMove.crouch();
        }
        else 
        {
            playerMove.overCrouch();
        }
        ///更新部分
        if (Input.GetKeyDown(KeyCode.J))
        {
            glean.useObject(0);
        }
        ///更新部分
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }
}
