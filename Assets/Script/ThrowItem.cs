using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            // 碰到墙壁啥也没事
            case "墙壁":
                PoolMgr.GetInstance().PushObj("Prefabs/石子", gameObject);
                break;
            // 碰到敌人，在消息中心存储<敌人扣血>消息
            case "敌人":
                EventCenter.GetInstance().EventTrigger<float>("敌人扣血", 1f);
                PoolMgr.GetInstance().PushObj("Prefabs/石子", gameObject);
                break;
        }
    }
}
