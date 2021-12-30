using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : UIBase
{
    public ItemClick item;

    public void Start()
    {
        EventCenter.GetInstance().AddEventListener<ItemClick>("�̵���Ʒ", (x) =>
        {
            Debug.Log(x);
            item = x;
        });
    }
    public override void HideMe()
    {
        Transform itemParent = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "�̵�չʾ����");
        for (int i = itemParent.childCount - 1; i >= 0; i--)
        {
            itemParent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
            PoolMgr.GetInstance().PushObj(itemParent.GetChild(i).name, itemParent.GetChild(i).gameObject);
        }
        GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "ItemSelect").gameObject.SetActive(false);
    }
    protected override void OnClick(string btnName)
    {
        switch(btnName)
        {
            case "Bto_ShopQuit":    
                UIMgr.GetInstance().HidePanel("ShopPanel");
                break;
            case "Bto_Buy":
                if (item == null)
                    return;

                Debug.Log("���򣡣�");
                AddItem();
                break;
            case "Bto_Sell":
                Debug.Log("�۳�����");
                break;
        }
    }
    private void AddItem()
    {
        Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "��������");


        // ��Ʒ-1
        // ����Ʒ����<=0��ʱ�򣬻��ճػ���Ԥ����
        if (--item.currentNum < 0)
        {
            item.transform.GetChild(0).GetComponent<Image>().sprite = null;
            PoolMgr.GetInstance().PushObj(item.gameObject.name, item.gameObject);
            EventCenter.GetInstance().EventTrigger<ItemClick>("�̵���Ʒ", null);
            return;
        }

        // û�дﵽ���ֵ�����
        for (int i = 0; i < content.childCount; i++)
        {
            ItemClick ic = content.GetChild(i).GetComponent<ItemClick>();
            if (item.id == GameMgr.GetInstance().GetItemInfo(ic.id).id &&
                ic.currentNum < GameMgr.GetInstance().GetItemInfo(ic.id).maxNum)
            {
                ic.currentNum++;
                return;
            }
        }


        // ֮ǰ��ͬ�Ķ����ˣ�����������ͬ�ĵ��ߣ������
        PoolMgr.GetInstance().GetObj("Prefabs/ShopItem", (x) =>
         {
             x.transform.SetParent(content);
             x.transform.localScale = Vector3.one;

             ItemClick ic = x.GetComponent<ItemClick>();
             ic.id = item.id;
             ic.currentNum = 1;

             x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(GameMgr.GetInstance().GetItemInfo(item.id).path);
         });



    }
}
