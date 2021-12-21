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
    /// <summary>
    /// ��ǰ�Ѳ�������
    /// </summary>
    //[HideInInspector]
    public int num;
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
    Vector2 size;

    // �Ƿ����޲���
    bool IsInfiniteSupply
    {
        get => data.supplyNum == -1;
    }


    private void Start()
    {
        cdTimer = new Timer(1f, true, true);
        size = GetComponent<BoxCollider2D>().bounds.size;

    }

    private void Update()
    {
        Collider2D cols = Physics2D.OverlapBox(transform.position, size, .1f, LayerMask.GetMask("����Ͷ����"));
        if (cols != null)
        {
            cdTimer.Continue();
        }

        if (cdTimer.isTimeUp)
        {
            NewSupply();
        }

    }

    private void NewSupply()
    {
        if (!IsInfiniteSupply && data.num == data.supplyNum || data.type == SupplyType.None)
        {
            cdTimer.End();
            return;
        }

        Collider2D cols = Physics2D.OverlapBox(transform.position, size, .1f, LayerMask.GetMask("����Ͷ����"));
        if (cols == null)
        {
            PoolMgr.GetInstance().GetObj("Prefabs/" + data.type.ToString(), (x) =>
            {
                x.transform.position = transform.position;
                data.num++;
            });

            cdTimer.Pause();
        }
    }
    private void CreateSupply()
    {
        PoolMgr.GetInstance().GetObj("Prefabs/" + data.type.ToString(), (x) =>
        {
            x.transform.position = transform.position;
            data.num++;
        });
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 255);
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().bounds.size);
    }
}
