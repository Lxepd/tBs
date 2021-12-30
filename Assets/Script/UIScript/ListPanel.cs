using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListPanel : UIBase
{
    public override void ShowMe()
    {
        Time.timeScale = 0f;
    }
    public override void HideMe()
    {
        Time.timeScale = 1f;
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            // ��������
            case "Bto_SetMusic":
                break;
            // ���ʰ�ť
            case "Bto_Q":
                // ������UI
                break;
            // �������˵�
            case "Bto_Home":
                SceneMgr.GetInstance().LoadSceneAsyn("Main", ()=>
                {
                    UIMgr.GetInstance().HideAllPanelBesides("MainPanel");
                    //PoolMgr.GetInstance().PushObj("��ɫ/"+GameMgr.GetInstance().GetPlayerInfo(Player.instance.id).name, Player.instance.gameObject);
                });
                break;
            // �رղ˵�
            case "Bto_ListQuit":
                UIMgr.GetInstance().HidePanel("ListPanel");
                break;
        }
    }
}
