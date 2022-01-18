using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIMgr.GetInstance().ShowPanel<MainPanel>("MainPanel", E_UI_Layer.Normal);

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

        //Debug.Log("投掷物字典数据数量：    " + GameMgr.GetInstance().ThrowItemDataDic.Count);
        //Debug.Log("道具字典数据数量：    " + GameMgr.GetInstance().ItemDataDic.Count);
        //Debug.Log("Npc字典数据数量：    " + GameMgr.GetInstance().NpcDataDic.Count);
        //Debug.Log("房间字典数据数量：    " + GameMgr.GetInstance().RoomDataDic.Count);
        //Debug.Log("角色字典数据数量：    " + GameMgr.GetInstance().PlayerDataDic.Count);
        //Debug.Log("怪物字典数据数量：    " + GameMgr.GetInstance().EnemyDataDic.Count);
        //Debug.Log("奖励字典数据数量：    " + GameMgr.GetInstance().RewardDataDic.Count);
        //Debug.Log("枪支字典数据数量：    " + GameMgr.GetInstance().WeaponDataDic.Count);
        //Debug.Log("枪支升级字典数据数量：    " + GameMgr.GetInstance().UpgradeDataDic.Count);
        //Debug.Log("技能字典数据数量：    " + GameMgr.GetInstance().SkillDataDic.Count);
    }
}
