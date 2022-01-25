using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    一次性消耗品,
    可重复使用消耗品,
    非消耗品
}
public enum ItemActType
{
    None,
    血量恢复,
    攻速强化
}
public class ItemData
{
    public int id; // id
    public string name; // 名
    public string path; // 图片路径
    public string tips; // 说明
    public bool canSuperPosition; // 能否叠加
    public int maxNum; // 最大数量
    public int cost;
    public ItemType itemType; // 道具类型
    public ItemActType actType; // 道具功能
    public int recovery; // 恢复
    public float increaseAttackSpeed; // 增加攻速
}
