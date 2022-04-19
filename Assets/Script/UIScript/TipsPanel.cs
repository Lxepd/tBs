using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : UIBase
{
    // UI�µ�Tips�ı�
    private Text tipsText;
    // ע���ʱ��
    private Timer showTimer;

    private void Start()
    {
        // �ҵ�UI�µ�Tips�ı�
        tipsText = GetControl<Text>("TipsText");
        // ���ü�ʱ��
        showTimer = new Timer(Datas.GetInstance().ShowTipsTime, false, false);
        // ע��Tips��Ϣ
        EventCenter.GetInstance().AddEventListener<string>("Tips", (x) =>
        {
            tipsText.text = x;
        });
    }
    private void FixedUpdate()
    {
        if(showTimer.isTimeUp)
        {
            gameObject.SetActive(false);
        }
    }
    // ��ʾUI
    public override void ShowMe()
    {
        if (showTimer != null)
            showTimer.Start();
    }
    public override void HideMe()
    {
        tipsText.text = "";
    }
}
