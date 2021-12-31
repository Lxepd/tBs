using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 列表界面
/// </summary>
public class ListPanel : UIBase
{
    public override void ShowMe()
    {
        // 暂停游戏
        Time.timeScale = 0f;
    }
    public override void HideMe()
    {
        // 恢复游戏
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
                // 切回开头场景
                SceneMgr.GetInstance().LoadSceneAsyn("Main", ()=>
                {
                    // 除了<主界面>以外的界面全部隐藏
                    UIMgr.GetInstance().HideAllPanelBesides("MainPanel");
                    // 清空缓存池
                    PoolMgr.GetInstance().Clear();
                    // 触发消息，初始化<选择角色>界面的选择id
                    EventCenter.GetInstance().EventTrigger<int>("选择角色", 0);
                    // 触发消息，执行<玩家角色>销毁
                    EventCenter.GetInstance().EventTrigger<GameObject>("玩家角色",Player.instance.gameObject);
                });
                break;
            // 关闭菜单
            case "Bto_ListQuit":
                // 隐藏<列表菜单>界面
                UIMgr.GetInstance().HidePanel("ListPanel");
                break;
        }
    }
}
