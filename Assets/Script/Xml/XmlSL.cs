using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using UnityEditor;
using System.IO;

public class Save
{
    public int RoleId;
    public int GunId;
    public int CoinNum;
    public int Hp;
}

public class XmlSL : InstanceNoMono<XmlSL>
{
    string path=
#if UNITY_EDITOR
    Application.dataPath + "/Resources/Xml/SaveXml.xml";
#elif UNITY_ANDROID
    Application.persistentDataPath + @"/SaveXml.xml";
#endif

    private Save CreateGoMsg()
    {
        Save save = new Save();
        save.CoinNum = Datas.GetInstance().CoinNum;
        save.RoleId = Datas.GetInstance().RoleId;
        save.GunId = Datas.GetInstance().GunId;
        save.Hp = Datas.GetInstance().Hp;

        return save;
    }
    public void Save()
    {
        Save save = CreateGoMsg();

        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("save");

        XmlElement file = xmlDoc.CreateElement("file");
        
        XmlElement coinNum = xmlDoc.CreateElement("coinNum");
        coinNum.InnerText = save.CoinNum.ToString();
        XmlElement role = xmlDoc.CreateElement("role");
        role.InnerText = save.RoleId.ToString();
        XmlElement gun = xmlDoc.CreateElement("gun");
        gun.InnerText = save.GunId.ToString();
        XmlElement hp = xmlDoc.CreateElement("hp");
        hp.InnerText = save.Hp.ToString();

        //XmlElement ywDic = xmlDoc.CreateElement("yw");

        //foreach (var item in Datas.GetInstance().haveYWDic)
        //{
        //    XmlElement saveYW = xmlDoc.CreateElement("saveYW");
        //    saveYW.SetAttribute("id ", item.Key.ToString());
        //    saveYW.InnerText = item.Value.ToString();

        //    ywDic.AppendChild(saveYW);
        //}

        file.AppendChild(role);
        file.AppendChild(gun);
        file.AppendChild(coinNum);
        file.AppendChild(hp);
        //file.AppendChild(ywDic);
        root.AppendChild(file);

        xmlDoc.AppendChild(root);
        xmlDoc.Save(path);

#if UNITY_EDITOR
        AssetDatabase.Refresh();
#endif
    }
    public GameLoadEnum Load()
    {
        Save save = new Save();

        if (File.Exists(path))
        {
            XmlDocument xmlDoc = new XmlDocument(); ;
            xmlDoc.Load(path);

            XmlNodeList xmlList = xmlDoc.SelectSingleNode("save").ChildNodes;

            if (xmlList.Count != 0)
            {
                foreach (XmlNode item in xmlList)
                {
                    save.RoleId = int.Parse(item.SelectSingleNode("role").InnerText);
                    save.GunId = int.Parse(item.SelectSingleNode("gun").InnerText);
                    save.CoinNum = int.Parse(item.SelectSingleNode("coinNum").InnerText);
                }
            }

            SetGame(save);
            return GameLoadEnum.Ok;
        }
        else
        {
            return GameLoadEnum.Null;
        }
    }
    public void SetGame(Save s)
    {
        Datas.GetInstance().RoleId = s.RoleId;
        Datas.GetInstance().GunId = s.GunId;
        Datas.GetInstance().CoinNum = s.CoinNum;
    }
}
