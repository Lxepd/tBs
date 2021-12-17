using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 单计时器事件
/// </summary>
public class ActionBase
{
    /// <summary>
    /// 执行的时间
    /// </summary>
    public float timer;
    /// <summary>
    /// 执行的事件
    /// </summary>
    public Action action;

    public ActionBase(float _t, Action callBack)
    {
        timer = _t;
        action = callBack;
    }
}
/// <summary>
/// 多事件
/// 在字典中扮演<某事件名>的<其下多事件>处理
/// </summary>
public class ActionsBase
{
    /// <summary>
    /// 多事件
    /// </summary>
    public List<ActionBase> actions = new List<ActionBase>();
    public ActionsBase(float _t, Action callBack)
    {
        AddAction(new ActionBase(_t, callBack));
    }
    /// <summary>
    /// 添加事件
    /// </summary>
    /// <param name="ab">单计时器事件（执行时间，委托）</param>
    public void AddAction(ActionBase ab)
    {
        actions.Add(ab);
    }
}
public class TimerAction : InstanceNoMono<TimerAction>
{
    /// <summary>
    /// 存储<某事件名>和<其下多事件>的字典
    /// </summary>
    private Dictionary<string, ActionsBase> actionDic = new Dictionary<string, ActionsBase>();
    /// <summary>
    /// 临时保存需要执行指定事件名下的事件
    /// </summary>
    private List<ActionBase> action = new List<ActionBase>();
    /// <summary>
    /// 记录按键按下之后的时间，用于执行事件
    /// </summary>
    private float currentTime;
    /// <summary>
    /// 判断是否开始进行 计时器事件
    /// </summary>
    private bool isPlayer;
    public TimerAction()
    {
        MonoMgr.GetInstance().AddUpdateListener(ActionUpdate);
    }
    public void ActionUpdate()
    {
        if (isPlayer)
        {
            currentTime += Time.deltaTime;

            foreach (var item in action)
            {
                if (item.timer <= currentTime)
                {
                    item.action();
                }
            }

            if (action.Count == 0)
            {
                isPlayer = false;
                Debug.Log("事件执行完毕！！");
            }
        }
    }
    public void PlayerAction(string name, float time = 0, Action callBack = null, bool isStop = true)
    {
        if (isStop)
        {
            Stop();
        }

        // 字典没有，则注册一个
        if (!actionDic.TryGetValue(name, out var act))
        {
            ActionsBase ab = new ActionsBase(time, callBack);
            actionDic.Add(name, ab);
            action.AddRange(ab.actions);
        }
        else
        {
            // 有则直接启用 计时器
            action.AddRange(act.actions);
        }

        isPlayer = true;
        currentTime = 0;
    }

    private void Stop()
    {
        if (!isPlayer)
            return;

        isPlayer = false;
        action.Clear();
    }
}
