using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : InstanceNoMono<GameMgr>
{
    // ���<�����>��Ϣ�ֵ�
    public Dictionary<int, ThrowItemData> ThrowItemDataDic = new Dictionary<int, ThrowItemData>();
    // ���<����>��Ϣ�ֵ�
    public Dictionary<int, ItemData> ItemDataDic = new Dictionary<int, ItemData>();
    // ���<װ��>��Ϣ�ֵ�
    public Dictionary<int, EquipmentData> EquipmentDataDic = new Dictionary<int, EquipmentData>();
    // ���<Npc>��Ϣ�ֵ�
    public Dictionary<int, NpcData> NpcDataDic = new Dictionary<int, NpcData>();
    // ���<�ؿ�>�ֵ�
    public Dictionary<int, RoomData> RoomDataDic = new Dictionary<int, RoomData>();

    /// <summary>
    /// ����ID��ȡͶ������Ϣ
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
    /// ����ID��ȡ������Ϣ
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
    /// ����ID��ȡװ����Ϣ
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
    /// ����ID��ȡNpc��Ϣ
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
    /// ����ID��ȡRoom��Ϣ
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
