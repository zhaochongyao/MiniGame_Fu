using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskShrink : MonoBehaviour
{
    public GameObject StartingPoint;
    public GameObject EndingPoint;
    public float StartingSize;
    public float EndingSize;
    private bool shink = true;
    private float MaskRadius;
    void Start()
    {
        this.transform.localScale = new Vector3(StartingSize, StartingSize, 1);
    }

    void Update()
    {
        if (shink)
        {
            MaskRadius = (EndingSize - StartingSize) * (this.transform.position.x - StartingPoint.transform.position.x) / (EndingPoint.transform.position.x - StartingPoint.transform.position.x) + StartingSize;
            if (MaskRadius <= EndingSize)
            {
                MaskRadius = EndingSize;
                shink = false;
            }
            if(MaskRadius < this.transform.localScale.x)
            this.transform.localScale = new Vector3(MaskRadius, MaskRadius, 1);
        }
    }
}
