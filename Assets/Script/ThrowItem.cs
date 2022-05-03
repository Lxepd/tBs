using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum WhoShoot
{
    Player,
    Enemy
}
public class ThrowItem : MonoBehaviour
{
    [HideInInspector] public WhoShoot ws;
    [HideInInspector] public float hurt;
    Collider2D col;
    LayerMask mask;

    Vector3 boxSize;

    private void Start()
    {
        mask = LayerMask.GetMask("³¡¾°");
        boxSize = GetComponent<BoxCollider2D>().size;
    }

    private void Update()
    {
        switch (ws)
        {
            case WhoShoot.Player:
                mask |= LayerMask.GetMask("µÐÈË");
                break;
            case WhoShoot.Enemy:
                mask |= LayerMask.GetMask("Íæ¼Ò");
                break;
        }

        col = Physics2D.OverlapBox(transform.position, boxSize * 2, 0, mask);
        if (col == null)
            return;

        switch (ws)
        {
            case WhoShoot.Player:
                if (col.CompareTag("µÐÈË"))
                {
                    EventCenter.GetInstance().EventTrigger<float>("µÐÈË¿ÛÑª", hurt);
                    PoolMgr.GetInstance().PushObj(name, gameObject);

                    MusicMgr.GetInstance().PlaySound("HIT_METAL_WRENCH_HEAVIEST_02", false);
                    MusicMgr.GetInstance().ChangeSoundValue(.3f);
                }
                break;
            case WhoShoot.Enemy:
                if (col.CompareTag("Player"))
                {
                    EventCenter.GetInstance().EventTrigger<float>("Íæ¼Ò¿ÛÑª", hurt);
                    PoolMgr.GetInstance().PushObj(name, gameObject);

                    MusicMgr.GetInstance().PlaySound("HIT_METAL_WRENCH_HEAVIEST_02", false);
                    MusicMgr.GetInstance().ChangeSoundValue(.3f);
                }
                break;
        }

        // Åöµ½Ç½±ÚÉ¶Ò²Ã»ÊÂ
        if (col.CompareTag("Ç½±Ú"))
        {
            PoolMgr.GetInstance().PushObj(name, gameObject);
            return;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //// Åöµ½Ç½±ÚÉ¶Ò²Ã»ÊÂ
        //if (collision.CompareTag("Ç½±Ú"))
        //{
        //    PoolMgr.GetInstance().PushObj(name, gameObject);
        //    return;
        //}

        //switch (ws)
        //{
        //    case WhoShoot.Player:
        //        if (collision.CompareTag("µÐÈË"))
        //        {
        //            EventCenter.GetInstance().EventTrigger<float>("µÐÈË¿ÛÑª", hurt);
        //            PoolMgr.GetInstance().PushObj(name, gameObject);
        //        }
        //        break;
        //    case WhoShoot.Enemy:
        //        if (collision.CompareTag("Player"))
        //        {
        //            EventCenter.GetInstance().EventTrigger<float>("Íæ¼Ò¿ÛÑª", hurt);
        //            PoolMgr.GetInstance().PushObj(name, gameObject);
        //        }
        //        break;
        //}
    }
}
