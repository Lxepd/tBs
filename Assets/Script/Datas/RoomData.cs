using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 房间类型
/// </summary>
[Serializable]
public enum RoomType
{
    Normal,     // 普通
    Boss     // boss
}
/// <summary>
/// 房间数据
/// </summary>
[Serializable]
public class RoomData
{
    public int id;     // 房间id
    public string name; // 名
    public string prefabPath;     // 预制体位置
    public RoomType roomType;     // 房间类型
    public List<GameObject> monsters;     // 房间内怪物表
}