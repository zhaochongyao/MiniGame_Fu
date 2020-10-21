using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAnimation : MonoBehaviour
{
    private Vector3 localPos;
    private float yAxis;

    void Start()
    {
        localPos = transform.position;
        yAxis = transform.position.y - GetComponent<Renderer>().bounds.size.y /2 ;
    }

    void Update()
    {
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("TreeEnd"))
        {
            transform.position = new Vector3(localPos.x, yAxis + GetComponent<Renderer>().bounds.size.y / 2, 0);
        }
    }

    public void changeComplete()
    {
        GetComponent<Animator>().SetBool("TreeGrow", true);
       
        foreach(Transform child in this.transform.parent.transform)
        {
            if (child.tag == "collider")
                child.gameObject.SetActive(true);
        }
    }
    public void changeDestory()
    {

    }
    public void ObjectChangeInMask()
    {
        GetComponent<Animator>().SetBool("PlayerClose", true);
        if (this.name == "FirstTree")
            GetComponent<Animator>().SetBool("TreeGrow", true);
    }
    public void ObjectChangeOutMask()
    {
        GetComponent<Animator>().SetBool("PlayerClose", false);
    }

    
}
