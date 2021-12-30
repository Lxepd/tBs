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
        Debug.Log("投掷物字典数据数量：    " + GameMgr.GetInstance().ThrowItemDataDic.Count);

        ReadXml.GetInstance().SetFileToPersistent("/ItemXml.xml", "Xml/ItemXml");
        ReadXml.GetInstance().LoadItemXml("/ItemXml.xml");
        Debug.Log("道具字典数据数量：    " + GameMgr.GetInstance().ItemDataDic.Count);

        ReadXml.GetInstance().SetFileToPersistent("/NpcXml.xml", "Xml/NpcXml");
        ReadXml.GetInstance().LoadNpcXml("/NpcXml.xml");
        Debug.Log("Npc字典数据数量：    " + GameMgr.GetInstance().NpcDataDic.Count);

        ReadXml.GetInstance().SetFileToPersistent("/RoomXml.xml", "Xml/RoomXml");
        ReadXml.GetInstance().LoadRoomXml("/RoomXml.xml");
        Debug.Log("房间字典数据数量：    " + GameMgr.GetInstance().RoomDataDic.Count);

        ReadXml.GetInstance().SetFileToPersistent("/PlayerXml.xml", "Xml/PlayerXml");
        ReadXml.GetInstance().LoadPlayerXml("/PlayerXml.xml");
        Debug.Log("角色字典数据数量：    " + GameMgr.GetInstance().PlayerDataDic.Count);


    }
}
