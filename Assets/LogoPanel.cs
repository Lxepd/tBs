using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoPanel : UIBase
{
    // ע���ʱ��
    private Timer showTimer;

    private void Start()
    {
        // ���ü�ʱ��
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
        // չ��<�������ҡ��>����
        UIMgr.GetInstance().ShowPanel<JoyStickPanel>("JoyStickPanel", E_UI_Layer.Normal);
        // չ��<�Ҳ����>����
        UIMgr.GetInstance().ShowPanel<ControlPanel>("ControlPanel", E_UI_Layer.Normal);
        // չ��<��ɫ״̬>����
        UIMgr.GetInstance().ShowPanel<StatePanel>("StatePanel", E_UI_Layer.Above);
        // չ��<װ������>����
        UIMgr.GetInstance().ShowPanel<UpgradePanel>("UpgradePanel", E_UI_Layer.Above);
        // չ��<����>����
        UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
        // չ��<����>����
        UIMgr.GetInstance().ShowPanel<EndPanel>("EndPanel", E_UI_Layer.Above);
        // չ��<Tips>����
        UIMgr.GetInstance().ShowPanel<TipsPanel>("TipsPanel", E_UI_Layer.Above);
        // չ��<�б�>����
        UIMgr.GetInstance().ShowPanel<ListPanel>("ListPanel", E_UI_Layer.Above);
        // չ��<����>����
        UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Load);
      
    }
    public override void HideMe()
    {
        UIMgr.GetInstance().ShowPanel<MainPanel>("MainPanel", E_UI_Layer.Normal);
    }
}
