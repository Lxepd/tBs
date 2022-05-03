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
        mask = LayerMask.GetMask("����");
        boxSize = GetComponent<BoxCollider2D>().size;
    }

    private void Update()
    {
        switch (ws)
        {
            case WhoShoot.Player:
                mask |= LayerMask.GetMask("����");
                break;
            case WhoShoot.Enemy:
                mask |= LayerMask.GetMask("���");
                break;
        }

        col = Physics2D.OverlapBox(transform.position, boxSize * 2, 0, mask);
        if (col == null)
            return;

        switch (ws)
        {
            case WhoShoot.Player:
                if (col.CompareTag("����"))
                {
                    EventCenter.GetInstance().EventTrigger<float>("���˿�Ѫ", hurt);
                    PoolMgr.GetInstance().PushObj(name, gameObject);

                    MusicMgr.GetInstance().PlaySound("HIT_METAL_WRENCH_HEAVIEST_02", false);
                    MusicMgr.GetInstance().ChangeSoundValue(.3f);
                }
                break;
            case WhoShoot.Enemy:
                if (col.CompareTag("Player"))
                {
                    EventCenter.GetInstance().EventTrigger<float>("��ҿ�Ѫ", hurt);
                    PoolMgr.GetInstance().PushObj(name, gameObject);

                    MusicMgr.GetInstance().PlaySound("HIT_METAL_WRENCH_HEAVIEST_02", false);
                    MusicMgr.GetInstance().ChangeSoundValue(.3f);
                }
                break;
        }

        // ����ǽ��ɶҲû��
        if (col.CompareTag("ǽ��"))
        {
            PoolMgr.GetInstance().PushObj(name, gameObject);
            return;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //// ����ǽ��ɶҲû��
        //if (collision.CompareTag("ǽ��"))
        //{
        //    PoolMgr.GetInstance().PushObj(name, gameObject);
        //    return;
        //}

        //switch (ws)
        //{
        //    case WhoShoot.Player:
        //        if (collision.CompareTag("����"))
        //        {
        //            EventCenter.GetInstance().EventTrigger<float>("���˿�Ѫ", hurt);
        //            PoolMgr.GetInstance().PushObj(name, gameObject);
        //        }
        //        break;
        //    case WhoShoot.Enemy:
        //        if (collision.CompareTag("Player"))
        //        {
        //            EventCenter.GetInstance().EventTrigger<float>("��ҿ�Ѫ", hurt);
        //            PoolMgr.GetInstance().PushObj(name, gameObject);
        //        }
        //        break;
        //}
    }
}
