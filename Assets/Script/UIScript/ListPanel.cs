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
            // 音量设置
            case "Bto_SetMusic":
                break;
            // 疑问按钮
            case "Bto_Q":
                // 打开疑问UI
                break;
            // 返回主菜单
            case "Bto_Home":
                SceneMgr.GetInstance().LoadSceneAsyn("Main", ()=>
                {
                    UIMgr.GetInstance().HideAllPanelBesides("MainPanel");
                    //PoolMgr.GetInstance().PushObj("角色/"+GameMgr.GetInstance().GetPlayerInfo(Player.instance.id).name, Player.instance.gameObject);
                });
                break;
            // 关闭菜单
            case "Bto_ListQuit":
                UIMgr.GetInstance().HidePanel("ListPanel");
                break;
        }
    }
}
