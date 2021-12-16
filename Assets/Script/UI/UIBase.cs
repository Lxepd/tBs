using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// UI基类
/// </summary>
public class UIBase : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    // Start is called before the first frame update
    void Start()
    {
        FindChildrenControl<Button>();
        FindChildrenControl<Image>();
        FindChildrenControl<Text>();
        FindChildrenControl<Toggle>();
        FindChildrenControl<Slider>();
        FindChildrenControl<ScrollRect>();
    }
    /// <summary>
    /// 显示UI
    /// </summary>
    public virtual void ShowMe()
    {
        
    }
    /// <summary>
    /// 隐藏UI
    /// </summary>
    public virtual void HideMe()
    {

    }
    /// <summary>
    /// 得到对应名字的对应控件脚本
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T: UIBehaviour
    {
        if(controlDic.ContainsKey(controlName))
        {
            foreach (var item in controlDic[controlName])
            {
                if (item is T)
                    return item as T;
            }
        }

        return null;
    }
    /// <summary>
    /// 找到子对象对应控件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChildrenControl<T>() where T : UIBehaviour
    {
        T[] btns = GetComponentsInChildren<T>();
        string objName;

        foreach (var item in btns)
        {
            objName = item.gameObject.name;

            if (controlDic.ContainsKey(item.gameObject.name))
                controlDic[objName].Add(item);
            else
                controlDic.Add(objName, new List<UIBehaviour>() { item });
        }

    }
}
