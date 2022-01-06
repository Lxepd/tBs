using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : UIBase
{
    private void Start()
    {
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
        EventCenter.GetInstance().AddEventListener<int>("��ý���", (x) =>
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
        Debug.Log("�رձ�������");
        UIMgr.GetInstance().HidePanel("BagPanel");
    }
    private void AddItemInBag(ItemClick item)
    {
        Transform bag = GameTool.FindTheChild(gameObject, "��������");

        // û�дﵽ���ֵ�����
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

        // ֮ǰ��ͬ�Ķ����ˣ�����������ͬ�ĵ��ߣ������
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
