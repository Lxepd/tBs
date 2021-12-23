using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    None,
    武器,
    防具
}
public class EquipmentData
{
    public int id; // id
    public string name; // 名
    public string tips; // 说明
    public EquipmentType type; // 装备类型
    public float atk; // 攻击力
}
