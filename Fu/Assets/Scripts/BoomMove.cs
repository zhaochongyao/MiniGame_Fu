using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 炸弹运动函数
/// 暂时为直线运动,可改为抛物线
/// </summary>
public class BoomMove : MonoBehaviour
{
    public float moveSpeed;     //运动速度
    public bool face;           //是否向右运动
    // Start is called before the first frame update
    void Start()
    {
        //开启自动销毁线程
        StartCoroutine("Free");
    }
    
    // Update is called once per frame
    void Update()
    {
        if (face)
        {
            transform.Translate(new Vector2(Mathf.Abs(moveSpeed) * Time.deltaTime, 0));
        }
        else
        {
            transform.Translate(new Vector2(-Mathf.Abs(moveSpeed) * Time.deltaTime, 0));
        }
    }
    IEnumerator Free()
    {
        yield return new WaitForSeconds(3.0f);
        //Destroy(gameObject);
    }
}
