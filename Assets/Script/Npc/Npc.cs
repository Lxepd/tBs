using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum NpcType
{
    None,
    道具商人,
    装备商人,
    工匠
}
[Serializable]
public class 道具商人
{
    // 道具ID
    public int id;
    // 道具数量
    public int num;

    public 道具商人(int id,int num)
    {
        this.id = id;
        this.num = num;
    }
}
[Serializable]
public class 装备商人
{
    public int test;
}
[Serializable]
public class 工匠
{
    public int test;
}
public class Npc : MonoBehaviour
{
    // NpcData
    public NpcData data;
    // Npc类型
    public NpcType type;
    
    public List<道具商人> itemNpc = new List<道具商人>();
    public Dictionary<int, int> itemDic = new Dictionary<int, int>();

    public List<装备商人> equipmentNpc = new List<装备商人>();
    public List<工匠> craftsMan = new List<工匠>();

    private float checkRadius = 2f;
    Timer reInit;

    private void Start()
    {
        switch (type)
        {
            case NpcType.道具商人:
                data = GameMgr.GetInstance().GetNpcInfo(13001);
                foreach (var item in itemNpc)
                {
                    itemDic.Add(item.id, item.num);
                }
                EventCenter.GetInstance().AddEventListener<int>("NPC道具数量更新", (x) =>
                {
                    foreach (var item in itemNpc)
                    {
                        if (item.id == x)
                        {
                            item.num--;
                            return;
                        }
                    }
                });
                break;
            case NpcType.装备商人:
                data = GameMgr.GetInstance().GetNpcInfo(13002);
                //TODO
                break;
            case NpcType.工匠:
                data = GameMgr.GetInstance().GetNpcInfo(13003);
                //TODO
                break;
        }

        reInit = new Timer(30, true, true);
    }
    private void FixedUpdate()
    {
        EventCenter.GetInstance().EventTrigger<float>("刷新时间", reInit.nowTime);
        if (reInit.isTimeUp)
        {
            switch (type)
            {
                case NpcType.道具商人:
                    itemNpc.Clear();
                    foreach (var key in itemDic)
                    {
                        itemNpc.Add(new 道具商人(key.Key, key.Value));
                    }
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
            foreach (道具商人 item in itemNpc)
            {
                int[] num = new int[]
                {
                        // 有多少个满的
                        item.num / GameMgr.GetInstance().GetItemInfo(item.id).maxNum,
                        // 多出来几个
                        item.num % GameMgr.GetInstance().GetItemInfo(item.id).maxNum
                };

                if (num[0] != 0)
                {
                    for (int i = 0; i < num[0]; i++)
                    {
                        CreateShopItem(content, item, GameMgr.GetInstance().GetItemInfo(item.id).maxNum);
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
            x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(GameMgr.GetInstance().GetItemInfo(item.id).path);
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
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
#endif
}
