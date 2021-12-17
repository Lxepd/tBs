using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���������
/// </summary>
public class PoolData
{
    // ���󸸽ڵ�
    public GameObject fatherObj;
    // ��ŵĶ�������
    public List<GameObject> poolList;

    public PoolData(GameObject _parent, GameObject _obj)
    {
        fatherObj = new GameObject(_obj.name);
        fatherObj.transform.parent = _parent.transform;

        poolList = new List<GameObject>() { };
        PushObj(_obj);
    }
    /// <summary>
    /// �Ŷ��������ø��ڵ�
    /// </summary>
    /// <param name="obj">��ɶ</param>
    public void PushObj(GameObject obj)
    {
        obj.SetActive(false);
        poolList.Add(obj);
        obj.transform.parent = fatherObj.transform;
    }
    /// <summary>
    /// �ö���
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
/// �����
/// </summary>
public class PoolMgr:InstanceNoMono<PoolMgr>
{
    // ���������
    public Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    private GameObject objParent;

    /// <summary>
    /// �ö���
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
            // ͨ���첽������Դ������������ⲿ��
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
   /// �Ŷ���
   /// </summary>
   /// <param name="name">������</param>
   /// <param name="obj">�ŵĶ�����ɶ</param>
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
    /// ��ջ����
    /// �л�����ʱʹ��
    /// </summary>
    public void Clear()
    {
        poolDic.Clear();
        objParent = null;
    }
}