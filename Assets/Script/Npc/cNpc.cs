//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class Npc : MonoBehaviour
//{
//    // NpcID
//    public int id;
//    // Npc����
//    public NpcType type;
//    // ���۵ĵ��߱�
//    public List<NpcSell> itemSellList = new List<NpcSell>();

//    private float checkRadius = 2f;

//    private void Start()
//    {
//        // data = GameMgr.GetInstance().GetNpcInfo(id);
//    }
//    // ��ʼ���̵�
//    public void InitShop()
//    {
//        switch (type)
//        {
//            case NpcType.��������:
//                ItemNpc();
//                break;
//            case NpcType.װ������:
//                break;
//            case NpcType.����:
//                break;
//        }
        
//    }
//    private void ItemNpc()
//    {
//        UIMgr.GetInstance().ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Above, (y) =>
//        {
//            Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "�̵�չʾ����");
//            foreach (NpcSell item in itemSellList)
//            {
//                int[] num = new int[]
//                {
//                    // �ж��ٸ�����
//                    item.num / GameMgr.GetInstance().GetItemInfo(item.id).maxNum,
//                    // ���������
//                    item.num % GameMgr.GetInstance().GetItemInfo(item.id).maxNum
//                };

//                if (num[0] != 0)
//                {
//                    for (int i = 0; i < num[0]; i++)
//                    {
//                        CreateShopItem(content, item, GameMgr.GetInstance().GetItemInfo(item.id).maxNum);
//                    }
//                }
//                if (num[1] != 0)
//                {
//                    CreateShopItem(content, item, num[1]);
//                }
//            }

//        });
//    }
//    // �����̵���Ʒ
//    private void CreateShopItem(Transform parent, NpcSell item,int num)
//    {
//        PoolMgr.GetInstance().GetObj("Prefabs/ShopItem", (x) =>
//        {
//            x.transform.SetParent(parent);
//            x.transform.localScale = Vector3.one;

//            ItemClick ic = x.GetComponent<ItemClick>();
//            ic.id = item.id;
//            ic.currentNum = num;
//            x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(GameMgr.GetInstance().GetItemInfo(item.id).path);
//            //x.transform.Find("ItemNum").GetComponent<Text>().text = num.ToString();

//        });
//    }
//#if UNITY_EDITOR
//    private void OnDrawGizmosSelected()
//    {
//        Gizmos.color = Color.black;
//        Gizmos.DrawWireSphere(transform.position, checkRadius);
//    }
//#endif
//}
