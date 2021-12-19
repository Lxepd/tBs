using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ����ʱ��
/// </summary>
public class Timer
{
    // �Ƿ��Զ�ѭ����<=0���ã�
    public bool isAutoLoop
    { 
        get; 
        private set; 
    }
    // �Ƿ���ͣ
    public bool isStop 
    { 
        get; 
        private set; 
    }
    // ��ǰʱ��
    public float nowTime 
    { 
        get { return UpdateNowTime(); } 
    } 
    // �Ƿ�ʱ�䵽
    public bool isTimeUp 
    {
        get { return nowTime <= 0; }
    }
    // ��ʱʱ�䳤��
    public float Duration 
    {
        get;
        private set;
    }
    // ��һ�θ���ʱ��
    private float lastTime;
    // ��һ�θ��µ���ʱ��֡��������һ֡��θ��£�
    private int lastUpdateFrame;
    // ��ǰ��ʱ��ʣ��ʱ��
    private float curTime;
    /// <summary>
    /// ���쵹��ʱ��
    /// </summary>
    /// <param name="duration">ʱ��</param>
    /// <param name="autoLoop">�Ƿ��Զ�ѭ��</param>
    /// <param name="autoStart">�Ƿ��Զ���ʼ</param>
    public Timer(float duration, bool autoLoop=false,bool autoStart = true)
    {
        isStop = true;
        Duration = Mathf.Max(0f, duration);
        isAutoLoop = autoLoop;
        Reset(duration, !autoStart);
    }
    /// <summary>
    /// ���¼�ʱ��ʱ��
    /// </summary>
    /// <returns>����ʣ��ʱ��</returns>
    private float UpdateNowTime()
    {
        // ��ͣ ���� ��һ֡�Ѿ����¹�
        if (isStop || lastUpdateFrame == Time.frameCount)
            // ����ʣ��ʱ��
            return curTime;
        // ���ʣ��ʱ��С�ڵ���0
        if(curTime <=0)
        {
            // �����ѭ��
            if(isAutoLoop)
                // ����ʱ��
                Reset(Duration, false);

            return curTime;
        }
        // ʣ��ʱ�� -= ��Ϸ��ʼʱ�� - ��һ�θ���ʱ��
        curTime -= Time.time - lastTime;

        UpdateLastTimeInfo();

        return curTime;
    }
    /// <summary>
    /// ����ʱ������Ϣ
    /// </summary>
    private void UpdateLastTimeInfo()
    {
        lastTime = Time.time;
        lastUpdateFrame = Time.frameCount;
    }
    /// <summary>
    /// ��ʼ��ʱ��ȡ����ͣ״̬
    /// </summary>
    public void Start()
    {
        Reset(Duration, false);
    }
    /// <summary>
    /// ���ü�ʱ��
    /// </summary>
    /// <param name="duration">����ʱ��</param>
    /// <param name="isStop">�Ƿ���ͣ</param>
    public void Reset(float duration,bool isStop=false)
    {
        // ���±��
        UpdateLastTimeInfo();
        // ������ʱ��
        Duration = Mathf.Max(0f, duration);
        // ʣ��ʱ���ʼ��
        curTime = Duration;
        // ��ͣ״̬����
        this.isStop = isStop;
    }
    /// <summary>
    /// ��ͣ��ʱ
    /// </summary>
    public void Pause()
    {
        // ��ͣǰ����ʱ����Ϣ
        UpdateLastTimeInfo();
        isStop = true;
    }
    /// <summary>
    /// �ָ���ʱ
    /// </summary>
    public void Continue()
    {
        // �ָ�ǰ����ʱ����Ϣ
        UpdateLastTimeInfo();
        isStop = false;
    }
    /// <summary>
    /// ��������ͣ��������ֵΪ0
    /// </summary>
    public void End()
    {
        // ��ͣ
        isStop = true;
        // ʣ��ʱ��Ϊ0
        curTime = 0;
    }
    /// <summary>
    /// ��ȡ����ʱ����ʣ�0Ϊû��ʼ��ʱ��1Ϊ��ʱ������
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