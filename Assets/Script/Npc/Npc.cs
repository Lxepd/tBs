using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum NpcType
{
    None,
    ��������,
    װ������,
    ����
}
[Serializable]
public class ��������
{
    // ����ID
    public int id;
    // ��������
    public int num;
}
[Serializable]
public class װ������
{
    public int test;
}
[Serializable]
public class ����
{
    public int test;
}
public class Npc : MonoBehaviour
{
    // NpcData
    public NpcData data;
    // Npc����
    public NpcType type;
    public List<��������> itemNpc = new List<��������>();
    public List<װ������> equipmentNpc = new List<װ������>();
    public List<����> craftsMan = new List<����>();

    private float checkRadius = 2f;
    // ��ʼ��Npc����
    public void InitNpcData(int id)
    {
        data = GameMgr.GetInstance().GetNpcInfo(id);
    }
    // ��ʼ���̵�
    public void InitShop()
    {
        switch (type)
        {
            case NpcType.��������:
                InitNpcData(13001);
                ItemNpc();
                break;
            case NpcType.װ������:
                InitNpcData(13002);
                break;
            case NpcType.����:
                InitNpcData(13003);
                break;
        }

    }
    #region <<<   ����Npc   >>>
    private void ItemNpc()
    {
        UIMgr.GetInstance().ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Above, (y) =>
        {
            Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "�̵�չʾ����");
            foreach (�������� item in itemNpc)
            {
                int[] num = new int[]
                {
                        // �ж��ٸ�����
                        item.num / GameMgr.GetInstance().GetItemInfo(item.id).maxNum,
                        // ���������
                        item.num % GameMgr.GetInstance().GetItemInfo(item.id).maxNum
                };

                if (num[0] != 0)
                {
                    for (int i = 0; i < num[0]; i++)
                    {
                        CreateShopItem(content, item, GameMgr.GetInstance().GetItemInfo(item.id).maxNum);
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
            x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(GameMgr.GetInstance().GetItemInfo(item.id).path);
                //x.transform.Find("ItemNum").GetComponent<Text>().text = num.ToString();

            });
    }
    #endregion
    // TODO
    #region <<<   װ��Npc   >>>

    #endregion
    // TODO
    #region <<<   ����Npc   >>>

    #endregion
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
#endif
}
