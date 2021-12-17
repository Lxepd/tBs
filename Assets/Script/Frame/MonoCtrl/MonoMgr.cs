using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 1. 给外部没继承Mono的脚本提供帧更新的方法
/// 2. 给外部提供协程的方法
/// </summary>
public class MonoMgr : InstanceNoMono<MonoMgr>
{
    private MonoCtrl ctrl;

    public MonoMgr()
    {
        // 保证MonoCtrl的唯一性
        GameObject obj = new GameObject("MonoCtrl");
        ctrl = obj.AddComponent<MonoCtrl>();
    }

    /// <summary>
    /// 给外部提供的 添加帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        ctrl.AddUpdateListener(fun);
    }
    /// <summary>
    /// 给外部提供的 移除帧更新事件
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        ctrl.RemoveUpdateListener(fun);
    }
    /// <summary>
    /// 开启协程的函数
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
