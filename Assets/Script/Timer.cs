using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer
{
    /// <summary>
    /// ί��ִ��ʱ��
    /// </summary>
    public float time;
    /// <summary>
    /// ί��
    /// </summary>
    public Action action;
    /// <summary>
    /// �Ƿ��ظ�
    /// </summary>
    public bool isLoop;
    /// <summary>
    /// ���ü�ʱʱ��
    /// </summary>
    public float curTime;
    /// <summary>
    /// ��ί���Ƿ���ͣ
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
    /// �Ѿ�ע��ļ�ʱ�����ֵ�
    /// </summary>
    private Dictionary<string, Timer> timeActionDic = new Dictionary<string, Timer>();
    /// <summary>
    /// ��ʱ�����
    /// </summary>
    private List<Timer> actTaskTable = new List<Timer>();

    public TimeAction()
    {
        MonoMgr.GetInstance().AddUpdateListener(OnUpdate);
    }
    /// <summary>
    /// ����
    /// </summary>
    private void OnUpdate()
    {
        if (actTaskTable.Count < 1)
            return;

        float deltaTime = Time.deltaTime;

        for (int i = 0; i < actTaskTable.Count; i++)
        {
            // ��������ͣ״̬
            if (actTaskTable[i].isPause)
                return;

            actTaskTable[i].curTime += deltaTime;
            // �������ʱ�� �� ִ�������ʱ��
            if(actTaskTable[i].curTime >= actTaskTable[i].time)
            {
                // �������ί��
                actTaskTable[i].action();
                // ���������ѭ������
                if(!actTaskTable[i].isLoop)
                {
                    // �������б��Ƴ�������
                    actTaskTable.RemoveAt(i--);
                }
                // �����ѭ������
                else
                {
                    // ����ʱ������
                    actTaskTable[i].curTime = 0;
                }
            }
        }

    }
    /// <summary>
    /// ע������
    /// </summary>
    /// <param name="name">������</param>
    /// <param name="time">ִ��ʱ��</param>
    /// <param name="act">ִ�е�ί��</param>
    /// <param name="loop">�Ƿ��ظ�</param>
    public void RegisterTimerTask(string name, float time, Action act, bool loop)
    {
        if (!timeActionDic.ContainsKey(name))
        {
            timeActionDic.Add(name, new Timer(time, act, loop));
        }
    }
    /// <summary>
    /// ��ͣ����
    /// </summary>
    /// <param name="name">ָ������</param>
    public void PauseActionTask(string name)
    {
        if (!timeActionDic.ContainsKey(name))
        {
            throw new Exception(string.Format("�ֵ�����û��ע��<{0}>����", name));
        }

        timeActionDic[name].isPause = true;
    }
    /// <summary>
    /// �ָ�����
    /// </summary>
    /// <param name="name"></param>
    public void UnPauseActionTask(string name)
    {
        if (!timeActionDic.ContainsKey(name))
        {
            throw new Exception(string.Format("�ֵ�����û��ע��<{0}>����", name));
        }

        timeActionDic[name].isPause = false;
    }
    /// <summary>
    /// �������
    /// </summary>
    public void Clear()
    {
        timeActionDic.Clear();
    }
}
