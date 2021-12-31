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
    ǿ����,
    ������,
    �˺���,
    ���ⵯ
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

    Timer supplyTimer;
    Vector2 size;

    // �Ƿ����޲���
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
        Collider2D cols = Physics2D.OverlapBox(transform.position, size, .1f, LayerMask.GetMask("ǿ����"));
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

        Collider2D cols = Physics2D.OverlapBox(transform.position, size, .1f, LayerMask.GetMask("ǿ����"));
        if (cols == null)
        {
            PoolMgr.GetInstance().GetObj("Prefabs/����", (x) =>
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
            case SupplyType.ǿ����:
                pathID = 10002;
                break;
            case SupplyType.������:
                pathID = 10003;
                break;
            case SupplyType.�˺���:
                pathID = 10004;
                break;
            case SupplyType.���ⵯ:
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
