using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NpcData
{
    public int id; // id
    public string name; // 名
}

public enum NpcType
{
    None,
    道具商人,
    装备商人,
    工匠
}
[Serializable]
public class 道具商人
{
    // 道具ID
    public int id;
    // 道具数量
    public int num;

    public 道具商人(int id, int num)
    {
        this.id = id;
        this.num = num;
    }
}
[Serializable]
public class 装备商人
{
    public int test;

}
[Serializable]
public class 工匠
{
    public int test;
}