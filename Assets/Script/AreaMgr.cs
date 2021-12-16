using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// ��������
/// </summary>
[Serializable]
public enum RoomType
{
    /// <summary>
    /// ��ͨ
    /// </summary>
    Normal,
    /// <summary>
    /// boss
    /// </summary>
    Boss
}
/// <summary>
/// �����С
/// </summary>
[Serializable]
public class RoomSize
{
    /// <summary>
    /// ��
    /// </summary>
    public float length;
    /// <summary>
    /// ��
    /// </summary>
    public float width;
}
/// <summary>
/// ��������
/// </summary>
[Serializable]
public class RoomData
{
    /// <summary>
    /// ����id
    /// </summary>
    public int id;
    /// <summary>
    /// ��������
    /// </summary>
    public RoomType roomType;
    /// <summary>
    /// �����С
    /// </summary>
    public RoomSize size;
    /// <summary>
    /// �����ڹ����
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

