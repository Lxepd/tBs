using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : UIBase
{
    Slider slider;
    Text sliderText;

    void Start()
    {
        slider = GetControl<Slider>("Slider");
        sliderText = GetControl<Text>("SliderText"); 
    }

    // Update is called once per frame
    void Update()
    {
        EventCenter.GetInstance().AddEventListener<float>("进度条更新", (x) =>
        {
            slider.value = x;
            sliderText.text = ((int)x).ToString();
        });
    }
}
