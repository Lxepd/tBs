using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    // Npc类型
    public NpcType type;

    [Header("道具商店售卖的东西")]
    public List<道具商人> items = new List<道具商人>();
    public List<道具商人> itemsCopy = new List<道具商人>();

    public List<装备商人> equipmentNpc = new List<装备商人>();

    private Timer reInit;
    [Header("商店刷新时间，最短30s")]
    public float reInitTime;
    [Header("检测玩家距离，最短1.5")]
    public float checkPlayerHereRadius = 1.5f;

    private bool playerHere;

    private void Start()
    {
        //data = GameTool.GetDicInfo(Datas.GetInstance().NpcDataDic, id);

        switch (type)
        {
            case NpcType.道具商人:
                itemsCopy = GameTool.Clone<道具商人>(items);
                EventCenter.GetInstance().AddEventListener<int>("NPC道具数量更新", (x) =>
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
            case NpcType.装备商人:
                //TODO
                break;
            case NpcType.工匠:
                //TODO
                break;
        }

    }
    private void FixedUpdate()
    {
        switch (type)
        {
            case NpcType.道具商人:
                if (playerHere = Physics2D.OverlapCircle(transform.position, checkPlayerHereRadius, LayerMask.GetMask("玩家")))
                {
                    EventCenter.GetInstance().EventTrigger<float>("刷新时间", reInit.nowTime);
                    EventCenter.GetInstance().EventTrigger<List<道具商人>>("道具商店", itemsCopy);
                }
                if (reInit.isTimeUp)
                {
                    itemsCopy.Clear();
                    itemsCopy = GameTool.Clone<道具商人>(items);
                }
                break;
            case NpcType.装备商人:
                break;
            case NpcType.工匠:
                break;
        }
    }
    // 初始化商店
    public void OpenShop()
    {
        switch (type)
        {
            case NpcType.道具商人:
                UIMgr.GetInstance().ShowPanel<ItemShopPanel>("ItemShopPanel", E_UI_Layer.Above);
                break;
            case NpcType.装备商人:
                break;
            case NpcType.工匠:
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
