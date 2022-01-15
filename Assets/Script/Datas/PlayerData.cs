using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 玩家数据
/// </summary>
public class PlayerData
{
    public int id; // id
    public string name; // 角色名
    public string path; // 预制体位置
    public string spritePath;
    public string tips; // 角色说明
    public int MaxHp; // 最大生命值
    public float speed; // 角色速度
    public int bulletID; // 初始子弹
    public float checkLen; // 道具检测范围
    public float shootLen; // 允许射击范围
}
