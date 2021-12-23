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
        Debug.Log(GameMgr.GetInstance().ThrowItemDataDic.Count);

        ReadXml.GetInstance().SetFileToPersistent("/ItemXml.xml", "Xml/ItemXml");
        ReadXml.GetInstance().LoadItemXml("/ItemXml.xml");
        Debug.Log(GameMgr.GetInstance().ItemDataDic.Count);

        ReadXml.GetInstance().SetFileToPersistent("/NpcXml.xml", "Xml/NpcXml");
        ReadXml.GetInstance().LoadNpcXml("/NpcXml.xml");
        Debug.Log(GameMgr.GetInstance().NpcDataDic.Count);
    }
}
