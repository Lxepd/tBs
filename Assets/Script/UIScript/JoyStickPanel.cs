using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoyStickPanel : UIBase
{
    private Image imgTouchRect;
    private Image imgBk;
    private Image imgControl;

    public float maxControlLength = 0;

    private void Start()
    {
        // 摇杆背景图
        imgBk = GetControl<Image>("ImgBK");
        // 摇杆控制点
        imgControl = GetControl<Image>("ImgCtrl");
        // 鼠标按下 抬起 拖曳 3个事件的控件
        // 主要用于控制 虚拟摇杆 的控制范围
        imgTouchRect = GetControl<Image>("ImgTouchRect");
        // 通过UI管理器添加自定义监听
        UIMgr.AddCustomEventListener(imgTouchRect, EventTriggerType.PointerDown, PointerDown);
        UIMgr.AddCustomEventListener(imgTouchRect, EventTriggerType.PointerUp, PointerUp);
        UIMgr.AddCustomEventListener(imgTouchRect, EventTriggerType.Drag, Drag);

        imgBk.gameObject.SetActive(false);
    }
    /// <summary>
    /// 按下
    /// </summary>
    /// <param name="data">屏幕点击位置</param>
    private void PointerDown(BaseEventData data)
    {
        Debug.Log("按下");

        // 显示
        imgBk.gameObject.SetActive(true);
        // 点击屏幕位置显示
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgTouchRect.rectTransform, // 改变位置的对象的父对象
            (data as PointerEventData).position, // 得到当前点击屏幕的坐标位置
            (data as PointerEventData).pressEventCamera, // UI摄像机
            out localPos); // 转换之后返回的坐标

        imgBk.transform.localPosition = localPos;
    }
    /// <summary>
    /// 抬起
    /// </summary>
    /// <param name="data">屏幕点击位置</param>
    private void PointerUp(BaseEventData data)
    {
        Debug.Log("抬起");
        imgControl.transform.localPosition = Vector2.zero;
        // 抬起隐藏
        imgBk.gameObject.SetActive(false);

        // 消息中心分发摇杆方向
        EventCenter.GetInstance().EventTrigger<Vector2>("Joystick", Vector2.zero);
        //cEventCenter.AddListener<Vector2>(EventType.Joystick, Vector2.zero);
    }
    /// <summary>
    /// 拖拽
    /// </summary>
    /// <param name="data">屏幕点击位置</param>
    private void Drag(BaseEventData data)
    {
        Debug.Log("拖拽");

        Vector2 localPos;
        // 屏幕转换UI坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgBk.rectTransform, // 改变位置的对象的父对象
            (data as PointerEventData).position, // 得到当前点击屏幕的坐标位置
            (data as PointerEventData).pressEventCamera, // UI摄像机
            out localPos); // 转换之后返回的坐标

        //// 范围判断
        //float r;
        //r = Mathf.Clamp(localPos.magnitude, 0, maxControlLength);
        ////localPos = r * localPos.normalized;
        //// 更新控制点位置
        //imgControl.transform.position = localPos.normalized;

        // 更新控制点位置
        imgControl.transform.localPosition = localPos;
        // 范围判断
        if (localPos.magnitude > maxControlLength)
        {
            imgControl.transform.localPosition = localPos.normalized * maxControlLength;
        }

        // 消息中心分发摇杆方向
        EventCenter.GetInstance().EventTrigger<Vector2>("Joystick", localPos.normalized);
    }
}
