using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : UIBase
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void HideMe()
    {
        Transform itemParent = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "商店展示界面");
        for (int i = itemParent.childCount - 1; i >= 0; i--)
        {
            itemParent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = null;
            PoolMgr.GetInstance().PushObj(itemParent.GetChild(i).name, itemParent.GetChild(i).gameObject);
        }
    }

    protected override void OnClick(string btnName)
    {
        switch(btnName)
        {
            case "Bto_ShopQuit":    
                UIMgr.GetInstance().HidePanel("ShopPanel");
                break;
        }
    }

}
