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
                if (collision.CompareTag("敌人"))
                {
                    EventCenter.GetInstance().EventTrigger<float>("敌人扣血", hurt);
                    PoolMgr.GetInstance().PushObj(name, gameObject);
                }
                break;
            case WhoShoot.Enemy:
                if (collision.CompareTag("Player"))
                {
                    EventCenter.GetInstance().EventTrigger<float>("玩家扣血", hurt);
                    PoolMgr.GetInstance().PushObj(name, gameObject);
                }
                break;
        }

        // 碰到墙壁啥也没事
        if (collision.CompareTag("墙壁"))
        {
            PoolMgr.GetInstance().PushObj(name, gameObject);
        }

        //if (ws == WhoShoot.Player && collision.CompareTag("敌人"))
        //{
        //    // 碰到敌人，在消息中心存储<敌人扣血>消息
        //    EventCenter.GetInstance().EventTrigger<float>("敌人扣血", data.hurt);
        //    PoolMgr.GetInstance().PushObj(name, gameObject);
        //}
        //else if (ws == WhoShoot.Enemy && collision.CompareTag("Player"))
        //{
        //    // 碰到玩家，在消息中心存储<玩家扣血>消息
        //    EventCenter.GetInstance().EventTrigger<float>("玩家扣血", data.hurt);
        //    PoolMgr.GetInstance().PushObj(name, gameObject);
        //}
    }
}
