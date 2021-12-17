using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 缓存池数据
/// </summary>
public class PoolData
{
    // 对象父节点
    public GameObject fatherObj;
    // 存放的对象容器
    public List<GameObject> poolList;

    public PoolData(GameObject _parent, GameObject _obj)
    {
        fatherObj = new GameObject(_obj.name);
        fatherObj.transform.parent = _parent.transform;

        poolList = new List<GameObject>() { };
        PushObj(_obj);
    }
    /// <summary>
    /// 放东西并设置父节点
    /// </summary>
    /// <param name="obj">放啥</param>
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        poolList.Add(obj);
        obj.transform.parent = fatherObj.transform;
    }
    /// <summary>
    /// 拿东西
    /// </summary>
    /// <returns></returns>
    public GameObject GetObj()
    {
        GameObject obj = null;

        obj = poolList[0];
        poolList.RemoveAt(0);
        obj.SetActive(true);

        return obj;
    }
}
/// <summary>
/// 缓存池
/// </summary>
public class PoolMgr:InstanceNoMono<PoolMgr>
{
    // 缓存池容器
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject objParent;

    /// <summary>
    /// 拿东西
    /// </summary>
    /// <param name="name"></param>
    public void GetObj(string name,UnityAction<GameObject> callBack)
    {
        if (poolDic.ContainsKey(name) && poolDic[name].poolList.Count > 0)
        {
            callBack(poolDic[name].GetObj());
        }
        else
        {
            // 通过异步加载资源，创建对象给外部用
            ResMgr.GetInstance().LoadAsync<GameObject>(name, (x) =>
            {
                x.name = name;
                callBack(x);
            });

            //obj = Object.Instantiate(Resources.Load<GameObject>(name));
            //obj.name = name;
        }

    }
   /// <summary>
   /// 放东西
   /// </summary>
   /// <param name="name">放哪里</param>
   /// <param name="obj">放的东西是啥</param>
    public void PushObj(string name,GameObject obj)
    {
        if (objParent==null)
        {
            objParent = new GameObject("Pool");
        }

        if(poolDic.ContainsKey(name))
        {
            poolDic[name].PushObj(obj);
        }
        else
        {
            poolDic.Add(name, new PoolData(objParent,obj));
        }
    }
    /// <summary>
    /// 清空缓存池
    /// 切换场景时使用
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        objParent = null;
    }
}