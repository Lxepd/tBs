using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer
{
    /// <summary>
    /// 委托执行时间
    /// </summary>
    public float time;
    /// <summary>
    /// 委托
    /// </summary>
    public Action action;
    /// <summary>
    /// 是否重复
    /// </summary>
    public bool isLoop;
    /// <summary>
    /// 内置计时时间
    /// </summary>
    public float curTime;
    /// <summary>
    /// 该委托是否暂停
    /// </summary>
    public bool isPause;
    public Timer(float _t,Action act,bool _loop)
    {
        time = _t;
        action = act;
        isLoop = _loop;
    }
}
public class TimeAction : InstanceNoMono<TimeAction>
{
    /// <summary>
    /// 已经注册的计时任务字典
    /// </summary>
    private Dictionary<string, Timer> timeActionDic = new Dictionary<string, Timer>();
    /// <summary>
    /// 计时任务表
    /// </summary>
    private List<Timer> actTaskTable = new List<Timer>();
    /// <summary>
    /// 是否在计时
    /// </summary>
    public bool isTimer;
    /// <summary>
    /// 记录按键按下之后的时间，用于执行事件
    /// </summary>
    private float currentTime;

    //public TimeAction()
    //{
    //    MonoMgr.GetInstance().AddUpdateListener(OnUpdate);
    //}
    /// <summary>
    /// 更新
    /// </summary>
    public void OnUpdate()
    {
        if (!isTimer)
            return;

        float deltaTime = Time.deltaTime;
        // currentTime += deltaTime;

        for (int i = 0; i < actTaskTable.Count; i++)
        {
            // 任务是暂停状态
            if (actTaskTable[i].isPause)
                return;
            actTaskTable[i].curTime += deltaTime;
            // 如果内置时间 到 执行任务的时间
            if (actTaskTable[i].curTime >= actTaskTable[i].time)
            {
                // 该任务的委托
                actTaskTable[i].action();
                // 如果不属于循环任务
                if(!actTaskTable[i].isLoop)
                {
                    // 在任务列表移除该任务
                    actTaskTable.RemoveAt(i--);
                }
                // 如果是循环任务
                else
                {
                    // 内置时间清零
                    actTaskTable[i].curTime = 0;
                }
            }
        }

        if(actTaskTable.Count==0)
        {
            isTimer = false;
        }

    }
    /// <summary>
    /// 注册任务
    /// </summary>
    /// <param name="name">任务名</param>
    /// <param name="time">执行时间</param>
    /// <param name="act">执行的委托</param>
    /// <param name="loop">是否重复</param>
    public void RegisterTimerTask(string name, float time, Action act, bool loop = false)
    {
        if (!timeActionDic.ContainsKey(name))
        {
            timeActionDic.Add(name, new Timer(time, act, loop));
        }
    }
    public void PlayTimerTask(string name)
    {
        if (!timeActionDic.ContainsKey(name))
        {
            throw new Exception(string.Format("字典里面没有注册<{0}>任务", name));
        }

        actTaskTable.Add(timeActionDic[name]);
        isTimer = true;

        foreach (var item in actTaskTable)
        {
            item.curTime = 0;
        }
    }
    /// <summary>
    /// 停止播放
    /// </summary>
    public void Stop()
    {
        if (!isTimer)
            return;


        isTimer = false;
        Clear();
    }
    /// <summary>
    /// 暂停任务
    /// </summary>
    /// <param name="name">指定任务</param>
    public void PauseActionTask(string name)
    {
        if (!timeActionDic.ContainsKey(name))
        {
            throw new Exception(string.Format("字典里面没有注册<{0}>任务", name));
        }

        timeActionDic[name].isPause = true;
    }
    /// <summary>
    /// 恢复任务
    /// </summary>
    /// <param name="name"></param>
    public void UnPauseActionTask(string name)
    {
        if (!timeActionDic.ContainsKey(name))
        {
            throw new Exception(string.Format("字典里面没有注册<{0}>任务", name));
        }

        timeActionDic[name].isPause = false;
    }
    /// <summary>
    /// 清空任务
    /// </summary>
    public void Clear()
    {
        actTaskTable.Clear();
    }
}
