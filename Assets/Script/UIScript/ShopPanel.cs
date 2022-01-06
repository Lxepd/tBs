using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �̵����
/// </summary>
public class ShopPanel : UIBase
{
    // �����<����>�ϵĽű�
    private ItemClick item;
    // <����>������
    private Transform itemParent;

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
    }
    private void Update()
    {
        // �������Ϊ0�ĵ���
        ClearZeroItem();
    }
    /// <summary>
    /// ����չ��ʱִ��
    /// </summary>
    public override void ShowMe()
    { 
        // �ҵ�������
        itemParent = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "�̵�չʾ����");
    }
    /// <summary>
    /// ��������ʱִ��
    /// </summary>
    public override void HideMe()
    {
        // �������ո������µĵ��ߣ����̵��ڵĵ���������
        for (int i = itemParent.childCount - 1; i >= 0; i--)
        {
            itemParent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
            PoolMgr.GetInstance().PushObj(itemParent.GetChild(i).name, itemParent.GetChild(i).gameObject);
        }
        GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "ItemSelect").gameObject.SetActive(false);
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
                UIMgr.GetInstance().HidePanel("ShopPanel");
                break;
            // �̵���߹���ť
            case "Bto_Buy":
                // �ų�û�е���κε���
                if (item == null)
                    return;
                // ��Ʒ-1
                item.currentNum--;
                EventCenter.GetInstance().EventTrigger<int>("NPC������������", item.id);
                EventCenter.GetInstance().EventTrigger<ItemClick>("�ɹ�����ĵ���", item);
                break;
            // �̵���߳��۰�ť
            case "Bto_Sell":
                //TODO: ���߳���
                for (int i = 0; i < itemParent.childCount; i++)
                {
                    ItemClick ic = itemParent.GetChild(i).GetComponent<ItemClick>();

                    if (item.id != ic.id)
                        continue;

                    if (ic.currentNum <= GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic,ic.id).maxNum)
                    {
                        
                    }
                }
                break;
        }
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
