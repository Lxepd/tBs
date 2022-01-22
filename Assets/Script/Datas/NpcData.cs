using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NpcData
{
    public int id; // id
    public string name; // 名
    public string path;
    public NpcType type;
    public List<道具> items;
    public float shopReTime;
    public List<升级> upgrades;
}

public enum NpcType
{
    None,
    道具商人,
    装备商人,
    工匠
}
[Serializable]
public class 道具
{
    // 道具ID
    public int id;
    // 道具数量
    public int num;

    public 道具(int id, int num)
    {
        this.id = id;
        this.num = num;
    }
}
public class UpgradeData
{
    public int id; // id
    public int beforeId; // 升级前
    public int afterId; // 升级后
    public int cost; // 花费
}
[Serializable]
public class 升级
{
    public int id; // id
    public int beforeId; // 升级前
    public int afterId; // 升级后
    public int cost; // 花费

    public 升级(int id)
    {
        this.id = id;
    }
}