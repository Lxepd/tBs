using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : UIBase
{
    GameObject playerGo;
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<GameObject>("玩家物体", (x) =>{ playerGo = x; });
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "ReturnBto":
                SceneMgr.GetInstance().LoadSceneAsyn("Game", () =>
                {
                    ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/RoomPrefabs/准备房", (z) =>
                    {
                        playerGo.transform.position = GameObject.Find("返回点").transform.position;
                    });

                    PoolMgr.GetInstance().Clear();
                    UIMgr.GetInstance().HidePanel("EndPanel");
                    EventCenter.GetInstance().EventTrigger("角色恢复");
                });
                break;
            case "ToMainBto":
                // 切回开头场景
                // 触发消息，执行<玩家角色>销毁
                EventCenter.GetInstance().EventTrigger("销毁角色");
                EventCenter.GetInstance().EventTrigger<int>("道具栏清空", 0);
                EventCenter.GetInstance().EventTrigger<bool>("背包清空", true);
                SceneMgr.GetInstance().LoadSceneAsyn("Main", () =>
                {
                    // 除了<主界面>以外的界面全部隐藏
                    UIMgr.GetInstance().HideAllPanelBesides("MainPanel");
                    // 清空缓存池
                    PoolMgr.GetInstance().Clear();
                    EventCenter.GetInstance().Clear();

                });
                break;
        }
    }
}
