using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��Դ����
/// 1. �첽����
/// 2. ί�� �� lambda���ʽ
/// 3. Э��
/// 4. ����
/// </summary>
public class ResMgr : InstanceNoMono<ResMgr>
{
    // ͬ��������Դ
    public T Load<T>(string name) where T:Object
    {
        T res = Resources.Load<T>(name);
        // ���������һ��GameObject���ͣ�ֱ��ʵ������ȥ��ֱ��ʹ��
        if (res is GameObject)
            return Object.Instantiate(res);
        // TextAsset AudioClip
        else
            return res;
    }

    // �첽������Դ
    public void LoadAsync<T>(string name, UnityAction<T> callBack) where T :Object
    {
        // �����첽����Э��
        MonoMgr.GetInstance().StartCoroutine(ReallyLoadAsync(name, callBack));
    }
    // ���� �����첽���ض�Ӧ����Դ
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
