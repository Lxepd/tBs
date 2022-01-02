using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 怪物数据
/// </summary>
public class EnemyData
{
    public int id; // ID
    public string name; // 名
    public string path; // 预制体位置
    public string tips; // 说明
    public float hp; // 血量
    public float speed;
    public float atk; // 伤害
    public float hitLen; //受击检测范围
    public float moveLen; // 移动检测范围
    public float atkLen; // 攻击检测范围
}
