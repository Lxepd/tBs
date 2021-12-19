using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FinishEventHandler();
public delegate void UpdateEventHandler(float time);

public class MyTimer : MonoBehaviour
{
    /// ��ӡ����
    public bool isLog = true;
    /// ʵʱ����ί��
    UpdateEventHandler updateEvent;
    /// ʵʱ����ί��
    FinishEventHandler finishEvent;
    /// ��ʱʱ��
    float timeTarget;
    /// ��ʼ��ʱʱ��
    float timeStart;
    /// ����ʱ��
    float timeNow;
    /// ��ʱƫ��
    float offsetTime;
    /// �Ƿ�ʼ��ʱ
    bool isTimer;
    /// ��ʱ�������Ƿ�����
    bool isDestory = true;
    /// ��ʱ�Ƿ����
    bool isEnd;
    /// �Ƿ����ʱ������
    bool isIgnoreTimeScale = true;
    /// �Ƿ��ظ�
    bool isRepeate;
    /// ��ʵʱ��
    float _Time
    {
        get
        {
            /// realtimeSinceStartup �ǿ�ʼ���е�ʱ���л�Ӧ�ò�����Ӱ�죨����ʱ�䣩
            /// time �ǿ�ʼ���е�ʱ���л�Ӧ�ò�����Ӱ�죨ʵ��ʱ�䣩
            return isIgnoreTimeScale ? Time.realtimeSinceStartup : Time.time;
        }
    }
    /// ��ǰ����ʱ��
    float now;
    /// ������ʱ��������
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
            //��Ŀ��ֵ�ı�����Ϊ1��ʱ�����
            updateEvent(Mathf.Clamp01(now / timeTarget));
        }
        if (now > timeTarget) //����Ŀ��ֵ
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
    /// ������ʱ��
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
    /// ��ʱ����
    private void Destory()
    {
        isTimer = false;
        isEnd = true;
        if (isDestory)
        {
            Destroy(gameObject);
        }
    }
    /// ��ͣʱ��
    float _pauseTime;
    /// ��ͣ��ʱ��
    public void PauseTimer()
    {
        if (isEnd)
        {
            if (isLog) 
                Debug.LogError("��ʱ�Ѿ�����");
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
    /// ���¼�ʱ
    public void ConnitueTimer()
    {
        if (isEnd)
        {
            if (isLog) 
                Debug.LogError("��ʱ�Ѿ�����!�����¿�ʼ!");
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
    /// ���¼�ʱ
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
    /// ֹͣ��ʱ��
    public void StopTimer()
    {
        Destory();
    }
    /// ��ʼ��ʱ
    public void StartTiming(float _time, FinishEventHandler finishdel, UpdateEventHandler updatedel = null, bool _isIgnoreTimeScale = true, bool _isRepeate = false, bool _isDestory = true)
    {
        //����Ŀ��ʱ��
        timeTarget = _time;

        //����ί��
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