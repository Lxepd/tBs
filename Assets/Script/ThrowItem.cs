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
    public int id;
    private ThrowItemData data;
    [HideInInspector] public WhoShoot ws;

    public ThrowItemData Data { get => data;}

    private void Start()
    {
        data = GameTool.GetDicInfo(Datas.GetInstance().ThrowItemDataDic, id);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ����ǽ��ɶҲû��
        if (collision.CompareTag("ǽ��"))
        {
            PoolMgr.GetInstance().PushObj(name, gameObject);
            return;
        }


        if (ws == WhoShoot.Player && collision.CompareTag("����"))
        {
            // �������ˣ�����Ϣ���Ĵ洢<���˿�Ѫ>��Ϣ
            EventCenter.GetInstance().EventTrigger<float>("���˿�Ѫ", data.hurt);
            PoolMgr.GetInstance().PushObj(name, gameObject);
        }
        else if (ws == WhoShoot.Enemy && collision.CompareTag("Player"))
        {
            // ������ң�����Ϣ���Ĵ洢<��ҿ�Ѫ>��Ϣ
            EventCenter.GetInstance().EventTrigger<float>("��ҿ�Ѫ", data.hurt);
            PoolMgr.GetInstance().PushObj(name, gameObject);
            Debug.Log(data.hurt);
        }
    }
}
