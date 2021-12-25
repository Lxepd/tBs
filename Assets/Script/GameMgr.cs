using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : InstanceNoMono<GameMgr>
{
    // 存放<射击物>信息字典
    public Dictionary<int, ThrowItemData> ThrowItemDataDic = new Dictionary<int, ThrowItemData>();
    // 存放<道具>信息字典
    public Dictionary<int, ItemData> ItemDataDic = new Dictionary<int, ItemData>();
    // 存放<装备>信息字典
    public Dictionary<int, EquipmentData> EquipmentDataDic = new Dictionary<int, EquipmentData>();
    // 存放<Npc>信息字典
    public Dictionary<int, NpcData> NpcDataDic = new Dictionary<int, NpcData>();
    // 存放<关卡>字典
    public Dictionary<int, RoomData> RoomDataDic = new Dictionary<int, RoomData>();

    /// <summary>
    /// 根据ID获取投掷物信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ThrowItemData GetThrowItemInfo(int id)
    {
        if (ThrowItemDataDic.ContainsKey(id))
            return ThrowItemDataDic[id];

        return null;
    }
    /// <summary>
    /// 根据ID获取道具信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ItemData GetItemInfo(int id)
    {
        if (ItemDataDic.ContainsKey(id))
            return ItemDataDic[id];

        return null;
    }
    /// <summary>
    /// 根据ID获取装备信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EquipmentData GetEquipmentInfo(int id)
    {
        if (EquipmentDataDic.ContainsKey(id))
            return EquipmentDataDic[id];

        return null;
    }
    /// <summary>
    /// 根据ID获取Npc信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public NpcData GetNpcInfo(int id)
    {
        if (NpcDataDic.ContainsKey(id))
            return NpcDataDic[id];

        return null;
    }
    /// <summary>
    /// 根据ID获取Room信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RoomData GetRoomInfo(int id)
    {
        if (RoomDataDic.ContainsKey(id))
            return RoomDataDic[id];

        return null;
    }
}
