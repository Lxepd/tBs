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
            // ����ǽ��ɶҲû��
            case "ǽ��":
                PoolMgr.GetInstance().PushObj(name, gameObject);
                break;
            // �������ˣ�����Ϣ���Ĵ洢<���˿�Ѫ>��Ϣ
            case "����":
                EventCenter.GetInstance().EventTrigger<ThrowItemData>("���˿�Ѫ", data);
                PoolMgr.GetInstance().PushObj(name, gameObject);
                break;
        }
    }
}
