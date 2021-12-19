using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FinishEventHandler();
public delegate void UpdateEventHandler(float time);

public class MyTimer : MonoBehaviour
{
    /// 打印开关
    public bool isLog = true;
    /// 实时调用委托
    UpdateEventHandler updateEvent;
    /// 实时调用委托
    FinishEventHandler finishEvent;
    /// 计时时间
    float timeTarget;
    /// 开始计时时间
    float timeStart;
    /// 现在时间
    float timeNow;
    /// 计时偏差
    float offsetTime;
    /// 是否开始计时
    bool isTimer;
    /// 计时结束后是否销毁
    bool isDestory = true;
    /// 计时是否结束
    bool isEnd;
    /// 是否忽略时间速率
    bool isIgnoreTimeScale = true;
    /// 是否重复
    bool isRepeate;
    /// 真实时间
    float _Time
    {
        get
        {
            /// realtimeSinceStartup 是开始运行的时间切换应用不对其影响（真是时间）
            /// time 是开始运行的时间切换应用不对其影响（实际时间）
            return isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
        }
    }
    /// 当前所用时间
    float now;
    /// 创建计时器：名字
    public static MyTimer CreateTimer(string objName = "Timer")
    {
        GameObject go = new GameObject(objName);
        MyTimer timer = go.AddComponent<MyTimer>();
        return timer;
    }
    private void Update()
    {
        if (!isTimer)
        {
            return;
        }
        timeNow = _Time - offsetTime;
        now = timeNow - timeStart;
        if (updateEvent != null)
        {
            //与目标值的比例当为1的时候完成
            updateEvent(Mathf.Clamp01(now / timeTarget));
        }
        if (now > timeTarget) //超过目标值
        {
            if (finishEvent != null)
            {
                finishEvent();
            }

            if (!isRepeate)
            {
                Destory();
            }
            else
            {
                ReStartTimer();
            }
        }
    }
    /// 经过的时间
    public float GetLeftTime()
    {
        return Mathf.Clamp(timeTarget - now, 0, timeTarget);
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PauseTimer();
        }
        else
        {
            ConnitueTimer();
        }
    }
    /// 计时结束
    private void Destory()
    {
        isTimer = false;
        isEnd = true;
        if (isDestory)
        {
            Destroy(gameObject);
        }
    }
    /// 暂停时间
    float _pauseTime;
    /// 暂停计时器
    public void PauseTimer()
    {
        if (isEnd)
        {
            if (isLog) 
                Debug.LogError("计时已经结束");
        }
        else
        {
            if (isTimer)
            {
                isTimer = false;
                _pauseTime = _Time;
            }
        }
    }
    /// 重新计时
    public void ConnitueTimer()
    {
        if (isEnd)
        {
            if (isLog) 
                Debug.LogError("计时已经结束!请重新开始!");
        }
        else
        {
            if (!isTimer)
            {
                offsetTime += (_Time - _pauseTime);
                isTimer = true;
            }
        }
    }
    /// 重新计时
    public void ReStartTimer()
    {
        if (!isTimer)
        {
            isTimer = true;
        }

        if (isEnd)
        {
            isEnd = false;
        }

        timeStart = _Time;
        offsetTime = 0;
        _pauseTime = 0;
    }
    /// 停止计时器
    public void StopTimer()
    {
        Destory();
    }
    /// 开始计时
    public void StartTiming(float _time, FinishEventHandler finishdel, UpdateEventHandler updatedel = null, bool _isIgnoreTimeScale = true, bool _isRepeate = false, bool _isDestory = true)
    {
        //设置目标时长
        timeTarget = _time;

        //设置委托
        if (finishdel != null)
        {
            finishEvent = finishdel;
        }

        if (updatedel != null)
        {
            updateEvent = updatedel;
        }

        isDestory = _isDestory;
        isIgnoreTimeScale = _isIgnoreTimeScale;
        isRepeate = _isRepeate;

        timeStart = _Time;
        offsetTime = 0;
        isEnd = false;
        isTimer = true;
    }

}