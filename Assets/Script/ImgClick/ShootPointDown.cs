using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootPointDown : ImgClickBase
{
    private bool isPause = false;

    private void Update()
    {
        EventCenter.GetInstance().EventTrigger("Éä»÷³¤°´", isPause);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        isPause = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        isPause = false;
    }
}
