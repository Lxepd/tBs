using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 选择角色界面
/// </summary>
public class SelectPanel : UIBase
{
    // 选择的角色ID
    private int playerID;

    private void Start()
    {
        // 注册角色ID消息
        EventCenter.GetInstance().AddEventListener<int>("选择角色", (x) =>
         {
             Debug.Log(x);
             playerID = x;
         });
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            // <选择角色游玩>按钮
            case "Bto_SelectPlay":
                // 排除没选择情况
                if (playerID == 0)
                    return;
                // 隐藏<选择角色>界面
                UIMgr.GetInstance().HidePanel("SelectPanel");
                // 展开<背包>界面
                UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
                // 展开<加载>界面
                UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Above);
                // 异步场景切换，并执行切换后的委托
                SceneMgr.GetInstance().LoadSceneAsyn("Game", () =>
                {
                    // 根据选择的角色ID进行初始化
                    InitPlayer(playerID);
                    // 隐藏<主界面>
                    UIMgr.GetInstance().HidePanel("MainPanel");
                    // 隐藏<加载>界面
                    UIMgr.GetInstance().HidePanel("LoadingPanel");
                    // 展开<左侧虚拟摇杆>界面
                    UIMgr.GetInstance().ShowPanel<JoyStickPanel>("JoyStickPanel", E_UI_Layer.Normal);
                    // 展开<右侧控制>界面
                    UIMgr.GetInstance().ShowPanel<ControlPanel>("ControlPanel", E_UI_Layer.Normal);
                    // 展开<角色状态>界面
                    UIMgr.GetInstance().ShowPanel<StatePanel>("StatePanel", E_UI_Layer.Normal);
                    // 隐藏<背包>界面
                    UIMgr.GetInstance().HidePanel("BagPanel");
                });
                break;
        }
    }
    /// <summary>
    /// 初始化角色
    /// </summary>
    /// <param name="id">选择的角色ID</param>
    private void InitPlayer(int id)
    {
        // 异步生成角色
        ResMgr.GetInstance().LoadAsync<GameObject>(GameMgr.GetInstance().GetPlayerInfo(playerID).path, (x) =>
         {
             // 初始化位置
             x.transform.position = GameObject.Find("返回点").transform.position;
             // 注册返回主界面的消息
             EventCenter.GetInstance().AddEventListener<GameObject>("玩家角色", (y) => 
             {
                 Destroy(y);
             });
             // 切换场景不销毁
             DontDestroyOnLoad(x);
         });
    }
}
