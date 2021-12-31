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
    强化弹,
    质量弹,
    伤害弹,
    均衡弹
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

    Timer supplyTimer;
    Vector2 size;

    // 是否无限补给
    bool IsInfiniteSupply
    {
        get => data.supplyNum == -1;
    }

    private void Start()
    {
        supplyTimer = new Timer(3f, true, true);
        size = GetComponent<BoxCollider2D>().bounds.size;

    }

    private void Update()
    {
        Collider2D cols = Physics2D.OverlapBox(transform.position, size, .1f, LayerMask.GetMask("强化物"));
        if (cols != null)
        {
            supplyTimer.Continue();
        }

        if (supplyTimer.isTimeUp)
        {
            NewSupply();
        }

    }

    private void NewSupply()
    {
        if (!IsInfiniteSupply && data.num == data.supplyNum || data.type == SupplyType.None)
        {
            supplyTimer.End();
            return;
        }

        Collider2D cols = Physics2D.OverlapBox(transform.position, size, .1f, LayerMask.GetMask("强化物"));
        if (cols == null)
        {
            PoolMgr.GetInstance().GetObj("Prefabs/弹包", (x) =>
            {
                int bulletID = TypeFindBulletOfID();
                ResMgr.GetInstance().LoadAsync<Sprite>(GameMgr.GetInstance().GetThrowItemInfo(bulletID).icon, (y) =>
                 {
                     x.GetComponent<SpriteRenderer>().sprite = y;
                     x.GetComponent<BulletBag>().id = bulletID;
                 });
                x.transform.position = transform.position;
                data.num++;
            });

            supplyTimer.Pause();
        }
    }
    private int TypeFindBulletOfID()
    {
        int pathID = 0;

        switch (data.type)
        {
            case SupplyType.强化弹:
                pathID = 10002;
                break;
            case SupplyType.质量弹:
                pathID = 10003;
                break;
            case SupplyType.伤害弹:
                pathID = 10004;
                break;
            case SupplyType.均衡弹:
                pathID = 10005;
                break;
        }

        return pathID;
    }
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 255, 255);
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().bounds.size);
    }
#endif
}
