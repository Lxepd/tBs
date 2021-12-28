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

            foreach (var item in itemSellList)
            {
                PoolMgr.GetInstance().GetObj("Prefabs/ShopItem", (x) =>
                {
                    x.transform.SetParent(content);
                    x.transform.localScale = Vector3.one;
                    x.transform.Find("Img").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(GameMgr.GetInstance().GetItemInfo(item.id).path);
                });
            }

        });
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
