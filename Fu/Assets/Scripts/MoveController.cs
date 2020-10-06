using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Windows版本的主角控制器
/// </summary>
public class MoveController : MonoBehaviour
{
    /// <summary>
    /// 主角移动脚本
    /// </summary>
    PlayerMove playerMove;
    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
    }
    /// <summary>
    /// 接收输入信号
    /// </summary>
    void CheckInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        playerMove.walk(x, y);
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
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }
}
