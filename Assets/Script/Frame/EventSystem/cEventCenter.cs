using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 事件类型（类名）
/// </summary>
public enum EventType
{
    Joystick
}
public delegate void CallBack();
public delegate void CallBack<T>(T t_arg);
public delegate void CallBack<T,K>(T t_arg,K k_arg);
public delegate void CallBack<T, K,Q>(T t_arg, K k_arg,Q q_arg);
public delegate void CallBack<T, K, Q,Z>(T t_arg, K k_arg, Q q_arg,Z z_arg);
public delegate void CallBack<T, K, Q, Z, W>(T t_arg, K k_arg, Q q_arg, Z z_arg, W w_arg);
/// <summary>
/// 事件中心
/// </summary>
public class cEventCenter:InstanceNoMono<cEventCenter>
{
    /// <summary>
    /// 事件字典
    /// </summary>
    private static Dictionary<EventType, Delegate> m_EventDic = new Dictionary<EventType, Delegate>();

    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    private static void OnListenerAdding(EventType eventType, Delegate callBack)
    {
        // 判断字典是否包含该类型事件
        if (!m_EventDic.ContainsKey(eventType))
        {
            // 无则添加 新类型
            m_EventDic.Add(eventType, null);
        }
        // 获取该类型事件
        Delegate d = m_EventDic[eventType];
        // 判断委托不为空并判断委托的类型不符合回调类型
        if (d != null && d.GetType() != callBack.GetType())
        {
            // 抛出异常
            throw new Exception(string.Format("尝试为事件{0}添加不同类型的委托，当前事件对应委托是{1}，要添加的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
        }
    }
    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    private static void OnListenerRemove(EventType eventType, Delegate callBack)
    {
        // 字典有委托类型
        if (m_EventDic.ContainsKey(eventType))
        {
            // 取出委托
            Delegate d = m_EventDic[eventType];
            // 如果委托为空，则抛出异常
            if (d == null)
            {
                throw new Exception(string.Format("移除监听错误：事件{0}没有对应的委托", eventType));
            }
            // 如果移除的委托类型 与 字典内的类型 不一致，则抛出异常
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误：尝试为事件{0}移除不同类型的委托，当前类型的委托为{1}，要移除的委托类型为{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        // 字典内无该委托类型
        else
        {
            throw new Exception(string.Format("移除监听错误：没有该委托事件码{0}", eventType));
        }
    }
    /// <summary>
    /// 检测移除完之后，字典内委托是否为空
    /// </summary>
    /// <param name="eventType"></param>
    private static void OnListenerRemoved(EventType eventType)
    {
        // 如果字典内为空了，就在字典中移除
        if (m_EventDic[eventType] == null)
        {
            m_EventDic.Remove(eventType);
        }
    }
    
    #region 添加事件
    /// <summary>
    /// 添加事件（无参）
    /// </summary>
    /// <param name="eventType">委托类型</param>
    /// <param name="callBack">委托</param>
    public static void AddListener(EventType eventType, CallBack callBack)
    {
        OnListenerAdding(eventType, callBack);
        // 添加委托的类型 和 回调类型一致
        // 进行委托关联
        m_EventDic[eventType] = (CallBack)m_EventDic[eventType] + callBack;

    }
    /// <summary>
    /// 添加事件（1个参）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerAdding(eventType, callBack);
        // 添加委托的类型 和 回调类型一致
        // 进行委托关联
        m_EventDic[eventType] = (CallBack<T>)m_EventDic[eventType] + callBack;

    }
    /// <summary>
    /// 添加事件（2个参）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T, K>(EventType eventType, CallBack<T, K> callBack)
    {
        OnListenerAdding(eventType, callBack);
        // 添加委托的类型 和 回调类型一致
        // 进行委托关联
        m_EventDic[eventType] = (CallBack<T, K>)m_EventDic[eventType] + callBack;

    }
    /// <summary>
    /// 添加事件（3个参）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T, K, Q>(EventType eventType, CallBack<T, K, Q> callBack)
    {
        OnListenerAdding(eventType, callBack);
        // 添加委托的类型 和 回调类型一致
        // 进行委托关联
        m_EventDic[eventType] = (CallBack<T, K, Q>)m_EventDic[eventType] + callBack;

    }
    /// <summary>
    /// 添加事件（4个参）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T, K, Q, Z>(EventType eventType, CallBack<T, K, Q, Z> callBack)
    {
        OnListenerAdding(eventType, callBack);
        // 添加委托的类型 和 回调类型一致
        // 进行委托关联
        m_EventDic[eventType] = (CallBack<T, K, Q, Z>)m_EventDic[eventType] + callBack;

    }
    /// <summary>
    /// 添加事件（5个参）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <typeparam name="W"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T, K, Q, Z, W>(EventType eventType, CallBack<T, K, Q, Z, W> callBack)
    {
        OnListenerAdding(eventType, callBack);
        // 添加委托的类型 和 回调类型一致
        // 进行委托关联
        m_EventDic[eventType] = (CallBack<T, K, Q, Z, W>)m_EventDic[eventType] + callBack;

    }
    #endregion
   
    #region 移除监听
    /// <summary>
    /// 移除监听（无参）
    /// </summary>
    /// <param name="eventType">委托类型</param>
    /// <param name="callBack">委托</param>
    public static void RemoveListener(EventType eventType, CallBack callBack)
    {
        OnListenerRemove(eventType, callBack);
        // 移除
        m_EventDic[eventType] = (CallBack)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    /// <summary>
    /// 移除监听（1个参）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerRemove(eventType, callBack);

        // 移除
        m_EventDic[eventType] = (CallBack<T>)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    /// <summary>
    /// 移除监听（2个参）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T, K>(EventType eventType, CallBack<T, K> callBack)
    {
        OnListenerRemove(eventType, callBack);

        // 移除
        m_EventDic[eventType] = (CallBack<T, K>)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    /// <summary>
    /// 移除监听（3个参）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T, K, Q>(EventType eventType, CallBack<T, K, Q> callBack)
    {
        OnListenerRemove(eventType, callBack);

        // 移除
        m_EventDic[eventType] = (CallBack<T, K, Q>)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    /// <summary>
    /// 移除监听（4个参）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T, K, Q, Z>(EventType eventType, CallBack<T, K, Q, Z> callBack)
    {
        OnListenerRemove(eventType, callBack);

        // 移除
        m_EventDic[eventType] = (CallBack<T, K, Q, Z>)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    /// <summary>
    /// 移除监听（5个参）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <typeparam name="W"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T, K, Q, Z, W>(EventType eventType, CallBack<T, K, Q, Z, W> callBack)
    {
        OnListenerRemove(eventType, callBack);

        // 移除
        m_EventDic[eventType] = (CallBack<T, K, Q, Z, W>)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }

    #endregion
    
    #region 广播
    /// <summary>
    /// 无参广播
    /// </summary>
    /// <param name="eventType">委托类型</param>
    public static void Broadcast(EventType eventType)
    {
        Delegate d;
        // 获取回调
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack callBack = (CallBack)d;
            // 如果委托不为空
            if (callBack != null)
            {
                // 进行广播
                callBack();
            }
            // 否则抛出异常
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
            }
        }
    }
    /// <summary>
    /// 带1个参的广播
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="arg"></param>
    public static void Broadcast<T>(EventType eventType, T t_arg)
    {
        Delegate d;
        // 获取回调
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack<T> callBack = (CallBack<T>)d;
            // 如果委托不为空
            if (callBack != null)
            {
                // 进行广播
                callBack(t_arg);
            }
            // 否则抛出异常
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
            }
        }
    }
    /// <summary>
    /// 带2个参的广播
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="t_arg"></param>
    /// <param name="k_arg"></param>
    public static void Broadcast<T,K>(EventType eventType, T t_arg,K k_arg)
    {
        Delegate d;
        // 获取回调
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack<T, K> callBack = (CallBack<T, K>)d;
            // 如果委托不为空
            if (callBack != null)
            {
                // 进行广播
                callBack(t_arg,k_arg);
            }
            // 否则抛出异常
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
            }
        }
    }
    /// <summary>
    /// 带3个参的广播
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="t_arg"></param>
    /// <param name="k_arg"></param>
    /// <param name="q_arg"></param>
    public static void Broadcast<T, K,Q>(EventType eventType, T t_arg, K k_arg,Q q_arg)
    {
        Delegate d;
        // 获取回调
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack<T, K, Q> callBack = (CallBack<T, K, Q>)d;
            // 如果委托不为空
            if (callBack != null)
            {
                // 进行广播
                callBack(t_arg, k_arg, q_arg);
            }
            // 否则抛出异常
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
            }
        }
    }
    /// <summary>
    /// 带4个参的广播
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="t_arg"></param>
    /// <param name="k_arg"></param>
    /// <param name="q_arg"></param>
    /// <param name="z_arg"></param>
    public static void Broadcast<T, K, Q,Z>(EventType eventType, T t_arg, K k_arg, Q q_arg,Z z_arg)
    {
        Delegate d;
        // 获取回调
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack<T, K, Q, Z> callBack = (CallBack<T, K, Q, Z>)d;
            // 如果委托不为空
            if (callBack != null)
            {
                // 进行广播
                callBack(t_arg, k_arg, q_arg, z_arg);
            }
            // 否则抛出异常
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
            }
        }
    }
    /// <summary>
    /// 带5个参的广播
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <typeparam name="Z"></typeparam>
    /// <typeparam name="W"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="t_arg"></param>
    /// <param name="k_arg"></param>
    /// <param name="q_arg"></param>
    /// <param name="z_arg"></param>
    /// <param name="w_arg"></param>
    public static void Broadcast<T, K, Q, Z,W>(EventType eventType, T t_arg, K k_arg, Q q_arg, Z z_arg,W w_arg)
    {
        Delegate d;
        // 获取回调
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack<T, K, Q, Z, W> callBack = (CallBack<T, K, Q, Z, W>)d;
            // 如果委托不为空
            if (callBack != null)
            {
                // 进行广播
                callBack(t_arg, k_arg, q_arg, z_arg,w_arg);
            }
            // 否则抛出异常
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应的委托具有不同的类型", eventType));
            }
        }
    }
    #endregion
}