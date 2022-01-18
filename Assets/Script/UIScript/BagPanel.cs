using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BagItem
{
    public int id;
    public int currentNum;
}
public class BagPanel : UIBase
{
    public List<BagItem> bagItems = new List<BagItem>();

    Transform bag;

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
                PoolMgr.GetInstance().PushObj(bag.GetChild(i).name, bag.GetChild(i).gameObject);
            }
        });
        EventCenter.GetInstance().AddEventListener<int>("当前金币", (x) =>
        {
            GetControl<Text>("CoinNum").text = x.ToString();
        });

        bag = GameTool.FindTheChild(gameObject, "背包界面");
    }
    private void Update()
    {
        UpdateBagItem();
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "CloseBagBto":
                UIMgr.GetInstance().HidePanel("BagPanel");
                break;
        }
    }
    private void AddItemInBag(ItemClick item)
    {
        // 没有达到最大值则叠加
        foreach (var it in bagItems)
        {
            if(it.currentNum < GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic,it.id).maxNum &&
                it.id == item.id)
            {
                it.currentNum++;
                return;
            }
        }

        // 之前相同的都满了，但是又有相同的道具，则添加
        PoolMgr.GetInstance().GetObj("Prefabs/ShopItem", (x) =>
        {
            x.transform.SetParent(bag);
            x.transform.localScale = Vector3.one;

            BagItem newBagItem = new BagItem();
            newBagItem.id = item.id;
            newBagItem.currentNum = 1;
            bagItems.Add(newBagItem);
        });
    }
    private void UpdateBagItem()
    {
        if (bagItems.Count == 0)
            return;

        for (int i = 0; i < bag.childCount; i++)
        {
            Debug.Log(bag.GetChild(i).gameObject.name);
            Image img = GameTool.FindTheChild(bag.GetChild(i).gameObject, "Img").GetComponent<Image>();
            Text num = GameTool.FindTheChild(bag.GetChild(i).gameObject, "ItemNum").GetComponent<Text>();

            img.sprite = ResMgr.GetInstance().Load<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, bagItems[i].id).path);
            num.text = bagItems[i].currentNum.ToString();

            //if (bagItems[i].currentNum <=0)
            //{
            //    bagItems.RemoveAt(i);
            //    PoolMgr.GetInstance().PushObj(bag.GetChild(i).name, bag.GetChild(i).gameObject);
            //    continue;
            //}

        }
    }
}
