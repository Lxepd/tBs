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
        EventCenter.GetInstance().AddEventListener<PlayerData>("��ɫ��ʼ", (x) =>
        {
            data = x;
            // ��ʼ��UI
            InitStateUI();
        });
        EventCenter.GetInstance().AddEventListener<float>("��ҿ�Ѫ", (x) =>
        {
            //MusicMgr.GetInstance().PlaySound("damaged1", false);
            ChangeHp((int)x);
        });
        EventCenter.GetInstance().AddEventListener<ItemClick>("�ɹ�����ĵ���", (x) =>
        {
            GetBagItem(x);
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
        EventCenter.GetInstance().AddEventListener("��ɫ�ָ�", () =>
         {
             // ��ʼ��UI
             InitStateUI();
         });
    }
    void Update()
    {
        if (data == null)
            return;

        // ����Ѫ��
        GetControl<Text>("CurrentHp").text = currentHp.ToString();
        GetControl<Image>("HpBar").fillAmount = Mathf.Lerp(GetControl<Image>("HpBar").fillAmount, currentHp / data.MaxHp, Time.deltaTime * 10f);

        List<int> zeroList = new List<int>();
        foreach (var item in BagItem)
        {
            if(item.Value <= 0)
            {
                zeroList.Add(item.Key);
            }
        }
        for (int i = 0; i < zeroList.Count; i++)
        {
            BagItem.Remove(zeroList[i]);
            BagItemIDList.Remove(zeroList[i]);
        }

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
                int itemId = BagItemIDList[index];
                switch (Datas.GetInstance().ItemDataDic[itemId].actType)
                {
                    case ItemActType.Ѫ���ָ�:
                        Debug.Log(itemId);
                        currentHp += Datas.GetInstance().ItemDataDic[BagItemIDList[index]].recovery;
                        break;
                    case ItemActType.����ǿ��:
                        break;
                }

                switch (Datas.GetInstance().ItemDataDic[itemId].itemType)
                {
                    case ItemType.һ��������Ʒ:
                        BagItem[itemId]--;
                        break;
                    case ItemType.���ظ�ʹ������Ʒ:
                        break;
                    case ItemType.������Ʒ:
                        break;
                }

                EventCenter.GetInstance().EventTrigger<int>("����ʹ������", itemId);
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
        {
            currentHp = 0;
            EventCenter.GetInstance().EventTrigger("�������");
        }
        else
        {
            EventCenter.GetInstance().EventTrigger("�������");
        }

    }
    private void GetBagItem(ItemClick item)
    {
        if (!BagItem.ContainsKey(item.id))
        {
            BagItem.Add(item.id, 1);
            BagItemIDList.Add(item.id);
        }
        else
        {
            BagItem[item.id] += 1;
        }
    }
    /// <summary>
    /// ��<һ��������Ʒ>��<���ظ�ʹ������Ʒ>����״̬���ϵ���Ʒ������
    /// </summary>
    /// <param name="index"></param>
    private void SetBagItem(int index)
    {
        if (BagItemIDList.Count == 0)
        {
            GetControl<Image>("Item").sprite = null;
            GetControl<Image>("Item").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
            GetControl<Text>("ItemNum").text = "";
            return;
        }

        ResMgr.GetInstance().LoadAsync<Sprite>(Datas.GetInstance().ItemDataDic[ BagItemIDList[index]].path, (x) =>
        {
            GetControl<Image>("Item").sprite = x;
            GetControl<Image>("Item").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            GetControl<Text>("ItemNum").text = BagItem[BagItemIDList[index]].ToString();
        });
    }
}
