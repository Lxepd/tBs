using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : UIBase
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
        switch (btnName)
        {
            case "CloseBagBto":
                HideBagPanel();
                break;
        }
    }
    private void HideBagPanel()
    {
        Debug.Log("¹Ø±Õ±³°ü£¡£¡");
        UIMgr.GetInstance().HidePanel("BagPanel");
    }
}
