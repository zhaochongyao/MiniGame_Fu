using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VineFillOut : MonoBehaviour
{
    public void changeComplete()
    {
        this.GetComponent<Animator>().SetTrigger("VineGrowth");
        this.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;

        foreach (Transform child in transform)
        {
            if (child.tag == "collider")
                child.gameObject.SetActive(true);
        }
    }
}
