using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIMgr.GetInstance().ShowPanel<LogoPanel>("LogoPanel", E_UI_Layer.Logo);

        ReadXml.GetInstance().SetFileToPersistent("/ThrowItemXml.xml", "Xml/ThrowItemXml");
        ReadXml.GetInstance().LoadThrowItemXml("/ThrowItemXml.xml");

        ReadXml.GetInstance().SetFileToPersistent("/ItemXml.xml", "Xml/ItemXml");
        ReadXml.GetInstance().LoadItemXml("/ItemXml.xml");

        ReadXml.GetInstance().SetFileToPersistent("/NpcXml.xml", "Xml/NpcXml");
        ReadXml.GetInstance().LoadNpcXml("/NpcXml.xml");

        ReadXml.GetInstance().SetFileToPersistent("/RoomXml.xml", "Xml/RoomXml");
        ReadXml.GetInstance().LoadRoomXml("/RoomXml.xml");

        ReadXml.GetInstance().SetFileToPersistent("/PlayerXml.xml", "Xml/PlayerXml");
        ReadXml.GetInstance().LoadPlayerXml("/PlayerXml.xml");

        ReadXml.GetInstance().SetFileToPersistent("/EnemyXml.xml", "Xml/EnemyXml");
        ReadXml.GetInstance().LoadEnemyXml("/EnemyXml.xml");

        ReadXml.GetInstance().SetFileToPersistent("/RewardXml.xml", "Xml/RewardXml");
        ReadXml.GetInstance().LoadRewardXml("/RewardXml.xml");

        ReadXml.GetInstance().SetFileToPersistent("/WeaponXml.xml", "Xml/WeaponXml");
        ReadXml.GetInstance().LoadWeaponXml("/WeaponXml.xml");

        ReadXml.GetInstance().SetFileToPersistent("/UpgradeXml.xml", "Xml/UpgradeXml");
        ReadXml.GetInstance().LoadUpgradeXml("/UpgradeXml.xml");

        ReadXml.GetInstance().SetFileToPersistent("/SkillXml.xml", "Xml/SkillXml");
        ReadXml.GetInstance().LoadSkillXml("/SkillXml.xml");

        ReadXml.GetInstance().SetFileToPersistent("/YWXml.xml", "Xml/YWXml");
        ReadXml.GetInstance().LoadYWXml("/YWXml.xml");
    }
}
