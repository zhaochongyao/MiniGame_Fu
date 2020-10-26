using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 炸弹运动函数
/// 抛物线运动
/// 将地面的tag设置为ground,炸弹将会在碰到地面时停下
/// </summary>
public class BoomMove : MonoBehaviour
{
    public float move_on_x;     //扔出后水平的移动距离
    public float move_height;   //扔出后的相对最大高度
    public float vertical_acceleration;     //垂直加速度
    public float moveSpeed_x;     //水平运动速度
    public float moveSpeed_y;   //垂直运动速度
    public bool face;           //是否向右运动
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<BeGleaned>()!=null)
            GetComponent<BeGleaned>().enabled = false;
        //开启自动销毁线程
        StartCoroutine("Free");
    }
    public void data_initialize()
    {
        UnityEngine.Debug.Log("move_on_x:" + move_on_x);
        UnityEngine.Debug.Log("moveSpeed_x:" + moveSpeed_x);
        moveSpeed_x = move_on_x / ((Mathf.Sqrt(2 * move_height / vertical_acceleration))*2);
        moveSpeed_y = ((Mathf.Sqrt(2 * move_height / vertical_acceleration))) * vertical_acceleration;
    }
    // Update is called once per frame
    void Update()
    {
        if (face)
        {
            transform.Translate(new Vector2(Mathf.Abs(moveSpeed_x) * Time.deltaTime, moveSpeed_y*Time.deltaTime));
        }
        else
        {
            transform.Translate(new Vector2(-Mathf.Abs(moveSpeed_x) * Time.deltaTime, moveSpeed_y*Time.deltaTime));
        }
        UnityEngine.Debug.Log("炸弹速度:"+moveSpeed_x);
        moveSpeed_y -= vertical_acceleration * Time.deltaTime;
    }

    IEnumerator Free()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
    }
    /// <summary>
    /// 碰撞到地面时停留
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag!="ground")
        {
            return;
        }
        UnityEngine.Debug.Log(collision.transform.name);
        /*moveSpeed_x = 0;
        moveSpeed_y = 0;
        vertical_acceleration = 0;*/
        if (GetComponent<BeGleaned>() != null)
            GetComponent<BeGleaned>().enabled = true;
    }
}
