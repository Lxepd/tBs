using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 1. ���ⲿû�̳�Mono�Ľű��ṩ֡���µķ���
/// 2. ���ⲿ�ṩЭ�̵ķ���
/// </summary>
public class MonoMgr : InstanceNoMono<MonoMgr>
{
    private MonoCtrl ctrl;

    public MonoMgr()
    {
        // ��֤MonoCtrl��Ψһ��
        GameObject obj = new GameObject("MonoCtrl");
        ctrl = obj.AddComponent<MonoCtrl>();
    }

    /// <summary>
    /// ���ⲿ�ṩ�� ���֡�����¼�
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        ctrl.AddUpdateListener(fun);
    }
    /// <summary>
    /// ���ⲿ�ṩ�� �Ƴ�֡�����¼�
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        ctrl.RemoveUpdateListener(fun);
    }
    /// <summary>
    /// ����Э�̵ĺ���
    /// </summary>
    /// <param name="routine"></param>
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return ctrl.StartCoroutine(routine);
    }
    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value)
    {
        return ctrl.StartCoroutine(methodName, value);
    }
    public Coroutine StartCoroutine(string methodName)
    {
        return ctrl.StartCoroutine(methodName);
    }
    public void StopCoroutine(IEnumerator routine)
    {
        ctrl.StopCoroutine(routine);
    }
    public void StopCoroutine(Coroutine routine)
    {
        ctrl.StopCoroutine(routine);
    }
    public void StopCoroutine(string methodName)
    {
        ctrl.StopCoroutine(methodName);
    }
}
