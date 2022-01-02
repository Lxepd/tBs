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
        // ע��<���ߵ��>��Ϣ
        EventCenter.GetInstance().AddEventListener<ItemClick>("�̵���Ʒ", (x) =>
        {
            Debug.Log(x);
            item = x;
        });
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

                Debug.Log("���򣡣�");
                // ����ұ�������ӵ���
                AddItem();
                break;
            // �̵���߳��۰�ť
            case "Bto_Sell":
                //TODO: ���߳���
                Debug.Log("�۳�����");
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
    /// <summary>
    /// �����Ʒ����ұ���
    /// </summary>
    private void AddItem()
    {
        Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "��������");

        // ��Ʒ-1
        item.currentNum--;
        EventCenter.GetInstance().EventTrigger<int>("NPC������������", item.id);

        // û�дﵽ���ֵ�����
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

        // ֮ǰ��ͬ�Ķ����ˣ�����������ͬ�ĵ��ߣ������
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
