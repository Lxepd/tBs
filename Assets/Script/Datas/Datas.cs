using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datas : InstanceNoMono<Datas>
{
    // Sava //
    public bool isLoad; // 是否点击了读档
    public int CoinNum; // 金币数
    public int RoleId; // 角色id
    public int GunId; // 枪支id
    public int Hp; // 血量
    // UnSave
    public PlayerData playerData; // 角色数据 
    public WeaponData weaponData; // 枪支数据
    public float itemAddAtkSpd; // 道具增加的攻速
    public float YWAddAtkSpd; // 遗物增加攻速
    public float YWAddRoleSpd; // 遗物增加角色移速
    public bool isEatItem; // 是否使用了道具
    public float itemReShootTimer; // 道具的恢复时间
    public Dictionary<int, int> haveYWDic = new Dictionary<int, int>(); // 带着遗物的字典<id，数量>

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
    // 存放<技能>信息字典
    public Dictionary<int, SkillData> SkillDataDic = new Dictionary<int, SkillData>();
    // 存放<遗物>信息字典
    public Dictionary<int, YWData> YWDataDic = new Dictionary<int, YWData>();

    public void GameReturnClear()
    {
        playerData = null;
        weaponData = null;
        itemAddAtkSpd = 0;
        YWAddAtkSpd = 0;
        YWAddRoleSpd = 0;
        isEatItem = false;
        itemReShootTimer = 0;
        haveYWDic.Clear();
    }
}
public enum YWType
{
    攻速,
    移速,
    移转攻
}
public class YWData
{
    public int id;
    public string name;
    public string imgPath;
    public string tips;
    public YWType type;
    public float effect;
}