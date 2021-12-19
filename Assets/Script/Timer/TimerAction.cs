using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 倒计时器
/// </summary>
public class Timer
{
    // 是否自动循环（<=0重置）
    public bool isAutoLoop
    { 
        get; 
        private set; 
    }
    // 是否暂停
    public bool isStop 
    { 
        get; 
        private set; 
    }
    // 当前时间
    public float nowTime 
    { 
        get { return UpdateNowTime(); } 
    } 
    // 是否时间到
    public bool isTimeUp 
    {
        get { return nowTime <= 0; }
    }
    // 计时时间长度
    public float Duration 
    {
        get;
        private set;
    }
    // 上一次更新时间
    private float lastTime;
    // 上一次更新倒计时的帧数（避免一帧多次更新）
    private int lastUpdateFrame;
    // 当前计时器剩余时间
    private float curTime;
    /// <summary>
    /// 构造倒计时器
    /// </summary>
    /// <param name="duration">时长</param>
    /// <param name="autoLoop">是否自动循环</param>
    /// <param name="autoStart">是否自动开始</param>
    public Timer(float duration, bool autoLoop=false,bool autoStart = true)
    {
        isStop = true;
        Duration = Mathf.Max(0f, duration);
        isAutoLoop = autoLoop;
        Reset(duration, !autoStart);
    }
    /// <summary>
    /// 更新计时器时间
    /// </summary>
    /// <returns>返回剩余时间</returns>
    private float UpdateNowTime()
    {
        // 暂停 或者 这一帧已经更新过
        if (isStop || lastUpdateFrame == Time.frameCount)
            // 返回剩余时间
            return curTime;
        // 如果剩余时间小于等于0
        if(curTime <=0)
        {
            // 如果是循环
            if(isAutoLoop)
                // 重置时间
                Reset(Duration, false);

            return curTime;
        }
        // 剩余时间 -= 游戏开始时间 - 上一次更新时间
        curTime -= Time.time - lastTime;

        UpdateLastTimeInfo();

        return curTime;
    }
    /// <summary>
    /// 更新时间标记信息
    /// </summary>
    private void UpdateLastTimeInfo()
    {
        lastTime = Time.time;
        lastUpdateFrame = Time.frameCount;
    }
    /// <summary>
    /// 开始计时，取消暂停状态
    /// </summary>
    public void Start()
    {
        Reset(Duration, false);
    }
    /// <summary>
    /// 重置计时器
    /// </summary>
    /// <param name="duration">持续时间</param>
    /// <param name="isStop">是否暂停</param>
    public void Reset(float duration,bool isStop=false)
    {
        // 更新标记
        UpdateLastTimeInfo();
        // 更新总时长
        Duration = Mathf.Max(0f, duration);
        // 剩余时间初始化
        curTime = Duration;
        // 暂停状态更新
        this.isStop = isStop;
    }
    /// <summary>
    /// 暂停计时
    /// </summary>
    public void Pause()
    {
        // 暂停前更新时间信息
        UpdateLastTimeInfo();
        isStop = true;
    }
    /// <summary>
    /// 恢复计时
    /// </summary>
    public void Continue()
    {
        // 恢复前更新时间信息
        UpdateLastTimeInfo();
        isStop = false;
    }
    /// <summary>
    /// 结束，暂停并设置其值为0
    /// </summary>
    public void End()
    {
        // 暂停
        isStop = true;
        // 剩余时间为0
        curTime = 0;
    }
    /// <summary>
    /// 获取倒计时完成率（0为没开始计时，1为计时结束）
    /// </summary>
    /// <returns></returns>
    public float GetPercent()
    {
        UpdateLastTimeInfo();

        if(curTime<=0||Duration<=0)
        {
            return 1f;
        }

        return 1f - curTime / Duration;
    }
}