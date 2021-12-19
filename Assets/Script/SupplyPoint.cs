using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// ��������
/// </summary>
public enum SupplyType
{
    None,
    ����,
    שͷ,
    ����,
    ��
}
/// <summary>
/// ��������
/// </summary>
[Serializable]
public class SupplyData
{
    /// <summary>
    /// ��������
    /// </summary>
    public int supplyNum;
    /// <summary>
    /// ��������
    /// </summary>
    public SupplyType type;
}
/// <summary>
/// ������
/// </summary>
public class SupplyPoint : MonoBehaviour
{
    /// <summary>
    /// ��������
    /// </summary>
    public SupplyData data;

    Timer cdTimer;

    private void Start()
    {
        cdTimer = new Timer(3f, true);
    }

    private void Update()
    {
        if (!cdTimer.isTimeUp)
            return;

        cdTimer.Start();
        NewSupply();
    }

    private void NewSupply()
    {
        if (data.type == SupplyType.None)
            return;

        //Collider2D cols = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().bounds.size, .1f, LayerMask.GetMask("����Ͷ����"));
        //if (cols == null)
        //{
        //    PoolMgr.GetInstance().GetObj("Prefabs/" + data.type.ToString(), (x) =>
        //    {
        //        x.transform.position = transform.position;
        //    });
            Debug.Log(data.type);
        //}

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 255);
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().bounds.size);
    }
}
