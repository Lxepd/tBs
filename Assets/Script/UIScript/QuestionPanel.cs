using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanel : UIBase
{
    public List<Sprite> questionSpriteList = new List<Sprite>();
    private int index;

    public override void ShowMe()
    {
        index = 0;
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            // 上一张图片
            case "Bto_Last":
                if (--index >= 0)
                    GetControl<Image>("QuestionPanel").sprite = questionSpriteList[index];
                break;
            // 下一张
            case "Bto_Next":
                if(++index < questionSpriteList.Count)
                    GetControl<Image>("QuestionPanel").sprite = questionSpriteList[index];
                break;
            // 退出
            case "Bto_QQuit":
                UIMgr.GetInstance().HidePanel("QuestionPanel");
                break;
        }
    }
}
