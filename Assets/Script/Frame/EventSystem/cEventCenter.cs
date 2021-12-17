using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �¼����ͣ�������
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
/// �¼�����
/// </summary>
public class cEventCenter:InstanceNoMono<cEventCenter>
{
    /// <summary>
    /// �¼��ֵ�
    /// </summary>
    private static Dictionary<EventType, Delegate> m_EventDic = new Dictionary<EventType, Delegate>();

    /// <summary>
    /// ��Ӽ���
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    private static void OnListenerAdding(EventType eventType, Delegate callBack)
    {
        // �ж��ֵ��Ƿ�����������¼�
        if (!m_EventDic.ContainsKey(eventType))
        {
            // ������� ������
            m_EventDic.Add(eventType, null);
        }
        // ��ȡ�������¼�
        Delegate d = m_EventDic[eventType];
        // �ж�ί�в�Ϊ�ղ��ж�ί�е����Ͳ����ϻص�����
        if (d != null && d.GetType() != callBack.GetType())
        {
            // �׳��쳣
            throw new Exception(string.Format("����Ϊ�¼�{0}��Ӳ�ͬ���͵�ί�У���ǰ�¼���Ӧί����{1}��Ҫ��ӵ�ί������Ϊ{2}", eventType, d.GetType(), callBack.GetType()));
        }
    }
    /// <summary>
    /// �Ƴ�����
    /// </summary>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    private static void OnListenerRemove(EventType eventType, Delegate callBack)
    {
        // �ֵ���ί������
        if (m_EventDic.ContainsKey(eventType))
        {
            // ȡ��ί��
            Delegate d = m_EventDic[eventType];
            // ���ί��Ϊ�գ����׳��쳣
            if (d == null)
            {
                throw new Exception(string.Format("�Ƴ����������¼�{0}û�ж�Ӧ��ί��", eventType));
            }
            // ����Ƴ���ί������ �� �ֵ��ڵ����� ��һ�£����׳��쳣
            else if (d.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("�Ƴ��������󣺳���Ϊ�¼�{0}�Ƴ���ͬ���͵�ί�У���ǰ���͵�ί��Ϊ{1}��Ҫ�Ƴ���ί������Ϊ{2}", eventType, d.GetType(), callBack.GetType()));
            }
        }
        // �ֵ����޸�ί������
        else
        {
            throw new Exception(string.Format("�Ƴ���������û�и�ί���¼���{0}", eventType));
        }
    }
    /// <summary>
    /// ����Ƴ���֮���ֵ���ί���Ƿ�Ϊ��
    /// </summary>
    /// <param name="eventType"></param>
    private static void OnListenerRemoved(EventType eventType)
    {
        // ����ֵ���Ϊ���ˣ������ֵ����Ƴ�
        if (m_EventDic[eventType] == null)
        {
            m_EventDic.Remove(eventType);
        }
    }
    
    #region ����¼�
    /// <summary>
    /// ����¼����޲Σ�
    /// </summary>
    /// <param name="eventType">ί������</param>
    /// <param name="callBack">ί��</param>
    public static void AddListener(EventType eventType, CallBack callBack)
    {
        OnListenerAdding(eventType, callBack);
        // ���ί�е����� �� �ص�����һ��
        // ����ί�й���
        m_EventDic[eventType] = (CallBack)m_EventDic[eventType] + callBack;

    }
    /// <summary>
    /// ����¼���1���Σ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerAdding(eventType, callBack);
        // ���ί�е����� �� �ص�����һ��
        // ����ί�й���
        m_EventDic[eventType] = (CallBack<T>)m_EventDic[eventType] + callBack;

    }
    /// <summary>
    /// ����¼���2���Σ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T, K>(EventType eventType, CallBack<T, K> callBack)
    {
        OnListenerAdding(eventType, callBack);
        // ���ί�е����� �� �ص�����һ��
        // ����ί�й���
        m_EventDic[eventType] = (CallBack<T, K>)m_EventDic[eventType] + callBack;

    }
    /// <summary>
    /// ����¼���3���Σ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void AddListener<T, K, Q>(EventType eventType, CallBack<T, K, Q> callBack)
    {
        OnListenerAdding(eventType, callBack);
        // ���ί�е����� �� �ص�����һ��
        // ����ί�й���
        m_EventDic[eventType] = (CallBack<T, K, Q>)m_EventDic[eventType] + callBack;

    }
    /// <summary>
    /// ����¼���4���Σ�
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
        // ���ί�е����� �� �ص�����һ��
        // ����ί�й���
        m_EventDic[eventType] = (CallBack<T, K, Q, Z>)m_EventDic[eventType] + callBack;

    }
    /// <summary>
    /// ����¼���5���Σ�
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
        // ���ί�е����� �� �ص�����һ��
        // ����ί�й���
        m_EventDic[eventType] = (CallBack<T, K, Q, Z, W>)m_EventDic[eventType] + callBack;

    }
    #endregion
   
    #region �Ƴ�����
    /// <summary>
    /// �Ƴ��������޲Σ�
    /// </summary>
    /// <param name="eventType">ί������</param>
    /// <param name="callBack">ί��</param>
    public static void RemoveListener(EventType eventType, CallBack callBack)
    {
        OnListenerRemove(eventType, callBack);
        // �Ƴ�
        m_EventDic[eventType] = (CallBack)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    /// <summary>
    /// �Ƴ�������1���Σ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T>(EventType eventType, CallBack<T> callBack)
    {
        OnListenerRemove(eventType, callBack);

        // �Ƴ�
        m_EventDic[eventType] = (CallBack<T>)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    /// <summary>
    /// �Ƴ�������2���Σ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T, K>(EventType eventType, CallBack<T, K> callBack)
    {
        OnListenerRemove(eventType, callBack);

        // �Ƴ�
        m_EventDic[eventType] = (CallBack<T, K>)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    /// <summary>
    /// �Ƴ�������3���Σ�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="Q"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="callBack"></param>
    public static void RemoveListener<T, K, Q>(EventType eventType, CallBack<T, K, Q> callBack)
    {
        OnListenerRemove(eventType, callBack);

        // �Ƴ�
        m_EventDic[eventType] = (CallBack<T, K, Q>)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    /// <summary>
    /// �Ƴ�������4���Σ�
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

        // �Ƴ�
        m_EventDic[eventType] = (CallBack<T, K, Q, Z>)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    /// <summary>
    /// �Ƴ�������5���Σ�
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

        // �Ƴ�
        m_EventDic[eventType] = (CallBack<T, K, Q, Z, W>)m_EventDic[eventType] - callBack;
        OnListenerRemoved(eventType);
    }

    #endregion
    
    #region �㲥
    /// <summary>
    /// �޲ι㲥
    /// </summary>
    /// <param name="eventType">ί������</param>
    public static void Broadcast(EventType eventType)
    {
        Delegate d;
        // ��ȡ�ص�
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack callBack = (CallBack)d;
            // ���ί�в�Ϊ��
            if (callBack != null)
            {
                // ���й㲥
                callBack();
            }
            // �����׳��쳣
            else
            {
                throw new Exception(string.Format("�㲥�¼������¼�{0}��Ӧ��ί�о��в�ͬ������", eventType));
            }
        }
    }
    /// <summary>
    /// ��1���εĹ㲥
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="arg"></param>
    public static void Broadcast<T>(EventType eventType, T t_arg)
    {
        Delegate d;
        // ��ȡ�ص�
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack<T> callBack = (CallBack<T>)d;
            // ���ί�в�Ϊ��
            if (callBack != null)
            {
                // ���й㲥
                callBack(t_arg);
            }
            // �����׳��쳣
            else
            {
                throw new Exception(string.Format("�㲥�¼������¼�{0}��Ӧ��ί�о��в�ͬ������", eventType));
            }
        }
    }
    /// <summary>
    /// ��2���εĹ㲥
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    /// <param name="eventType"></param>
    /// <param name="t_arg"></param>
    /// <param name="k_arg"></param>
    public static void Broadcast<T,K>(EventType eventType, T t_arg,K k_arg)
    {
        Delegate d;
        // ��ȡ�ص�
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack<T, K> callBack = (CallBack<T, K>)d;
            // ���ί�в�Ϊ��
            if (callBack != null)
            {
                // ���й㲥
                callBack(t_arg,k_arg);
            }
            // �����׳��쳣
            else
            {
                throw new Exception(string.Format("�㲥�¼������¼�{0}��Ӧ��ί�о��в�ͬ������", eventType));
            }
        }
    }
    /// <summary>
    /// ��3���εĹ㲥
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
        // ��ȡ�ص�
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack<T, K, Q> callBack = (CallBack<T, K, Q>)d;
            // ���ί�в�Ϊ��
            if (callBack != null)
            {
                // ���й㲥
                callBack(t_arg, k_arg, q_arg);
            }
            // �����׳��쳣
            else
            {
                throw new Exception(string.Format("�㲥�¼������¼�{0}��Ӧ��ί�о��в�ͬ������", eventType));
            }
        }
    }
    /// <summary>
    /// ��4���εĹ㲥
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
        // ��ȡ�ص�
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack<T, K, Q, Z> callBack = (CallBack<T, K, Q, Z>)d;
            // ���ί�в�Ϊ��
            if (callBack != null)
            {
                // ���й㲥
                callBack(t_arg, k_arg, q_arg, z_arg);
            }
            // �����׳��쳣
            else
            {
                throw new Exception(string.Format("�㲥�¼������¼�{0}��Ӧ��ί�о��в�ͬ������", eventType));
            }
        }
    }
    /// <summary>
    /// ��5���εĹ㲥
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
        // ��ȡ�ص�
        if (m_EventDic.TryGetValue(eventType, out d))
        {
            CallBack<T, K, Q, Z, W> callBack = (CallBack<T, K, Q, Z, W>)d;
            // ���ί�в�Ϊ��
            if (callBack != null)
            {
                // ���й㲥
                callBack(t_arg, k_arg, q_arg, z_arg,w_arg);
            }
            // �����׳��쳣
            else
            {
                throw new Exception(string.Format("�㲥�¼������¼�{0}��Ӧ��ί�о��в�ͬ������", eventType));
            }
        }
    }
    #endregion
}