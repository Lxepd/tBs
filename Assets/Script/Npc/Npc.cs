using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    // NpcData
    public NpcData data;
    // Npc����
    public NpcType type;

    [Header("�����̵������Ķ���")]
    public List<��������> items = new List<��������>();
    public List<��������> itemsCopy = new List<��������>();

    public List<װ������> equipmentNpc = new List<װ������>();
    public List<����> craftsMan = new List<����>();

    private Timer reInit;
    [Header("�̵�ˢ��ʱ�䣬���30s")]
    public float reInitTime;
    [Header("�����Ҿ��룬���1.5")]
    public float checkPlayerHereRadius = 1.5f;

    private bool playerHere;

    private void Start()
    {
        switch (type)
        {
            case NpcType.��������:
                data = GameTool.GetDicInfo(Datas.GetInstance().NpcDataDic, 13001);
                itemsCopy = GameTool.Clone<��������>(items);
                EventCenter.GetInstance().AddEventListener<int>("NPC������������", (x) =>
                {
                    if (!playerHere)
                        return;

                    ShopItemNumReduce(x);
                });
                reInit = new Timer(Mathf.Max(30, reInitTime), true, true);
                break;
            case NpcType.װ������:
                data = GameTool.GetDicInfo(Datas.GetInstance().NpcDataDic, 13002);
                //TODO
                break;
            case NpcType.����:
                data = GameTool.GetDicInfo(Datas.GetInstance().NpcDataDic, 13003);
                //TODO
                break;
        }

    }
    private void FixedUpdate()
    {
        if (playerHere = Physics2D.OverlapCircle(transform.position, checkPlayerHereRadius, LayerMask.GetMask("���")))
        {
            EventCenter.GetInstance().EventTrigger<float>("ˢ��ʱ��", reInit.nowTime);
        }
        
        if (reInit.isTimeUp)
        {
            switch (type)
            {
                case NpcType.��������:
                    itemsCopy.Clear();
                    itemsCopy = GameTool.Clone<��������>(items);
                    break;
                case NpcType.װ������:
                    break;
                case NpcType.����:
                    break;
            }
        }
    }
    // ��ʼ���̵�
    public void InitShop()
    {
        switch (type)
        {
            case NpcType.��������:
                ItemNpc();
                break;
            case NpcType.װ������:
                break;
            case NpcType.����:
                break;
        }
    }
    #region <<<   ����Npc   >>>
    private void ItemNpc()
    {
        UIMgr.GetInstance().ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Above, (y) =>
        {
            Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "�̵�չʾ����");
            foreach (�������� item in itemsCopy)
            {
                int[] num = new int[]
                {
                        // �ж��ٸ�����
                        item.num / GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic,item.id).maxNum,
                        // ���������
                        item.num % GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic,item.id).maxNum
                };

                if (num[0] != 0)
                {
                    for (int i = 0; i < num[0]; i++)
                    {
                        CreateShopItem(content, item, GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, item.id).maxNum);
                    }
                }
                if (num[1] != 0)
                {
                    CreateShopItem(content, item, num[1]);
                }
            }

        });
    }
    /// <summary>
    /// �����̵���Ʒ
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="item">����</param>
    /// <param name="num">����</param>
    private void CreateShopItem(Transform parent, �������� item, int num)
    {
        PoolMgr.GetInstance().GetObj("Prefabs/ShopItem", (x) =>
        {
            x.transform.SetParent(parent);
            x.transform.localScale = Vector3.one;

            ItemClick ic = x.GetComponent<ItemClick>();
            ic.id = item.id;
            ic.currentNum = num;
            x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, item.id).path);
            //x.transform.Find("ItemNum").GetComponent<Text>().text = num.ToString();

        });
    }
    private void ShopItemNumReduce(int _id)
    {
        foreach (var item in itemsCopy)
        {
            if (item.id == _id)
            {
                item.num--;
                return;
            }
        }
    }
    #endregion
    // TODO
    #region <<<   װ��Npc   >>>

    #endregion
    // TODO
    #region <<<   ����Npc   >>>

    #endregion
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, checkPlayerHereRadius);
    }
}
