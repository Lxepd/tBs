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
/// �¼�����  ����ģʽ����
/// 1. Dictionary
/// 2. ί��
/// 3. �۲���ģʽ
/// </summary>
public class EventCenter:InstanceNoMono<EventCenter>
{
    /// <summary>
    /// �¼��ֵ�
    /// key -----> �¼����֣��磺������������������ȣ�
    /// value -----> ����������¼�����Ӧ��ί�к���
    /// </summary>
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();
    /// <summary>
    /// ����¼�����
    /// </summary>
    /// <param name="name">�¼�����</param>
    /// <param name="action">��Ҫ������¼�ί�к���</param>
    public void AddEventListener<T>(string name,UnityAction<T> action)
    {
        // �ж����޶�Ӧ�¼�
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
        // �ж����޶�Ӧ�¼�
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
    /// �Ƴ���Ӧ���¼�����
    /// </summary>
    /// <param name="name">�¼�����</param>
    /// <param name="action">���ֵ��ڵ�ί�к���</param>
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
    /// �¼�����
    /// </summary>
    /// <param name="name">��һ�������¼�����</param>
    public void EventTrigger<T>(string name,T info)
    {
        // �ж����޶�Ӧ�¼�
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo<T>).actions != null)
                (eventDic[name] as EventInfo<T>).actions.Invoke(info);
        }

    }
    public void EventTrigger(string name)
    {
        // �ж����޶�Ӧ�¼�
        if (eventDic.ContainsKey(name))
        {
            if ((eventDic[name] as EventInfo).actions != null)
                (eventDic[name] as EventInfo).actions.Invoke();
        }

    }
    /// <summary>
    /// ����¼��ֵ�
    /// ���ڳ����л�
    /// </summary>
    public void Clear()
    {
        eventDic.Clear();
    }
}
