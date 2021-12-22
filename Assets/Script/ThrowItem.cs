using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowItem : MonoBehaviour
{
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
                EventCenter.GetInstance().EventTrigger<float>("���˿�Ѫ", 1f);
                PoolMgr.GetInstance().PushObj(name, gameObject);
                break;
        }
    }
}
