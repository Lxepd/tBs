using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;

public class MainPanel : UIBase
{
    protected override void OnClick(string btnName)
    {
        switch(btnName)
        {
            case "Bto_Play":
                UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Above);
                SceneMgr.GetInstance().LoadSceneAsyn("Game", () => 
                {
                    UIMgr.GetInstance().HidePanel("MainPanel");
                    UIMgr.GetInstance().HidePanel("LoadingPanel");
                    UIMgr.GetInstance().ShowPanel<JoyStickPanel>("JoyStickPanel", E_UI_Layer.Normal);
                    UIMgr.GetInstance().ShowPanel<ControlPanel>("ControlPanel", E_UI_Layer.Normal);
                });
                break;
        }
    }

}
