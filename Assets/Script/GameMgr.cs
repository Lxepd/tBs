using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : InstanceNoMono<GameMgr>
{
    // 存放投掷物信息字典
    private Dictionary<int, ThrowItemData> throwItemDataDic = new Dictionary<int, ThrowItemData>();
    // 存放道具信息字典
    private Dictionary<int, ItemData> itemDataDic = new Dictionary<int, ItemData>();
    // 存放装备信息字典
    private Dictionary<int, EquipmentData> equipmentDataDic = new Dictionary<int, EquipmentData>();
    // 存放Npc信息字典
    private Dictionary<int, NpcData> npcDataDic = new Dictionary<int, NpcData>();

    public Dictionary<int, ThrowItemData> ThrowItemDataDic { get => throwItemDataDic; set => throwItemDataDic = value; }
    public Dictionary<int, ItemData> ItemDataDic { get => itemDataDic; set => itemDataDic = value; }
    public Dictionary<int, EquipmentData> EquipmentDataDic { get => equipmentDataDic; set => equipmentDataDic = value; }
    public Dictionary<int, NpcData> NpcDataDic { get => npcDataDic; set => npcDataDic = value; }

    /// <summary>
    /// 根据ID获取投掷物信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ThrowItemData GetThrowItemInfo(int id)
    {
        if (throwItemDataDic.ContainsKey(id))
            return throwItemDataDic[id];

        return null;
    }
    /// <summary>
    /// 根据ID获取道具信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ItemData GetItemInfo(int id)
    {
        if (itemDataDic.ContainsKey(id))
            return itemDataDic[id];

        return null;
    }
    /// <summary>
    /// 根据ID获取装备信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EquipmentData GetEquipmentInfo(int id)
    {
        if (equipmentDataDic.ContainsKey(id))
            return equipmentDataDic[id];

        return null;
    }
    /// <summary>
    /// 根据ID获取Npc信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public NpcData GetNpcInfo(int id)
    {
        if (npcDataDic.ContainsKey(id))
            return npcDataDic[id];

        return null;
    }
}
