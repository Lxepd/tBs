using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NpcData
{
    public int id; // id
    public string name; // ��
}

public enum NpcType
{
    None,
    ��������,
    װ������,
    ����
}
[Serializable]
public class ��������
{
    // ����ID
    public int id;
    // ��������
    public int num;

    public ��������(int id, int num)
    {
        this.id = id;
        this.num = num;
    }
}
[Serializable]
public class װ������
{
    public int test;

}
[Serializable]
public class ����
{
    public int test;
}