using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 地图状态控制脚本
/// 切换废土状态和完整状态
/// </summary>
public class ObjectChange : MonoBehaviour
{
    [Header("完整和损坏的图片")]
    public Sprite destoryImage;
    public Sprite completeImage;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    /// <summary>
    /// 初始化废土状态
    /// </summary>
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            throw new System.Exception("物体没有SpriteRenderer");
        }
        changeDestory();
    }
    /// <summary>
    /// 改变为废土状态
    /// </summary>
    public void changeDestory()
    {
        spriteRenderer.sprite = destoryImage;
    }
    /// <summary>
    /// 改变为完整状态
    /// </summary>
    public void changeComplete()
    {
        spriteRenderer.sprite = completeImage;
    }
}
