using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskRestoreAbility : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TreeAnimation change = collision.gameObject.GetComponent<TreeAnimation>();
        if (change == null)
            return;
        change.ObjectChangeInMask();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        TreeAnimation change = collision.gameObject.GetComponent<TreeAnimation>();
        if (change == null)
            return;
        change.ObjectChangeOutMask();
    }
}
