using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VineFillOut : MonoBehaviour
{
    private float fillAmount;
    private float fillStart;
    private float fillTime;
    private bool restore = false; 
    // Start is called before the first frame update
    void Start()
    {
        fillStart = this.GetComponent<Image>().fillAmount;
        fillTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (restore)
        {
            fillAmount = Mathf.Lerp(fillStart, 1f, fillTime);
            fillTime += Time.deltaTime;
            this.GetComponent<Image>().fillAmount = fillAmount;
            foreach (Transform child in transform)
            {
                if (child.tag == "collider")
                    child.gameObject.SetActive(true);
            }
        }
    } 
    public void changeComplete()
    {
        restore = true;
    }
}
