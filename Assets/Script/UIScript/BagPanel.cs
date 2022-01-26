using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : UIBase
{
    Transform bag;

    private void Start()
    {
        bag = GameTool.FindTheChild(gameObject, "��������");

        // ע��<���ߵ��>��Ϣ
        EventCenter.GetInstance().AddEventListener<ItemClick>("�ɹ�����ĵ���", (x) =>
        {
            AddItemInBag(x);
        });
        EventCenter.GetInstance().AddEventListener<bool>("�������", (x) =>
        {
            Transform bag = GameTool.FindTheChild(gameObject, "��������");

            for (int i = bag.childCount - 1; i >= 0; i--)
            {
                bag.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
                PoolMgr.GetInstance().PushObj(bag.GetChild(i).name, bag.GetChild(i).gameObject);
            }
        });
        EventCenter.GetInstance().AddEventListener<int>("����ʹ������", (x) =>
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
        Debug.Log("�رձ�������");
        UIMgr.GetInstance().HidePanel("BagPanel");
    }
    private void AddItemInBag(ItemClick item)
    {

        // û�дﵽ���ֵ�����
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

        // ֮ǰ��ͬ�Ķ����ˣ�����������ͬ�ĵ��ߣ������
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
