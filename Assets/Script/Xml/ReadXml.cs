using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class ReadXml : InstanceNoMono<ReadXml>
{
    private Dictionary<int, ThrowItemData> itemDataDic = new Dictionary<int, ThrowItemData>();

    //private void Start()
    //{
    //    path = Application.persistentDataPath + "/DataXml.xml";
    //    SetFileToPersistent(path);
    //    LoadXml(path);
    //}
    public void ReadXmlFile(string path)
    {
        SetFileToPersistent(path);
        LoadXml(path);
    }
    /// <summary>
    /// 检查persistentDataPath路径有没有文件
    /// </summary>
    private void SetFileToPersistent(string path)
    {
        FileInfo info = new FileInfo(path);
        if (!info.Exists)
        {
            TextAsset ts = Resources.Load("Xml/DataXml") as TextAsset;
            string content = ts.text;
            StreamWriter sw = info.CreateText();
            sw.Write(content);
            sw.Close();
            sw.Dispose();
        }
    }
    /// <summary>
    /// 读取Xml
    /// </summary>
    private void LoadXml(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            ThrowItemData newData = new ThrowItemData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.path = item.SelectSingleNode("path").InnerText;
            newData.name = item.SelectSingleNode("name").InnerText;
            newData.tips = item.SelectSingleNode("tips").InnerText;
            newData.mass = float.Parse(item.SelectSingleNode("mass").InnerText);
            newData.hurt = float.Parse(item.SelectSingleNode("hurt").InnerText);
            newData.price = int.Parse(item.SelectSingleNode("price").InnerText);
            newData.canBuy = bool.Parse(item.SelectSingleNode("buy").InnerText);
            newData.canSell = bool.Parse(item.SelectSingleNode("sell").InnerText);

            itemDataDic.Add(newData.id, newData);
        }
    }
    /// <summary>
    /// 根据ID获取
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ThrowItemData GetThrowItemInfo(int id)
    {
        if (itemDataDic.ContainsKey(id))
            return itemDataDic[id];

        return null;
    }
}
