using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Datas : InstanceNoMono<Datas>
{
    // Sava //
    public bool isLoad; // �Ƿ����˶���
    public int CoinNum; // �����
    public int RoleId; // ��ɫid
    public int GunId; // ǹ֧id
    public int Hp; // Ѫ��
    // UnSave
    public PlayerData playerData; // ��ɫ���� 
    public WeaponData weaponData; // ǹ֧����
    public float itemAddAtkSpd; // �������ӵĹ���
    public float YWAddAtkSpd; // �������ӹ���
    public float YWAddRoleSpd; // �������ӽ�ɫ����
    public bool isEatItem; // �Ƿ�ʹ���˵���
    public float itemReShootTimer; // ���ߵĻָ�ʱ��
    public Dictionary<int, int> haveYWDic = new Dictionary<int, int>(); // ����������ֵ�<id������>

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
    // ���<����>��Ϣ�ֵ�
    public Dictionary<int, SkillData> SkillDataDic = new Dictionary<int, SkillData>();
    // ���<����>��Ϣ�ֵ�
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
    ����,
    ����,
    ��ת��
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