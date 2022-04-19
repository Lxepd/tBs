using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : UIBase
{
    // UI下的Tips文本
    private Text tipsText;
    // 注册计时器
    private Timer showTimer;

    private void Start()
    {
        // 找到UI下的Tips文本
        tipsText = GetControl<Text>("TipsText");
        // 设置计时器
        showTimer = new Timer(Datas.GetInstance().ShowTipsTime, false, false);
        // 注册Tips消息
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
    // 显示UI
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
