using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NpcData
{
    public int id; // id
    public string name; // ��
    public string path;
    public NpcType type;
    public List<����> items;
    public float shopReTime;
    public List<����> upgrades;
}

public enum NpcType
{
    None,
    ��������,
    װ������,
    ����
}
[Serializable]
public class ����
{
    // ����ID
    public int id;
    // ��������
    public int num;

    public ����(int id, int num)
    {
        this.id = id;
        this.num = num;
    }
}
public class UpgradeData
{
    public int id; // id
    public int beforeId; // ����ǰ
    public int afterId; // ������
    public int cost; // ����
}
[Serializable]
public class ����
{
    public int id; // id
    public int beforeId; // ����ǰ
    public int afterId; // ������
    public int cost; // ����

    public ����(int id)
    {
        this.id = id;
    }
}