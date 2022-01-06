using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ״̬����
/// </summary>
public class StatePanel : UIBase
{
    // �������
    private PlayerData data;
    // ��ǰѪ��
    private float currentHp;
    // ��ű�������Ʒ��id������
    Dictionary<int, int> BagItem = new Dictionary<int, int>();
    // ���id��˳���
    List<int> BagItemIDList = new List<int>();
    // �±�
    int index = 0;

    private void Start()
    {
        // ��ȡ����
        EventCenter.GetInstance().AddEventListener<int>("��ɫ��Ϣ", (x) =>
        {
            data = GameTool.GetDicInfo(Datas.GetInstance().PlayerDataDic, x);
            // ��ʼ��UI
            InitStateUI();
        });
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("��ҿ�Ѫ", (x) =>
        {
            ChangeHp((int)x.hurt);
        });
        EventCenter.GetInstance().AddEventListener<int>("���������", (x) =>
        {
            data = null;
            BagItem.Clear();
            BagItemIDList.Clear();
            index = 0;

            GetControl<Image>("Item").sprite = null;
            GetControl<Image>("Item").color = new Color(43 / 255f, 43 / 255f, 43 / 255f);
            GetControl<Text>("ItemNum").text = "";
        });
    }
    void Update()
    {
        if (data == null)
            return;

        // ����Ѫ��
        GetControl<Image>("HpBar").fillAmount = currentHp / data.MaxHp;
        GetBagItem();
        SetBagItem(index);
    }
    /// <summary>
    /// ��Ӧ��ť���
    /// </summary>
    /// <param name="btnName"></param>
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "Bto_List":
                // չ��<�б�>����
                UIMgr.GetInstance().ShowPanel<ListPanel>("ListPanel", E_UI_Layer.Above);
                break;
            // �������İ�ť
            // ����
            case "Bto_Left":
                Debug.Log("��������");
                // �ų�����û���κ����ĵ��ߵ����
                if (BagItemIDList.Count == 0)
                    return;
                if (index > 0)
                {
                    --index;
                }
                break;
            // ����
            case "Bto_Right":
                Debug.Log("��������");
                // �ų�����û���κ����ĵ��ߵ����
                if (BagItemIDList.Count == 0)
                    return;
                if (index < BagItemIDList.Count - 1)
                {
                    ++index;
                }
                break;
            // ʹ��
            case "Bto_":
                Debug.Log("����ʹ��");        
                break;

        }
    }
    /// <summary>
    /// UI��ʼ��
    /// </summary>
    private void InitStateUI()
    {
        // ��ȡ���Ѫ��
        currentHp = data.MaxHp;
        // ��ʼ���ı�
        GetControl<Text>("CurrentHp").text = data.MaxHp.ToString();
        GetControl<Text>("MaxHp").text = data.MaxHp.ToString();

    }
    /// <summary>
    /// ��Ѫִ��
    /// </summary>
    /// <param name="hurt">�۳���Ѫ��</param>
    private void ChangeHp(int hurt)
    {
        currentHp -= hurt;

        if (currentHp <= 0)
            currentHp = 0;

        GetControl<Text>("CurrentHp").text = currentHp.ToString();
    }
    private void GetBagItem()
    {
        Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "��������");
        Dictionary<int, int> test = new Dictionary<int, int>();
        // ��ȡ�����ڵĿ����ĵ���
        for (int i = 0; i < content.childCount; i++)
        {
            ItemClick ic = content.GetChild(i).GetComponent<ItemClick>();
            ItemData itemd = GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, ic.id);
            // �ų�������<������Ʒ>
            if (itemd.itemType == ItemType.������Ʒ)
                continue;
            // ����ֵ���û��
            // �����������
            if (test.ContainsKey(ic.id))
            {
                test[ic.id] += ic.currentNum;
            }
            // �������
            else
            {
                test.Add(ic.id, ic.currentNum);
                BagItemIDList.Add(ic.id);
            }
        }

        BagItem = test;
    }
    /// <summary>
    /// ��<һ��������Ʒ>��<���ظ�ʹ������Ʒ>����״̬���ϵ���Ʒ������
    /// </summary>
    /// <param name="index"></param>
    private void SetBagItem(int index)
    {
        if (BagItemIDList.Count == 0)
            return;   

        ResMgr.GetInstance().LoadAsync<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, BagItemIDList[index]).path, (x) =>
        {
            GetControl<Image>("Item").sprite = x;
            GetControl<Image>("Item").color = new Color(255f, 255f, 255f);
            GetControl<Text>("ItemNum").text = BagItem[BagItemIDList[index]].ToString();
        });
    }
}
