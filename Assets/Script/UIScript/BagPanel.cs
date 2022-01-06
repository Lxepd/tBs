using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : UIBase
{
    private void Start()
    {
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
        EventCenter.GetInstance().AddEventListener<int>("获得奖励", (x) =>
        {
            int num = int.Parse(GetControl<Text>("CoinNum").text);
            GetControl<Text>("CoinNum").text = (num + x).ToString();
        });
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
        Transform bag = GameTool.FindTheChild(gameObject, "背包界面");

        // 没有达到最大值则叠加
        for (int i = 0; i < bag.childCount; i++)
        {
            ItemClick ic = bag.GetChild(i).GetComponent<ItemClick>();
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
            x.transform.SetParent(bag);
            x.transform.localScale = Vector3.one;

            ItemClick ic = x.GetComponent<ItemClick>();
            ic.id = item.id;
            ic.currentNum = 1;

            x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, item.id).path);
        });
    }
}
