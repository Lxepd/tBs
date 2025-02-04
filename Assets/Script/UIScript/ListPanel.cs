using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 列表界面
/// </summary>
public class ListPanel : UIBase
{
    GameObject playerGo;
    bool isFirst = true;
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<GameObject>("玩家物体", (x) => { playerGo = x; });
        isFirst = !isFirst;
    }

    public override void ShowMe()
    {
        // 暂停游戏
        if (!isFirst)
        {
            Time.timeScale = 0f;
        }
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
                UIMgr.GetInstance().ShowPanel<QuestionPanel>("QuestionPanel", E_UI_Layer.Above);
                break;
            // 返回主菜单
            case "Bto_Home":
                // 切回开头场景
                // 触发消息，执行<玩家角色>销毁
                EventCenter.GetInstance().EventTrigger("销毁角色");
                EventCenter.GetInstance().EventTrigger<int>("道具栏清空", 0);
                EventCenter.GetInstance().EventTrigger<bool>("背包清空", true);
                Datas.GetInstance().GameReturnClear();
                SceneMgr.GetInstance().LoadSceneAsyn("Main", ()=>
                {
                    // 除了<主界面>以外的界面全部隐藏
                    UIMgr.GetInstance().HideAllPanelBesides("MainPanel");
                    // 清空缓存池
                    PoolMgr.GetInstance().Clear();
                    EventCenter.GetInstance().Clear();
                });
                break;
            // 关闭菜单
            case "Bto_ListQuit":
                // 隐藏<列表菜单>界面
                UIMgr.GetInstance().HidePanel("ListPanel");
                break;
            case "Bto_Save":
                if(LevelMgr.GetInstance().isInLevel)
                {
                    Debug.Log("无法在关卡保存");
                    return;
                }    

                XmlSL.GetInstance().Save();
                break;
            case "Bto_Load":
                //UIMgr.GetInstance().ShowPanel<SLPanel>("SLPanel", E_UI_Layer.Above);
                break;
        }
    }
}
