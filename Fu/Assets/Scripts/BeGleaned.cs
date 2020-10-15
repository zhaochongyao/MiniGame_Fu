using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 道具被拾取脚本
/// 所有能被拾取的物体上都应该挂这个脚本
/// </summary>
public class BeGleaned : MonoBehaviour
{
    public int index;       //道具索引  对应player的道具数组
    private void Start()
    {
       if (GetComponent<Collider2D>() == null)
        {
            throw new System.Exception("道具未设置触发器");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        UnityEngine.Debug.Log("有人物捡到道具");
        gleaner glean = collision.gameObject.GetComponent<gleaner>();
        if (glean == null)
            return;
        glean.gleanObject(index);
        gameObject.SetActive(false);
    }
  
}
