using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo
{

}
public class EventInfo<T> : IEventInfo
{
    public UnityAction<T> actions;
    public EventInfo(UnityAction<T> action)
    {
        action += action;
    }
}
public class EventInfo : IEventInfo
{
    public UnityAction actions;
    public EventInfo(UnityAction action)
    {
        action += action;
    }
}
/// <summary>
/// 事件中心  单例模式对象
/// 1. Dictionary
/// 2. 委托
/// 3. 观察者模式
/// </summary>
public class EventCenter:InstanceNoMono<EventCenter>
{
    /// <summary>
    /// 事件字典
    /// key -----> 事件名字（如：怪物死亡，玩家死亡等）
    /// value -----> 监听的这个事件，对应的委托函数
    /// </summary>
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();
    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="name">事件名字</param>
    /// <param name="action">需要处理的事件委托函数</param>
    public void AddEventListener<T>(string name,UnityAction<T> action)
    {
        // 判断有无对应事件
        if(eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }
    public void AddEventListener(string name, UnityAction action)
    {
        // 判断有无对应事件
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions += action;
        }
        else
        {
            eventDic.Add(name, new EventInfo(action));
        }
    }
    /// <summary>
    /// 移除对应的事件监听
    /// </summary>
    /// <param name="name">事件名字</param>
    /// <param name="action">在字典内的委托函数</param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action)
    {
        if(eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }
    public void RemoveEventListener(string name, UnityAction action)
    {
        if (eventDic.ContainsKey(name))
        {
            (eventDic[name] as EventInfo).actions -= action;
        }
    }
    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="name">哪一个名字事件触发</param>
    public void EventTrigger<T>(string name,T info)
    {
        // 判断有无对应事件
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo<T>).actions != null)
                (eventDic[name] as EventInfo<T>).actions.Invoke(info);
        }

    }
    public void EventTrigger(string name)
    {
        // 判断有无对应事件
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions.Invoke();
        }

    }
    /// <summary>
    /// 清空事件字典
    /// 用于场景切换
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
