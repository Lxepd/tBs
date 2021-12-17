using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("г╫╠з"))
        {
            PoolMgr.GetInstance().PushObj("Prefabs/й╞вс", gameObject);
        }
    }
}
