using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum NpcType
{
    None,   
    ��������,
    װ������,
    ����
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
    public string name; // ��
    public NpcType type; // Npc����
}
