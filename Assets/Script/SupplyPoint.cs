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
    שͷ
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

    private void Start()
    {
        TimerAction.GetInstance().AddTimerActionDic("����", 2f, NewSupply);
        

    }

    private void Update()
    {
        TimerAction.GetInstance().PlayerAction("����");
    }

    private void NewSupply()
    {
        if (data.type == SupplyType.None)
            return;

        Debug.Log(data.type);
    }
}
