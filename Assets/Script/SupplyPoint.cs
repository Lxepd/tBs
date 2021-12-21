using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// 补给类型
/// </summary>
public enum SupplyType
{
    None,
    罐子,
    砖头,
    玻璃,
    球
}
/// <summary>
/// 补给数据
/// </summary>
[Serializable]
public class SupplyData
{
    /// <summary>
    /// 补给数量
    /// </summary>
    public int supplyNum;
    /// <summary>
    /// 补给类型
    /// </summary>
    public SupplyType type;
    /// <summary>
    /// 当前已补给数量
    /// </summary>
    //[HideInInspector]
    public int num;
}
/// <summary>
/// 补给点
/// </summary>
public class SupplyPoint : MonoBehaviour
{
    /// <summary>
    /// 补给数据
    /// </summary>
    public SupplyData data;

    Timer cdTimer;
    Vector2 size;

    // 是否无限补给
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
        Collider2D cols = Physics2D.OverlapBox(transform.position, size, .1f, LayerMask.GetMask("场景投掷物"));
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

        Collider2D cols = Physics2D.OverlapBox(transform.position, size, .1f, LayerMask.GetMask("场景投掷物"));
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
