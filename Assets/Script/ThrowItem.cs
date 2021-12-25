using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowItem : MonoBehaviour
{
    public int id;
    private ThrowItemData data;

    public ThrowItemData Data { get => data;}

    private void Start()
    {
        data = GameMgr.GetInstance().GetThrowItemInfo(id);
        Debug.Log(data.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            // 碰到墙壁啥也没事
            case "墙壁":
                PoolMgr.GetInstance().PushObj(name, gameObject);
                break;
            // 碰到敌人，在消息中心存储<敌人扣血>消息
            case "敌人":
                EventCenter.GetInstance().EventTrigger<ThrowItemData>("敌人扣血", data);
                PoolMgr.GetInstance().PushObj(name, gameObject);
                break;
        }
    }
}
