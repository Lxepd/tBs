using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum NpcType
{
    None,   
    道具商人,
    装备商人,
    工匠
}
[Serializable]
public class NpcSell
{
    public int id;
    public int num;
}
public class NpcData
{
    public int id; // id
    public string name; // 名
    public NpcType type; // Npc类型
}
