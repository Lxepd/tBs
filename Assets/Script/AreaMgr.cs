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
    /// <summary>
    /// 普通
    /// </summary>
    Normal,
    /// <summary>
    /// boss
    /// </summary>
    Boss
}
/// <summary>
/// 房间大小
/// </summary>
[Serializable]
public class RoomSize
{
    /// <summary>
    /// 长
    /// </summary>
    public float length;
    /// <summary>
    /// 宽
    /// </summary>
    public float width;
}
/// <summary>
/// 房间数据
/// </summary>
[Serializable]
public class RoomData
{
    /// <summary>
    /// 房间id
    /// </summary>
    public int id;
    /// <summary>
    /// 房间类型
    /// </summary>
    public RoomType roomType;
    /// <summary>
    /// 房间大小
    /// </summary>
    public RoomSize size;
    /// <summary>
    /// 房间内怪物表
    /// </summary>
    public List<GameObject> monsters;
}
public class AreaMgr : MonoBehaviour
{
    public List<GameObject> roomPrefabs = new List<GameObject>();
    public List<RoomData> roomData = new List<RoomData>();
    public Dictionary<Vector3, GameObject> rooms = new Dictionary<Vector3, GameObject>();

    public int roomNum;
    public bool aaa;
    public void Start()
    {

    }

    public void InitRoom()
    {
   
    }
    public bool CheckRoomPos(Vector3 pos)
    {
        return false;
    }

}

