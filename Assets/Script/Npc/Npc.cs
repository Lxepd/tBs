using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc:MonoBehaviour
{
    public int id;
    public NpcData data;

    [HideInInspector] public List<道具> items = new List<道具>();

    [Header("检测玩家距离，最短1.5")]
    public float checkPlayerHereRadius = 1.5f;

    private bool playerHere;

    private Timer reInit;

    private void Start()
    {
        data = Datas.GetInstance().NpcDataDic[id];

        switch (data.type)
        {
            case NpcType.道具商人:
                items = GameTool.Clone<道具>(data.items);
                EventCenter.GetInstance().AddEventListener<int>("NPC道具数量更新", (x) =>
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
            case NpcType.装备商人:
                break;
            case NpcType.工匠:
                break;
        }

    }
    private void Update()
    {
        playerHere = Physics2D.OverlapCircle(transform.position, checkPlayerHereRadius, LayerMask.GetMask("玩家"));

        switch (data.type)
        {
            case NpcType.道具商人:
                if(playerHere)
                {
                    EventCenter.GetInstance().EventTrigger<float>("刷新时间", reInit.nowTime);
                    EventCenter.GetInstance().EventTrigger<List<道具>>("道具商店", items);
                }
                if (reInit.isTimeUp)
                {
                    items = GameTool.Clone<道具>(data.items);
                }
                break;
            case NpcType.装备商人:
                break;
            case NpcType.工匠:
                if (playerHere)
                {
                    EventCenter.GetInstance().EventTrigger<List<升级>>("武器升级", data.upgrades);
                }
                break;
        }
    }
    // 初始化商店
    public void OpenShop()
    {
        switch (data.type)
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