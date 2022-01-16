using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    // NpcData
    public NpcData data;
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
        switch (type)
        {
            case NpcType.道具商人:
                data = GameTool.GetDicInfo(Datas.GetInstance().NpcDataDic, 13001);
                itemsCopy = GameTool.Clone<道具商人>(items);
                EventCenter.GetInstance().AddEventListener<int>("NPC道具数量更新", (x) =>
                {
                    if (!playerHere)
                        return;

                    ShopItemNumReduce(x);
                });
                reInit = new Timer(Mathf.Max(30, reInitTime), true, true);
                break;
            case NpcType.装备商人:
                data = GameTool.GetDicInfo(Datas.GetInstance().NpcDataDic, 13002);
                //TODO
                break;
            case NpcType.工匠:
                data = GameTool.GetDicInfo(Datas.GetInstance().NpcDataDic, 13003);
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
    public void InitShop()
    {
        switch (type)
        {
            case NpcType.道具商人:
                ItemNpc();
                break;
            case NpcType.装备商人:
                break;
            case NpcType.工匠:
                UIMgr.GetInstance().ShowPanel<UpgradePanel>("UpgradePanel", E_UI_Layer.Above);
                break;
        }
    }
    #region <<<   道具Npc   >>>
    private void ItemNpc()
    {
        UIMgr.GetInstance().ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Above, (y) =>
        {
            Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "商店展示界面");
            foreach (道具商人 item in itemsCopy)
            {
                int[] num = new int[]
                {
                        // 有多少个满的
                        item.num / GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic,item.id).maxNum,
                        // 多出来几个
                        item.num % GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic,item.id).maxNum
                };

                if (num[0] != 0)
                {
                    for (int i = 0; i < num[0]; i++)
                    {
                        CreateShopItem(content, item, GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, item.id).maxNum);
                    }
                }
                if (num[1] != 0)
                {
                    CreateShopItem(content, item, num[1]);
                }
            }

        });
    }
    /// <summary>
    /// 生成商店物品
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="item">道具</param>
    /// <param name="num">数量</param>
    private void CreateShopItem(Transform parent, 道具商人 item, int num)
    {
        PoolMgr.GetInstance().GetObj("Prefabs/ShopItem", (x) =>
        {
            x.transform.SetParent(parent);
            x.transform.localScale = Vector3.one;

            ItemClick ic = x.GetComponent<ItemClick>();
            ic.id = item.id;
            ic.currentNum = num;
            x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, item.id).path);
            //x.transform.Find("ItemNum").GetComponent<Text>().text = num.ToString();

        });
    }
    private void ShopItemNumReduce(int _id)
    {
        foreach (var item in itemsCopy)
        {
            if (item.id == _id)
            {
                item.num--;
                return;
            }
        }
    }
    #endregion
    // TODO
    #region <<<   装备Npc   >>>

    #endregion
    // TODO
    #region <<<   工匠Npc   >>>

    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, checkPlayerHereRadius);
    }
}
