using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    // Npc����
    public NpcType type;

    [Header("�����̵������Ķ���")]
    public List<��������> items = new List<��������>();
    public List<��������> itemsCopy = new List<��������>();

    public List<װ������> equipmentNpc = new List<װ������>();

    private Timer reInit;
    [Header("�̵�ˢ��ʱ�䣬���30s")]
    public float reInitTime;
    [Header("�����Ҿ��룬���1.5")]
    public float checkPlayerHereRadius = 1.5f;

    private bool playerHere;

    private void Start()
    {
        //data = GameTool.GetDicInfo(Datas.GetInstance().NpcDataDic, id);

        switch (type)
        {
            case NpcType.��������:
                itemsCopy = GameTool.Clone<��������>(items);
                EventCenter.GetInstance().AddEventListener<int>("NPC������������", (x) =>
                {
                    if (!playerHere)
                        return;

                    foreach (var item in itemsCopy)
                    {
                        if (item.id == x)
                        {
                            item.num--;
                            return;
                        }
                    }
                });
                reInit = new Timer(Mathf.Max(30, reInitTime), true, true);
                break;
            case NpcType.װ������:
                //TODO
                break;
            case NpcType.����:
                //TODO
                break;
        }

    }
    private void FixedUpdate()
    {
        switch (type)
        {
            case NpcType.��������:
                if (playerHere = Physics2D.OverlapCircle(transform.position, checkPlayerHereRadius, LayerMask.GetMask("���")))
                {
                    EventCenter.GetInstance().EventTrigger<float>("ˢ��ʱ��", reInit.nowTime);
                    EventCenter.GetInstance().EventTrigger<List<��������>>("�����̵�", itemsCopy);
                }
                if (reInit.isTimeUp)
                {
                    itemsCopy.Clear();
                    itemsCopy = GameTool.Clone<��������>(items);
                }
                break;
            case NpcType.װ������:
                break;
            case NpcType.����:
                break;
        }
    }
    // ��ʼ���̵�
    public void OpenShop()
    {
        switch (type)
        {
            case NpcType.��������:
                UIMgr.GetInstance().ShowPanel<ItemShopPanel>("ItemShopPanel", E_UI_Layer.Above);
                break;
            case NpcType.װ������:
                break;
            case NpcType.����:
                UIMgr.GetInstance().ShowPanel<UpgradePanel>("UpgradePanel", E_UI_Layer.Above);
                break;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, checkPlayerHereRadius);
    }
}
