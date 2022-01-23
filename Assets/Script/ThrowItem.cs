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
                if (collision.CompareTag("µÐÈË"))
                {
                    EventCenter.GetInstance().EventTrigger<float>("µÐÈË¿ÛÑª", hurt);
                    PoolMgr.GetInstance().PushObj(name, gameObject);
                }
                break;
            case WhoShoot.Enemy:
                if (collision.CompareTag("Player"))
                {
                    EventCenter.GetInstance().EventTrigger<float>("Íæ¼Ò¿ÛÑª", hurt);
                    PoolMgr.GetInstance().PushObj(name, gameObject);
                }
                break;
        }

        // Åöµ½Ç½±ÚÉ¶Ò²Ã»ÊÂ
        if (collision.CompareTag("Ç½±Ú"))
        {
            PoolMgr.GetInstance().PushObj(name, gameObject);
        }
    }
}
