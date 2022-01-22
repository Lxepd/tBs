using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc:MonoBehaviour
{
    public int id;
    public NpcData data;

    [HideInInspector] public List<����> items = new List<����>();

    [Header("�����Ҿ��룬���1.5")]
    public float checkPlayerHereRadius = 1.5f;

    private bool playerHere;

    private Timer reInit;

    private void Start()
    {
        data = Datas.GetInstance().NpcDataDic[id];

        switch (data.type)
        {
            case NpcType.��������:
                items = GameTool.Clone<����>(data.items);
                EventCenter.GetInstance().AddEventListener<int>("NPC������������", (x) =>
                {
                    if (!playerHere)
                        return;

                    foreach (var item in items)
                    {
                        if (item.id == x)
                        {
                            item.num--;
                            return;
                        }
                    }
                });
                reInit = new Timer(Mathf.Max(30, data.shopReTime), true, true);
                break;
            case NpcType.װ������:
                break;
            case NpcType.����:
                break;
        }

    }
    private void Update()
    {
        playerHere = Physics2D.OverlapCircle(transform.position, checkPlayerHereRadius, LayerMask.GetMask("���"));

        switch (data.type)
        {
            case NpcType.��������:
                if(playerHere)
                {
                    EventCenter.GetInstance().EventTrigger<float>("ˢ��ʱ��", reInit.nowTime);
                    EventCenter.GetInstance().EventTrigger<List<����>>("�����̵�", items);
                }
                if (reInit.isTimeUp)
                {
                    items = GameTool.Clone<����>(data.items);
                }
                break;
            case NpcType.װ������:
                break;
            case NpcType.����:
                if (playerHere)
                {
                    EventCenter.GetInstance().EventTrigger<List<����>>("��������", data.upgrades);
                }
                break;
        }
    }
    // ��ʼ���̵�
    public void OpenShop()
    {
        switch (data.type)
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