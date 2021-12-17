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
        // ҡ�˱���ͼ
        imgBk = GetControl<Image>("ImgBK");
        // ҡ�˿��Ƶ�
        imgControl = GetControl<Image>("ImgCtrl");
        // ��갴�� ̧�� ��ҷ 3���¼��Ŀؼ�
        // ��Ҫ���ڿ��� ����ҡ�� �Ŀ��Ʒ�Χ
        imgTouchRect = GetControl<Image>("ImgTouchRect");
        // ͨ��UI����������Զ������
        UIMgr.AddCustomEventListener(imgTouchRect, EventTriggerType.PointerDown, PointerDown);
        UIMgr.AddCustomEventListener(imgTouchRect, EventTriggerType.PointerUp, PointerUp);
        UIMgr.AddCustomEventListener(imgTouchRect, EventTriggerType.Drag, Drag);

        imgBk.gameObject.SetActive(false);
    }
    /// <summary>
    /// ����
    /// </summary>
    /// <param name="data">��Ļ���λ��</param>
    private void PointerDown(BaseEventData data)
    {
        Debug.Log("����");

        // ��ʾ
        imgBk.gameObject.SetActive(true);
        // �����Ļλ����ʾ
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgTouchRect.rectTransform, // �ı�λ�õĶ���ĸ�����
            (data as PointerEventData).position, // �õ���ǰ�����Ļ������λ��
            (data as PointerEventData).pressEventCamera, // UI�����
            out localPos); // ת��֮�󷵻ص�����

        imgBk.transform.localPosition = localPos;
    }
    /// <summary>
    /// ̧��
    /// </summary>
    /// <param name="data">��Ļ���λ��</param>
    private void PointerUp(BaseEventData data)
    {
        Debug.Log("̧��");
        imgControl.transform.localPosition = Vector2.zero;
        // ̧������
        imgBk.gameObject.SetActive(false);

        // ��Ϣ���ķַ�ҡ�˷���
        EventCenter.GetInstance().EventTrigger<Vector2>("Joystick", Vector2.zero);
        //cEventCenter.AddListener<Vector2>(EventType.Joystick, Vector2.zero);
    }
    /// <summary>
    /// ��ק
    /// </summary>
    /// <param name="data">��Ļ���λ��</param>
    private void Drag(BaseEventData data)
    {
        Debug.Log("��ק");

        Vector2 localPos;
        // ��Ļת��UI����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            imgBk.rectTransform, // �ı�λ�õĶ���ĸ�����
            (data as PointerEventData).position, // �õ���ǰ�����Ļ������λ��
            (data as PointerEventData).pressEventCamera, // UI�����
            out localPos); // ת��֮�󷵻ص�����

        //// ��Χ�ж�
        //float r;
        //r = Mathf.Clamp(localPos.magnitude, 0, maxControlLength);
        ////localPos = r * localPos.normalized;
        //// ���¿��Ƶ�λ��
        //imgControl.transform.position = localPos.normalized;

        // ���¿��Ƶ�λ��
        imgControl.transform.localPosition = localPos;
        // ��Χ�ж�
        if (localPos.magnitude > maxControlLength)
        {
            imgControl.transform.localPosition = localPos.normalized * maxControlLength;
        }

        // ��Ϣ���ķַ�ҡ�˷���
        EventCenter.GetInstance().EventTrigger<Vector2>("Joystick", localPos.normalized);
    }
}
