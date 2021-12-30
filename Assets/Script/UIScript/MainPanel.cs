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
                UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
                UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Above);
                SceneMgr.GetInstance().LoadSceneAsyn("Game", () => 
                {
                    //InitPlayer();

                    UIMgr.GetInstance().HidePanel("MainPanel");
                    UIMgr.GetInstance().HidePanel("LoadingPanel");
                    UIMgr.GetInstance().ShowPanel<JoyStickPanel>("JoyStickPanel", E_UI_Layer.Normal);
                    UIMgr.GetInstance().ShowPanel<ControlPanel>("ControlPanel", E_UI_Layer.Normal);
                    UIMgr.GetInstance().ShowPanel<StatePanel>("StatePanel", E_UI_Layer.Normal);
                    UIMgr.GetInstance().HidePanel("BagPanel");
                });
                break;
        }
    }

    private void InitPlayer()
    {
        PoolMgr.GetInstance().GetObj("Prefabs/С����", (x) =>
         {
             x.transform.position = GameObject.Find("���ص�").transform.position;
             DontDestroyOnLoad(x);
         });

        EventCenter.GetInstance().AddEventListener<ItemClick>("�̵���Ʒ", (x) =>
        {
            Debug.Log(x);
        });
    }
}
