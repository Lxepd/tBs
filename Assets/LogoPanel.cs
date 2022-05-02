using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoPanel : UIBase
{
    // 注册计时器
    private Timer showTimer;

    private void Start()
    {
        // 设置计时器
        showTimer = new Timer(Datas.GetInstance().ShowLogoTime, false, true);

    }
    private void FixedUpdate()
    {
        if (showTimer.isTimeUp)
        {
            UIMgr.GetInstance().HideAllPanel();
            MusicMgr.GetInstance().PlayBkMusic("Risk of Rain 2");
            MusicMgr.GetInstance().ChangeBkValue(.1f);
        }
    }
    public override void ShowMe()
    {
        // 展开<左侧虚拟摇杆>界面
        UIMgr.GetInstance().ShowPanel<JoyStickPanel>("JoyStickPanel", E_UI_Layer.Normal);
        // 展开<右侧控制>界面
        UIMgr.GetInstance().ShowPanel<ControlPanel>("ControlPanel", E_UI_Layer.Normal);
        // 展开<角色状态>界面
        UIMgr.GetInstance().ShowPanel<StatePanel>("StatePanel", E_UI_Layer.Above);
        // 展开<装备升级>界面
        UIMgr.GetInstance().ShowPanel<UpgradePanel>("UpgradePanel", E_UI_Layer.Above);
        // 展开<背包>界面
        UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
        // 展开<结束>界面
        UIMgr.GetInstance().ShowPanel<EndPanel>("EndPanel", E_UI_Layer.Above);
        // 展开<Tips>界面
        UIMgr.GetInstance().ShowPanel<TipsPanel>("TipsPanel", E_UI_Layer.Above);
        // 展开<列表>界面
        UIMgr.GetInstance().ShowPanel<ListPanel>("ListPanel", E_UI_Layer.Above);
        // 展开<加载>界面
        UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Load);
      
    }
    public override void HideMe()
    {
        UIMgr.GetInstance().ShowPanel<MainPanel>("MainPanel", E_UI_Layer.Normal);
    }
}
