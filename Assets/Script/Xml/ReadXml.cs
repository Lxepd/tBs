using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class ReadXml : InstanceNoMono<ReadXml>
{
    //private void Start()
    //{
    //    path = Application.persistentDataPath + "/DataXml.xml";
    //    SetFileToPersistent(path);
    //    LoadXml(path);
    //}

    /// <summary>
    /// 检查persistentDataPath路径有没有文件
    /// </summary>
    public void SetFileToPersistent(string path, string xmlPath)
    {
        FileInfo info = new FileInfo(Application.persistentDataPath + path);

        TextAsset ts = Resources.Load(xmlPath) as TextAsset;
        string content = ts.text;
        StreamWriter sw = info.CreateText();
        sw.Write(content);
        sw.Close();
        sw.Dispose();

    }
    /// <summary>
    /// 读取Xml
    /// </summary>
    public void LoadThrowItemXml(string path)
    {
        if (GameMgr.GetInstance().ThrowItemDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            ThrowItemData newData = new ThrowItemData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.icon = item.SelectSingleNode("icon").InnerText;
            newData.path = item.SelectSingleNode("path").InnerText;
            newData.name = item.SelectSingleNode("name").InnerText;
            newData.tips = item.SelectSingleNode("tips").InnerText;
            newData.mass = float.Parse(item.SelectSingleNode("mass").InnerText);
            newData.speed = float.Parse(item.SelectSingleNode("speed").InnerText);
            newData.hurt = float.Parse(item.SelectSingleNode("hurt").InnerText);
            newData.price = int.Parse(item.SelectSingleNode("price").InnerText);
            newData.canBuy = bool.Parse(item.SelectSingleNode("buy").InnerText);
            newData.canSell = bool.Parse(item.SelectSingleNode("sell").InnerText);

            GameMgr.GetInstance().ThrowItemDataDic.Add(newData.id, newData);
        }
    }
    public void LoadItemXml(string path)
    {
        if (GameMgr.GetInstance().ItemDataDic.Count != 0)
            return;


        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            ItemData newData = new ItemData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.name = item.SelectSingleNode("name").InnerText;
            newData.path = item.SelectSingleNode("path").InnerText;
            newData.tips = item.SelectSingleNode("tips").InnerText;
            newData.canSuperPosition = bool.Parse(item.SelectSingleNode("canSuperPosition").InnerText);
            newData.maxNum = int.Parse(item.SelectSingleNode("maxNum").InnerText);
            newData.canSell = bool.Parse(item.SelectSingleNode("canSell").InnerText);
            newData.sellPrice = int.Parse(item.SelectSingleNode("sellPrice").InnerText);
            newData.canBuy = bool.Parse(item.SelectSingleNode("canBuy").InnerText);
            newData.buyPrice = int.Parse(item.SelectSingleNode("buyPrice").InnerText);
            newData.itemType = (ItemType)int.Parse(item.SelectSingleNode("itemType").InnerText);
            newData.actType = (ItemActType)int.Parse(item.SelectSingleNode("actType").InnerText);

            GameMgr.GetInstance().ItemDataDic.Add(newData.id, newData);
        }
    }
    public void LoadNpcXml(string path)
    {
        if (GameMgr.GetInstance().NpcDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            NpcData newData = new NpcData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.name = item.SelectSingleNode("name").InnerText;

            GameMgr.GetInstance().NpcDataDic.Add(newData.id, newData);
        }
    }
    public void LoadRoomXml(string path)
    {
        if (GameMgr.GetInstance().RoomDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            RoomData newData = new RoomData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.name = item.SelectSingleNode("name").InnerText;
            newData.prefabPath = item.SelectSingleNode("path").InnerText;
            newData.roomType = (RoomType)int.Parse(item.SelectSingleNode("roomType").InnerText);

            GameMgr.GetInstance().RoomDataDic.Add(newData.id, newData);
        }
    }
    public void LoadPlayerXml(string path)
    {
        if (GameMgr.GetInstance().PlayerDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            PlayerData newData = new PlayerData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.name = item.SelectSingleNode("name").InnerText;
            newData.path = item.SelectSingleNode("path").InnerText;
            newData.tips = item.SelectSingleNode("tips").InnerText;
            newData.MaxHp = int.Parse(item.SelectSingleNode("maxHp").InnerText);
            newData.speed = float.Parse(item.SelectSingleNode("speed").InnerText);
            newData.bulletID = int.Parse(item.SelectSingleNode("bulletID").InnerText);
            newData.checkLen = float.Parse(item.SelectSingleNode("checkLen").InnerText);
            newData.shootLen = float.Parse(item.SelectSingleNode("shootLen").InnerText);

            GameMgr.GetInstance().PlayerDataDic.Add(newData.id, newData);
        }
    }
}
