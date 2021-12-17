using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ����ʱ���¼�
/// </summary>
public class ActionBase
{
    /// <summary>
    /// ִ�е�ʱ��
    /// </summary>
    public float timer;
    /// <summary>
    /// ִ�е��¼�
    /// </summary>
    public Action action;

    public ActionBase(float _t, Action callBack)
    {
        timer = _t;
        action = callBack;
    }
}
/// <summary>
/// ���¼�
/// ���ֵ��а���<ĳ�¼���>��<���¶��¼�>����
/// </summary>
public class ActionsBase
{
    /// <summary>
    /// ���¼�
    /// </summary>
    public List<ActionBase> actions = new List<ActionBase>();
    public ActionsBase(float _t, Action callBack)
    {
        AddAction(new ActionBase(_t, callBack));
    }
    /// <summary>
    /// ����¼�
    /// </summary>
    /// <param name="ab">����ʱ���¼���ִ��ʱ�䣬ί�У�</param>
    public void AddAction(ActionBase ab)
    {
        actions.Add(ab);
    }
}
public class TimerAction : InstanceNoMono<TimerAction>
{
    /// <summary>
    /// �洢<ĳ�¼���>��<���¶��¼�>���ֵ�
    /// </summary>
    private Dictionary<string, ActionsBase> actionDic = new Dictionary<string, ActionsBase>();
    /// <summary>
    /// ��ʱ������Ҫִ��ָ���¼����µ��¼�
    /// </summary>
    private List<ActionBase> action = new List<ActionBase>();
    /// <summary>
    /// ��¼��������֮���ʱ�䣬����ִ���¼�
    /// </summary>
    private float currentTime;
    /// <summary>
    /// �ж��Ƿ�ʼ���� ��ʱ���¼�
    /// </summary>
    public bool isPlayer;
    public TimerAction()
    {
        MonoMgr.GetInstance().AddUpdateListener(ActionUpdate);
    }
    public void ActionUpdate()
    {
        if (isPlayer)
        {
            currentTime += Time.deltaTime;

            for (int i = 0; i < action.Count; i++)
            {
                if (action[i].timer <= currentTime)
                {
                    action[i].action();
                    action.RemoveAt(i--);
                }
            }


            if (action.Count == 0)
            {
                isPlayer = false;
                Debug.Log("�¼�ִ����ϣ���");
            }
        }
    }
    public void AddTimerActionDic(string name, float time = 0, Action callBack = null)
    {
        if(!actionDic.ContainsKey(name))
        {
            ActionsBase ab = new ActionsBase(time, callBack);
            actionDic.Add(name, ab);
        }
    }

    public void PlayerAction(string name)
    {
        if (actionDic.TryGetValue(name, out var act))
        {
            // ����ֱ������ ��ʱ��
            action.AddRange(actionDic[name].actions);
            isPlayer = true;
            currentTime = 0;
        }
    }

    private void Stop()
    {
        if (!isPlayer)
            return;

        isPlayer = false;
        action.Clear();
    }
}
