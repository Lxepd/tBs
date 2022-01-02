using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 商店界面
/// </summary>
public class ShopPanel : UIBase
{
    // 点击的<道具>上的脚本
    private ItemClick item;
    // <道具>父物体
    private Transform itemParent;

    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<float>("刷新时间", (x) =>
         {
             GetControl<Text>("ReTimeText").text = "距离刷新时间：" + GameTool.SetTime(x);
         });
    }
    private void Update()
    {
        // 清空数量为0的道具
        ClearZeroItem();
    }
    /// <summary>
    /// 界面展开时执行
    /// </summary>
    public override void ShowMe()
    {
        // 注册<道具点击>消息
        EventCenter.GetInstance().AddEventListener<ItemClick>("商店物品", (x) =>
        {
            Debug.Log(x);
            item = x;
        });
        // 找到父物体
        itemParent = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "商店展示界面");

    }
    /// <summary>
    /// 界面隐藏时执行
    /// </summary>
    public override void HideMe()
    {
        // 挨个回收父物体下的道具，即商店内的道具逐个清除
        for (int i = itemParent.childCount - 1; i >= 0; i--)
        {
            itemParent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
            PoolMgr.GetInstance().PushObj(itemParent.GetChild(i).name, itemParent.GetChild(i).gameObject);
        }
        GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "ItemSelect").gameObject.SetActive(false);
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
                UIMgr.GetInstance().HidePanel("ShopPanel");
                break;
            // 商店道具购买按钮
            case "Bto_Buy":
                // 排除没有点击任何道具
                if (item == null)
                    return;

                Debug.Log("购买！！");
                // 给玩家背包里添加道具
                AddItem();
                break;
            // 商店道具出售按钮
            case "Bto_Sell":
                //TODO: 道具出售
                Debug.Log("售出！！");
                break;
        }
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
    /// <summary>
    /// 添加物品进玩家背包
    /// </summary>
    private void AddItem()
    {
        Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "背包界面");

        // 商品-1
        item.currentNum--;
        EventCenter.GetInstance().EventTrigger<int>("NPC道具数量更新", item.id);

        // 没有达到最大值则叠加
        for (int i = 0; i < content.childCount; i++)
        {
            ItemClick ic = content.GetChild(i).GetComponent<ItemClick>();
            if (item.id == GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, ic.id).id &&
                ic.currentNum < GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, ic.id).maxNum)
            {
                ic.currentNum++;
                return;
            }
        }

        // 之前相同的都满了，但是又有相同的道具，则添加
        PoolMgr.GetInstance().GetObj("Prefabs/ShopItem", (x) =>
         {
             x.transform.SetParent(content);
             x.transform.localScale = Vector3.one;

             ItemClick ic = x.GetComponent<ItemClick>();
             ic.id = item.id;
             ic.currentNum = 1;

             x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, item.id).path);
         });

    }
}
