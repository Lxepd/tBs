using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    TimeAction timeAction = new TimeAction();

    private void Start()
    {
        timeAction.RegisterTimerTask("����" + data.type.ToString(), 2f, NewSupply,true);
        timeAction.PlayTimerTask("����" + data.type.ToString());
        //if (timeAction == null)
        //    timeAction = new TimeAction();
        //Debug.Log(timeAction);
    }

    private void Update()
    {
        timeAction.OnUpdate();
    }

    private void NewSupply()
    {
        if (data.type == SupplyType.None)
            return;

        Debug.Log(data.type);
    }
}
