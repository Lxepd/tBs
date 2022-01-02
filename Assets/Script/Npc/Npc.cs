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
    public List<工匠> craftsMan = new List<工匠>();

    Timer reInit;

    private void Start()
    {
        switch (type)
        {
            case NpcType.道具商人:
                data = GameTool.GetDicInfo(Datas.GetInstance().NpcDataDic, 13001);
                itemsCopy = GameTool.Clone<道具商人>(items);
                EventCenter.GetInstance().AddEventListener<int>("NPC道具数量更新", (x) =>
                {
                    foreach (var item in itemsCopy)
                    {
                        if (item.id == x)
                        {
                            item.num--;
                            return;
                        }
                    }
                });
                reInit = new Timer(Mathf.Max(30, ReTime.商人刷新时间), true, true);
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
        EventCenter.GetInstance().EventTrigger<float>("刷新时间", reInit.nowTime);
        if (reInit.isTimeUp)
        {
            switch (type)
            {
                case NpcType.道具商人:
                    itemsCopy.Clear();
                    itemsCopy = GameTool.Clone<道具商人>(items);
                    break;
                case NpcType.装备商人:
                    break;
                case NpcType.工匠:
                    break;
            }
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
    #endregion
    // TODO
    #region <<<   装备Npc   >>>

    #endregion
    // TODO
    #region <<<   工匠Npc   >>>

    #endregion
}
