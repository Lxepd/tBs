using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIMgr.GetInstance().ShowPanel<MainPanel>("MainPanel", E_UI_Layer.Normal);
        ReadXml.GetInstance().ReadXmlFile(Application.persistentDataPath + "/DataXml.xml");
        //Debug.Log(ReadXml.GetInstance().ItemDataList.Count);
    }

}
