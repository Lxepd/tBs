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
    恢复,
    强化
}
public class ItemData
{
    public int id; // id
    public string name; // 名
    public string path; // 图片路径
    public string tips; // 说明
    public bool canSuperPosition; // 能否叠加
    public int maxNum; // 最大数量
    public bool canSell; // 能否出售
    public int sellPrice; // 出售价
    public bool canBuy; // 能否买入
    public int buyPrice; // 买入价
    public ItemType itemType; // 道具类型
    public ItemActType actType; // 道具功能

}
