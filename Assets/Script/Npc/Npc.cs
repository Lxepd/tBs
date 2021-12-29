using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Npc : MonoBehaviour
{
    public int id;
    private NpcData data;
    // 出售的道具表
    public List<NpcSell> itemSellList = new List<NpcSell>();

    private float checkRadius = 2f;
    private bool playerOpenShop;

    private void Start()
    {
        data = GameMgr.GetInstance().GetNpcInfo(id);
  
    }
    private void FixedUpdate()
    {
        
    }
    public void OpenShop()
    {
        UIMgr.GetInstance().ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Above, (y) =>
        {
            Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject , "商店展示界面");
            Debug.Log(itemSellList.Count);
            foreach (NpcSell item in itemSellList)
            {
                int[] num = new int[]
                {
                    // 有多少个满的
                    item.num / GameMgr.GetInstance().GetItemInfo(item.id).maxNum,
                    // 多出来几个
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
    private void CreateShopItem(Transform parent, NpcSell item,int num)
    {
        PoolMgr.GetInstance().GetObj("Prefabs/ShopItem", (x) =>
        {
            x.transform.SetParent(parent);
            x.transform.localScale = Vector3.one;
            x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(GameMgr.GetInstance().GetItemInfo(item.id).path);
            x.transform.Find("ItemNum").GetComponent<Text>().text = num.ToString();

        });
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
