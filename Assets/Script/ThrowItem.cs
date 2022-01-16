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
    [SerializeField] public float hurt;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (ws)
        {
            case WhoShoot.Player:
                if (collision.CompareTag("����"))
                {
                    EventCenter.GetInstance().EventTrigger<float>("���˿�Ѫ", hurt);
                    PoolMgr.GetInstance().PushObj(name, gameObject);
                }
                break;
            case WhoShoot.Enemy:
                if (collision.CompareTag("Player"))
                {
                    EventCenter.GetInstance().EventTrigger<float>("��ҿ�Ѫ", hurt);
                    PoolMgr.GetInstance().PushObj(name, gameObject);
                }
                break;
        }

        // ����ǽ��ɶҲû��
        if (collision.CompareTag("ǽ��"))
        {
            PoolMgr.GetInstance().PushObj(name, gameObject);
        }

        //if (ws == WhoShoot.Player && collision.CompareTag("����"))
        //{
        //    // �������ˣ�����Ϣ���Ĵ洢<���˿�Ѫ>��Ϣ
        //    EventCenter.GetInstance().EventTrigger<float>("���˿�Ѫ", data.hurt);
        //    PoolMgr.GetInstance().PushObj(name, gameObject);
        //}
        //else if (ws == WhoShoot.Enemy && collision.CompareTag("Player"))
        //{
        //    // ������ң�����Ϣ���Ĵ洢<��ҿ�Ѫ>��Ϣ
        //    EventCenter.GetInstance().EventTrigger<float>("��ҿ�Ѫ", data.hurt);
        //    PoolMgr.GetInstance().PushObj(name, gameObject);
        //}
    }
}
