using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datas : InstanceNoMono<Datas>
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
    // ���<���>��Ϣ�ֵ�
    public Dictionary<int, PlayerData> PlayerDataDic = new Dictionary<int, PlayerData>();
    // ���<����>��Ϣ�ֵ�
    public Dictionary<int, EnemyData> EnemyDataDic = new Dictionary<int, EnemyData>();
    // ���<����>��Ϣ�ֵ�
    public Dictionary<int, RewardData> RewardDataDic = new Dictionary<int, RewardData>();
    // ���<ǹ֧>��Ϣ�ֵ�
    public Dictionary<int, WeaponData> WeaponDataDic = new Dictionary<int, WeaponData>();
    // ���<����>��Ϣ�ֵ�
    public Dictionary<int, UpgradeData> UpgradeDataDic = new Dictionary<int, UpgradeData>();
}
