using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_UI_Layer
{
    Normal,
    Above
}

/// <summary>
/// UI������
/// 1. ����������ʾ�����
/// 2. �ṩ���ⲿ ��ʾ�����صȽӿ�
/// </summary>
public class UIMgr : InstanceNoMono<UIMgr>
{
    private Dictionary<string, UIBase> panelDic = new Dictionary<string, UIBase>();

    private Transform normal;
    private Transform above;

    public RectTransform canvas;

    public UIMgr()
    {
        GameObject obj = ResMgr.GetInstance().Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        Object.DontDestroyOnLoad(obj);

        normal = canvas.Find("Normal");
        above = canvas.Find("Above");

        obj = ResMgr.GetInstance().Load<GameObject>("UI/EventSystem");
        Object.DontDestroyOnLoad(obj);
    }
    public Transform GetLayerFather(E_UI_Layer layer)
    {
        switch (layer)
        {
            case E_UI_Layer.Normal:
                return this.normal;
            case E_UI_Layer.Above:
                return this.above;
        }

        return null;
    }
    /// <summary>
    /// ��ʾUI
    /// </summary>
    /// <typeparam name="T">UI�ű�����</typeparam>
    /// <param name="panelName">UI��</param>
    /// <param name="layer">��һ��</param>
    /// <param name="callBack">ί��</param>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer=E_UI_Layer.Normal,UnityAction<T> callBack=null) where T:UIBase
    {
        if(panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].ShowMe();
            panelDic[panelName].gameObject.SetActive(true);

            if (callBack != null)
                callBack(panelDic[panelName] as T);

            return;
        }

        ResMgr.GetInstance().LoadAsync<GameObject>("UI/" + panelName,(obj)=> 
        {

            Transform father = normal;
            switch (layer)
            {
                case E_UI_Layer.Above:
                    father = above;
                    break;
            }

            obj.transform.SetParent(father);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            // ��ȡ�ű�
            T panel = obj.GetComponent<T>();

            if (callBack != null)
                callBack(panel);

            panelDic.Add(panelName, panel);

            panel.ShowMe();
        });
    }
    /// <summary>
    /// ����UI
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if(panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].HideMe();
            panelDic[panelName].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// ��ȡ�Ѿ���ʾ��UI
    /// </summary>
    public T GetPanel<T>(string name) where T:UIBase
    {
        if(panelDic.ContainsKey(name))
        {
            return panelDic[name] as T;
        }
        return null;
    }
    /// <summary>
    /// �ؼ�����Զ����¼�����
    /// </summary>
    /// <param name="control">�ؼ�����</param>
    /// <param name="type">�¼�����</param>
    /// <param name="callBack">�¼�ί��</param>
    public static void AddCustomEventListener(UIBehaviour control,EventTriggerType type, UnityAction<BaseEventData> callBack)
    {
        EventTrigger trigger = control.GetComponent<EventTrigger>();

        if(trigger==null)
        {
            trigger = control.gameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callBack);

        trigger.triggers.Add(entry);
    }
}
