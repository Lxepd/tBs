using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 补给类型
/// </summary>
public enum SupplyType
{
    None,
    罐子,
    砖头
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

    private void Start()
    {
        TimerAction.GetInstance().AddTimerActionDic("补给", 2f, NewSupply);
        

    }

    private void Update()
    {
        TimerAction.GetInstance().PlayerAction("补给");
    }

    private void NewSupply()
    {
        if (data.type == SupplyType.None)
            return;

        Debug.Log(data.type);
    }
}
