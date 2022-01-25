using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �̵����
/// </summary>
public class ItemShopPanel : UIBase
{
    // �����<����>�ϵĽű�
    private ItemClick item;
    // <����>������
    private Transform itemParent;

    private int nowCoin;

    bool isInit;
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<float>("ˢ��ʱ��", (x) =>
         {
             GetControl<Text>("ReTimeText").text = "����ˢ��ʱ�䣺" + GameTool.SetTime(x);
         });
        // ע��<���ߵ��>��Ϣ
        EventCenter.GetInstance().AddEventListener<ItemClick>("�̵���Ʒ", (x) =>
        {
            item = x;
        });
        EventCenter.GetInstance().AddEventListener<List<����>>("�����̵�", (x) =>
        {
            if (!isInit)
            {
                isInit = true;
                InitShop(x);
            }
        });
        EventCenter.GetInstance().AddEventListener<int>("��ǰ���", (x) =>
        {
            nowCoin = x;
            GetControl<Text>("CoinNum").text = nowCoin.ToString();
        });

        // �ҵ�������
        itemParent = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "�̵�չʾ����");
    }
    private void Update()
    {
        // �������Ϊ0�ĵ���
        ClearZeroItem();
    }
    public override void ShowMe()
    {
        isInit = false;
    }
    /// <summary>
    /// ��������ʱִ��
    /// </summary>
    public override void HideMe()
    {
        // �������ո������µĵ��ߣ����̵��ڵĵ���������
        for (int i = itemParent.childCount - 1; i >= 0; i--)
        {
            PoolMgr.GetInstance().PushObj(itemParent.GetChild(i).name, itemParent.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// ��Ӧ��ť���
    /// </summary>
    /// <param name="btnName">�ý����µİ�ť��</param>
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            // �ر��̵갴ť
            case "Bto_ShopQuit":
                // ����<�̵�>����
                UIMgr.GetInstance().HidePanel("ItemShopPanel");
                break;
            // �̵���߹���ť
            case "Bto_Buy":
                // �ų�û�е���κε���
                if (item == null || nowCoin < Datas.GetInstance().ItemDataDic[item.id].cost)
                    return;
                // ��Ʒ-1
                item.currentNum--;
                EventCenter.GetInstance().EventTrigger<int>("NPC������������", item.id);
                EventCenter.GetInstance().EventTrigger<ItemClick>("�ɹ�����ĵ���", item);
                EventCenter.GetInstance().EventTrigger<int>("��ý��", -Datas.GetInstance().ItemDataDic[item.id].cost);
                break;
            // �̵���߳��۰�ť
            case "Bto_Sell":
                //TODO: ���߳���
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
    /// ��ʼ���̵�
    /// </summary>
    /// <param name="shop"></param>
    private void InitShop(List<����> shop)
    {
        Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "�̵�չʾ����");
        foreach (���� item in shop)
        {
            int[] num = new int[]
            {
                // �ж��ٸ�����
                item.num / Datas.GetInstance().ItemDataDic[item.id].maxNum,
                // ���������
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
    /// �����̵���Ʒ
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="item">����</param>
    /// <param name="num">����</param>
    private void CreateShopItem(Transform parent, ���� item, int num)
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
    /// ���Ϊ0�ĵ�����Ʒ
    /// </summary>
    private void ClearZeroItem()
    {
        // ����Ʒ����<=0��ʱ�򣬻��ճػ���Ԥ����
        for (int i = 0; i < itemParent.childCount; i++)
        {
            ItemClick ic = itemParent.GetChild(i).GetComponent<ItemClick>();
            if (ic.currentNum <= 0)
            {
                PoolMgr.GetInstance().PushObj(itemParent.GetChild(i).name, itemParent.GetChild(i).gameObject);
                EventCenter.GetInstance().EventTrigger<ItemClick>("�̵���Ʒ", null);
            }
        }
    }

}
