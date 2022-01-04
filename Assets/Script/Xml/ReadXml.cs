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
        if (Datas.GetInstance().ThrowItemDataDic.Count != 0)
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

            Datas.GetInstance().ThrowItemDataDic.Add(newData.id, newData);
        }
    }

    #region 数据读取
    public void LoadItemXml(string path)
    {
        if (Datas.GetInstance().ItemDataDic.Count != 0)
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

            Datas.GetInstance().ItemDataDic.Add(newData.id, newData);
        }
    }
    public void LoadNpcXml(string path)
    {
        if (Datas.GetInstance().NpcDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            NpcData newData = new NpcData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.name = item.SelectSingleNode("name").InnerText;

            Datas.GetInstance().NpcDataDic.Add(newData.id, newData);
        }
    }
    public void LoadRoomXml(string path)
    {
        if (Datas.GetInstance().RoomDataDic.Count != 0)
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

            Datas.GetInstance().RoomDataDic.Add(newData.id, newData);
        }
    }
    public void LoadPlayerXml(string path)
    {
        if (Datas.GetInstance().PlayerDataDic.Count != 0)
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

            Datas.GetInstance().PlayerDataDic.Add(newData.id, newData);
        }
    }
    public void LoadEnemyXml(string path)
    {
        if (Datas.GetInstance().EnemyDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            EnemyData newData = new EnemyData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.name = item.SelectSingleNode("name").InnerText;
            newData.path = item.SelectSingleNode("path").InnerText;
            newData.tips = item.SelectSingleNode("tips").InnerText;
            newData.hp = float.Parse(item.SelectSingleNode("hp").InnerText);
            newData.speed = float.Parse(item.SelectSingleNode("speed").InnerText);
            newData.atk = float.Parse(item.SelectSingleNode("atk").InnerText);
            newData.hitLen = float.Parse(item.SelectSingleNode("hitLen").InnerText);
            newData.moveLen = float.Parse(item.SelectSingleNode("moveLen").InnerText);
            newData.atkLen = float.Parse(item.SelectSingleNode("atkLen").InnerText);

            Datas.GetInstance().EnemyDataDic.Add(newData.id, newData);
        }
    }
    #endregion
}
