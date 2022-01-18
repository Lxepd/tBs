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

        //Debug.Log("Ͷ�����ֵ�����������    " + GameMgr.GetInstance().ThrowItemDataDic.Count);
        //Debug.Log("�����ֵ�����������    " + GameMgr.GetInstance().ItemDataDic.Count);
        //Debug.Log("Npc�ֵ�����������    " + GameMgr.GetInstance().NpcDataDic.Count);
        //Debug.Log("�����ֵ�����������    " + GameMgr.GetInstance().RoomDataDic.Count);
        //Debug.Log("��ɫ�ֵ�����������    " + GameMgr.GetInstance().PlayerDataDic.Count);
        //Debug.Log("�����ֵ�����������    " + GameMgr.GetInstance().EnemyDataDic.Count);
        //Debug.Log("�����ֵ�����������    " + GameMgr.GetInstance().RewardDataDic.Count);
        //Debug.Log("ǹ֧�ֵ�����������    " + GameMgr.GetInstance().WeaponDataDic.Count);
        //Debug.Log("ǹ֧�����ֵ�����������    " + GameMgr.GetInstance().UpgradeDataDic.Count);
        //Debug.Log("�����ֵ�����������    " + GameMgr.GetInstance().SkillDataDic.Count);
    }
}
