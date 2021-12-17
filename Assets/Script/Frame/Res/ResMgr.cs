using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载
/// 1. 异步加载
/// 2. 委托 和 lambda表达式
/// 3. 协程
/// 4. 泛型
/// </summary>
public class ResMgr : InstanceNoMono<ResMgr>
{
    // 同步加载资源
    public T Load<T>(string name) where T:Object
    {
        T res = Resources.Load<T>(name);
        // 如果对象是一个GameObject类型，直接实例化出去，直接使用
        if (res is GameObject)
            return Object.Instantiate(res);
        // TextAsset AudioClip
        else
            return res;
    }

    // 异步加载资源
    public void LoadAsync<T>(string name, UnityAction<T> callBack) where T :Object
    {
        // 开启异步加载协程
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync(name, callBack));
    }
    // 用于 开启异步加载对应的资源
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callBack) where T:Object
    {
        var r = Resources.LoadAsync<T>(name);
        yield return r;

        if (r.asset is GameObject)
            callBack(Object.Instantiate(r.asset) as T);
        else
            callBack(r.asset as T);
    }
}
