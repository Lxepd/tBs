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

    TimeAction timeAction = new TimeAction();

    private void Start()
    {
        timeAction.RegisterTimerTask("补给" + data.type.ToString(), 2f, NewSupply,true);
        timeAction.PlayTimerTask("补给" + data.type.ToString());
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
