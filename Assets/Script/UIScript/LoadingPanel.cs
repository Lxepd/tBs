using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : UIBase
{
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<float>("进度条更新", (x) =>
        {
            GetControl<Slider>("Slider").value = x;
            GetControl<Text>("SliderText").text = (x * 100).ToString() + "%";
        });
    }
}
