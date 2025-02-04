using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum E_UI_Layer
{
    Normal,
    Above,
    Load,
    Logo
}

/// <summary>
/// UI管理器
/// 1. 管理所有显示的面板
/// 2. 提供给外部 显示和隐藏等接口
/// </summary>
public class UIMgr : InstanceNoMono<UIMgr>
{
    private Dictionary<string, UIBase> panelDic = new Dictionary<string, UIBase>();

    private Transform normal;
    private Transform above;
    private Transform load;
    private Transform logo;

    public RectTransform canvas;

    public UIMgr()
    {
        GameObject obj = ResMgr.GetInstance().Load<GameObject>("Prefabs/UI/Canvas");
        canvas = obj.transform as RectTransform;
        Object.DontDestroyOnLoad(obj);

        normal = canvas.Find("Normal");
        above = canvas.Find("Above");
        load = canvas.Find("Load");
        logo = canvas.Find("Logo");

        obj = ResMgr.GetInstance().Load<GameObject>("Prefabs/UI/EventSystem");
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
            case E_UI_Layer.Load:
                return this.load;
            case E_UI_Layer.Logo:
                return this.logo;
        }

        return null;
    }
    /// <summary>
    /// 显示UI
    /// </summary>
    /// <typeparam name="T">UI脚本类名</typeparam>
    /// <param name="panelName">UI名</param>
    /// <param name="layer">哪一层</param>
    /// <param name="callBack">委托</param>
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

        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/UI/" + panelName,(obj)=> 
        {

            Transform father = normal;
            switch (layer)
            {
                case E_UI_Layer.Above:
                    father = above;
                    break;
                case E_UI_Layer.Load:
                    father = load;
                    break;
                case E_UI_Layer.Logo:
                    father = logo;
                    break;
            }

            obj.transform.SetParent(father);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            // 获取脚本
            T panel = obj.GetComponent<T>();

            if (callBack != null)
                callBack(panel);

            panelDic.Add(panelName, panel);

            panel.ShowMe();
        });
    }
    /// <summary>
    /// 隐藏UI
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
    public void HideAllPanel()
    {
        foreach (var panel in panelDic)
        {
            HidePanel(panel.Key);
        }
    }
    public void HideAllPanelBesides(string name)
    {
        List<string> aList = new List<string>();

        foreach (var panel in panelDic)
        {
            if (panel.Key == name)
                continue;

            Object.Destroy(panel.Value.gameObject);
            aList.Add(panel.Key);
            //HidePanel(panel.Key);
        }

        for (int i = 0; i < aList.Count; i++)
        {
            if (aList[i] == name)
                continue;

            panelDic.Remove(aList[i]);
        }
    }

    /// <summary>
    /// 获取已经显示的UI
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
    /// 控件添加自定义事件监听
    /// </summary>
    /// <param name="control">控件对象</param>
    /// <param name="type">事件类型</param>
    /// <param name="callBack">事件委托</param>
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
