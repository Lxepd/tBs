//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class Npc : MonoBehaviour
//{
//    // NpcID
//    public int id;
//    // Npc类型
//    public NpcType type;
//    // 出售的道具表
//    public List<NpcSell> itemSellList = new List<NpcSell>();

//    private float checkRadius = 2f;

//    private void Start()
//    {
//        // data = GameMgr.GetInstance().GetNpcInfo(id);
//    }
//    // 初始化商店
//    public void InitShop()
//    {
//        switch (type)
//        {
//            case NpcType.道具商人:
//                ItemNpc();
//                break;
//            case NpcType.装备商人:
//                break;
//            case NpcType.工匠:
//                break;
//        }
        
//    }
//    private void ItemNpc()
//    {
//        UIMgr.GetInstance().ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Above, (y) =>
//        {
//            Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "商店展示界面");
//            foreach (NpcSell item in itemSellList)
//            {
//                int[] num = new int[]
//                {
//                    // 有多少个满的
//                    item.num / GameMgr.GetInstance().GetItemInfo(item.id).maxNum,
//                    // 多出来几个
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
//    // 生成商店物品
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
