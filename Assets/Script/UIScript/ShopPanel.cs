using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
