using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 商店界面
/// </summary>
public class ItemShopPanel : UIBase
{
    // 点击的<道具>上的脚本
    private ItemClick item;
    // <道具>父物体
    private Transform itemParent;

    private int nowCoin;

    bool isInit;
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<float>("刷新时间", (x) =>
         {
             GetControl<Text>("ReTimeText").text = "距离刷新时间：" + GameTool.SetTime(x);
         });
        // 注册<道具点击>消息
        EventCenter.GetInstance().AddEventListener<ItemClick>("商店物品", (x) =>
        {
            item = x;
        });
        EventCenter.GetInstance().AddEventListener<List<道具>>("道具商店", (x) =>
        {
            if (!isInit)
            {
                isInit = true;
                InitShop(x);
            }
        });
        EventCenter.GetInstance().AddEventListener<int>("当前金币", (x) =>
        {
            nowCoin = x;
            GetControl<Text>("CoinNum").text = nowCoin.ToString();
        });

        // 找到父物体
        itemParent = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "商店展示界面");
    }
    private void Update()
    {
        // 清空数量为0的道具
        ClearZeroItem();
    }
    public override void ShowMe()
    {
        isInit = false;
    }
    /// <summary>
    /// 界面隐藏时执行
    /// </summary>
    public override void HideMe()
    {
        // 挨个回收父物体下的道具，即商店内的道具逐个清除
        for (int i = itemParent.childCount - 1; i >= 0; i--)
        {
            PoolMgr.GetInstance().PushObj(itemParent.GetChild(i).name, itemParent.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 对应按钮点击
    /// </summary>
    /// <param name="btnName">该界面下的按钮名</param>
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            // 关闭商店按钮
            case "Bto_ShopQuit":
                // 隐藏<商店>界面
                UIMgr.GetInstance().HidePanel("ItemShopPanel");
                break;
            // 商店道具购买按钮
            case "Bto_Buy":
                // 排除没有点击任何道具
                if (item == null || nowCoin < Datas.GetInstance().ItemDataDic[item.id].cost)
                    return;
                // 商品-1
                item.currentNum--;
                EventCenter.GetInstance().EventTrigger<int>("NPC道具数量更新", item.id);
                EventCenter.GetInstance().EventTrigger<ItemClick>("成功购买的道具", item);
                EventCenter.GetInstance().EventTrigger<int>("获得金币", -Datas.GetInstance().ItemDataDic[item.id].cost);
                break;
            // 商店道具出售按钮
            case "Bto_Sell":
                //TODO: 道具出售
                for (int i = 0; i < itemParent.childCount; i++)
                {
                    ItemClick ic = itemParent.GetChild(i).GetComponent<ItemClick>();

                    if (item.id != ic.id)
                        continue;

                    if (ic.currentNum <= Datas.GetInstance().ItemDataDic[ic.id].maxNum)
                    {
                        
                    }
                }
                break;
        }
    }
    /// <summary>
    /// 初始化商店
    /// </summary>
    /// <param name="shop"></param>
    private void InitShop(List<道具> shop)
    {
        Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "商店展示界面");
        foreach (道具 item in shop)
        {
            int[] num = new int[]
            {
                // 有多少个满的
                item.num / Datas.GetInstance().ItemDataDic[item.id].maxNum,
                // 多出来几个
                item.num % Datas.GetInstance().ItemDataDic[item.id].maxNum
            };

            if (num[0] != 0)
            {
                for (int i = 0; i < num[0]; i++)
                {
                    CreateShopItem(content, item, Datas.GetInstance().ItemDataDic[item.id].maxNum);
                }
            }
            if (num[1] != 0)
            {
                CreateShopItem(content, item, num[1]);
            }
        }
    }
    /// <summary>
    /// 生成商店物品
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="item">道具</param>
    /// <param name="num">数量</param>
    private void CreateShopItem(Transform parent, 道具 item, int num)
    {
        PoolMgr.GetInstance().GetObj("Prefabs/ShopItem", (x) =>
        {
            x.transform.SetParent(parent);
            x.transform.localScale = Vector3.one;

            ItemClick ic = x.GetComponent<ItemClick>();
            ic.id = item.id;
            ic.currentNum = num;
            x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(Datas.GetInstance().ItemDataDic[item.id].path);
            //x.transform.Find("ItemNum").GetComponent<Text>().text = num.ToString();

        });
    }
    /// <summary>
    /// 清除为0的道具商品
    /// </summary>
    private void ClearZeroItem()
    {
        // 当商品数量<=0的时候，回收池回收预制体
        for (int i = 0; i < itemParent.childCount; i++)
        {
            ItemClick ic = itemParent.GetChild(i).GetComponent<ItemClick>();
            if (ic.currentNum <= 0)
            {
                PoolMgr.GetInstance().PushObj(itemParent.GetChild(i).name, itemParent.GetChild(i).gameObject);
                EventCenter.GetInstance().EventTrigger<ItemClick>("商店物品", null);
            }
        }
    }

}
