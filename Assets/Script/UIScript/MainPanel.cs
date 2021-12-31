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
                // չ��<ѡ���ɫ>����
                UIMgr.GetInstance().ShowPanel<SelectPanel>("SelectPanel", E_UI_Layer.Normal);
                // ����<������>
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
