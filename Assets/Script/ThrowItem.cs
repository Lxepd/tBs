using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ç½±Ú"))
        {
            PoolMgr.GetInstance().PushObj(name.Replace("(Clone)",""), gameObject);
        }
    }
}
