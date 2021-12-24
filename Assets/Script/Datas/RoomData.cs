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
    Normal,     // ��ͨ
    Boss     // boss
}
/// <summary>
/// ��������
/// </summary>
[Serializable]
public class RoomData
{
    public int id;     // ����id
    public string name; // ��
    public string prefabPath;     // Ԥ����λ��
    public RoomType roomType;     // ��������
    public List<GameObject> monsters;     // �����ڹ����
}