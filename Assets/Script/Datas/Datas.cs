using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datas : InstanceNoMono<Datas>
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
    // 存放<玩家>信息字典
    public Dictionary<int, PlayerData> PlayerDataDic = new Dictionary<int, PlayerData>();
    // 存放<怪物>信息字典
    public Dictionary<int, EnemyData> EnemyDataDic = new Dictionary<int, EnemyData>();
    // 存放<奖励>信息字典
    public Dictionary<int, RewardData> RewardDataDic = new Dictionary<int, RewardData>();
    // 存放<枪支>信息字典
    public Dictionary<int, WeaponData> WeaponDataDic = new Dictionary<int, WeaponData>();
    // 存放<升级>信息字典
    public Dictionary<int, UpgradeData> UpgradeDataDic = new Dictionary<int, UpgradeData>();
}
