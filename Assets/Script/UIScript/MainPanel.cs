using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using Cinemachine;

public class MainPanel : UIBase
{
    protected override void OnClick(string btnName)
    {
        switch(btnName)
        {
            case "Bto_Play":
                // 展开<选择角色>界面
                UIMgr.GetInstance().ShowPanel<SelectPanel>("SelectPanel", E_UI_Layer.Normal);
                // 隐藏<主界面>
                UIMgr.GetInstance().HidePanel("MainPanel");
                break;
            case "Bto_Quit":
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }
}
