using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : InstanceNoMono<GameMgr>
{
    // ���Ͷ������Ϣ�ֵ�
    private Dictionary<int, ThrowItemData> throwItemDataDic = new Dictionary<int, ThrowItemData>();
    // ��ŵ�����Ϣ�ֵ�
    private Dictionary<int, ItemData> itemDataDic = new Dictionary<int, ItemData>();
    // ���װ����Ϣ�ֵ�
    private Dictionary<int, EquipmentData> equipmentDataDic = new Dictionary<int, EquipmentData>();
    // ���Npc��Ϣ�ֵ�
    private Dictionary<int, NpcData> npcDataDic = new Dictionary<int, NpcData>();

    public Dictionary<int, ThrowItemData> ThrowItemDataDic { get => throwItemDataDic; set => throwItemDataDic = value; }
    public Dictionary<int, ItemData> ItemDataDic { get => itemDataDic; set => itemDataDic = value; }
    public Dictionary<int, EquipmentData> EquipmentDataDic { get => equipmentDataDic; set => equipmentDataDic = value; }
    public Dictionary<int, NpcData> NpcDataDic { get => npcDataDic; set => npcDataDic = value; }

    /// <summary>
    /// ����ID��ȡͶ������Ϣ
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
    /// ����ID��ȡ������Ϣ
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
    /// ����ID��ȡװ����Ϣ
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
    /// ����ID��ȡNpc��Ϣ
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
