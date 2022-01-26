using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : UIBase
{
    Transform bag;

    private void Start()
    {
        bag = GameTool.FindTheChild(gameObject, "背包界面");

        // 注册<道具点击>消息
        EventCenter.GetInstance().AddEventListener<ItemClick>("成功购买的道具", (x) =>
        {
            AddItemInBag(x);
        });
        EventCenter.GetInstance().AddEventListener<bool>("背包清空", (x) =>
        {
            Transform bag = GameTool.FindTheChild(gameObject, "背包界面");

            for (int i = bag.childCount - 1; i >= 0; i--)
            {
                bag.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
                PoolMgr.GetInstance().PushObj(bag.GetChild(i).name, bag.GetChild(i).gameObject);
            }
        });
        EventCenter.GetInstance().AddEventListener<int>("道具使用消耗", (x) =>
        {
            for (int i = 0; i < bag.childCount; i++)
            {
                ItemClick ic = bag.GetChild(i).GetComponent<ItemClick>();
                if (ic.id == x)
                {
                    ic.currentNum--;
                    return;
                }
            }
        });
    }
    private void Update()
    {
        GetControl<Text>("CoinNum").text = Datas.GetInstance().CoinNum.ToString();
        for (int i = bag.childCount - 1; i >= 0; i--)
        {
            ItemClick ic = bag.GetChild(i).GetComponent<ItemClick>();
            if (ic.currentNum <= 0)
            {
                PoolMgr.GetInstance().PushObj(bag.GetChild(i).name, bag.GetChild(i).gameObject);
            }
        }
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "CloseBagBto":
                HideBagPanel();
                break;
        }
    }
    private void HideBagPanel()
    {
        Debug.Log("关闭背包！！");
        UIMgr.GetInstance().HidePanel("BagPanel");
    }
    private void AddItemInBag(ItemClick item)
    {

        // 没有达到最大值则叠加
        for (int i = 0; i < bag.childCount; i++)
        {
            ItemClick ic = bag.GetChild(i).GetComponent<ItemClick>();
            if (item.id == Datas.GetInstance().ItemDataDic[ic.id].id &&
                ic.currentNum < Datas.GetInstance().ItemDataDic[ic.id].maxNum)
            {
                ic.currentNum++;
                return;
            }
        }

        // 之前相同的都满了，但是又有相同的道具，则添加
        PoolMgr.GetInstance().GetObj("Prefabs/ShopItem", (x) =>
        {
            x.transform.SetParent(bag);
            x.transform.localScale = Vector3.one;

            ItemClick ic = x.GetComponent<ItemClick>();
            ic.id = item.id;
            ic.currentNum = 1;

            x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(Datas.GetInstance().ItemDataDic[item.id].path);
        });
    }
}
