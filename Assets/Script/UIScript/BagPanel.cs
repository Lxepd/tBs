using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagPanel : UIBase
{
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
        Debug.Log("�رձ�������");
        UIMgr.GetInstance().HidePanel("BagPanel");
    }
}
