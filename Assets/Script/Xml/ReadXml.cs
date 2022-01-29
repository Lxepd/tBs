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
            newData.cost = int.Parse(item.SelectSingleNode("cost").InnerText);
            newData.itemType = (ItemType)int.Parse(item.SelectSingleNode("itemType").InnerText);
            newData.actType = (ItemActType)int.Parse(item.SelectSingleNode("actType").InnerText);
            newData.recovery = int.Parse(item.SelectSingleNode("recovery").InnerText);
            newData.increaseAttackSpeed = float.Parse(item.SelectSingleNode("increaseAttackSpeed").InnerText);
            newData.reAtkSpeed = float.Parse(item.SelectSingleNode("reAtkSpeed").InnerText);

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
            newData.path = item.SelectSingleNode("path").InnerText;
            newData.type = (NpcType)int.Parse(item.SelectSingleNode("type").InnerText);
            newData.shopReTime = float.Parse(item.SelectSingleNode("shopReTime").InnerText);

            switch (newData.type)
            {
                case NpcType.道具商人:
                    foreach (XmlNode it in item.SelectSingleNode("shop").ChildNodes)
                    {
                        道具 shopItem = new 道具(int.Parse(it.Attributes["id"].InnerText), int.Parse(it.InnerText));

                        if(newData.items == null)
                        {
                            newData.items = new List<道具>();
                        }

                        newData.items.Add(shopItem);
                    }
                    break;
                case NpcType.装备商人:
                    break;
                case NpcType.工匠:
                    foreach (XmlNode it in item.SelectSingleNode("shop").ChildNodes)
                    {
                        升级 shopItem = new 升级(int.Parse(it.Attributes["id"].InnerText));

                        if (newData.upgrades == null)
                        {
                            newData.upgrades = new List<升级>();
                        }

                        newData.upgrades.Add(shopItem);
                    }
                    break;
            }

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
            newData.monsterNum = int.Parse(item.SelectSingleNode("num").InnerText);

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
            newData.spritePath = item.SelectSingleNode("sprite").InnerText;
            newData.tips = item.SelectSingleNode("tips").InnerText;
            newData.MaxHp = int.Parse(item.SelectSingleNode("maxHp").InnerText);
            newData.speed = float.Parse(item.SelectSingleNode("speed").InnerText);
            newData.initialWeaponId = int.Parse(item.SelectSingleNode("initialWeaponId").InnerText);
            newData.checkLen = float.Parse(item.SelectSingleNode("checkLen").InnerText);

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
            newData.type = (EnemyType)int.Parse(item.SelectSingleNode("type").InnerText);
            newData.path = item.SelectSingleNode("path").InnerText;
            newData.tips = item.SelectSingleNode("tips").InnerText;
            newData.rewardId = int.Parse(item.SelectSingleNode("rewardId").InnerText);
            newData.hp = float.Parse(item.SelectSingleNode("hp").InnerText);
            newData.speed = float.Parse(item.SelectSingleNode("speed").InnerText);
            newData.atk = float.Parse(item.SelectSingleNode("atk").InnerText);
            newData.hitLen = float.Parse(item.SelectSingleNode("hitLen").InnerText);
            newData.moveLen = float.Parse(item.SelectSingleNode("moveLen").InnerText);
            newData.atkLen = float.Parse(item.SelectSingleNode("atkLen").InnerText);

            Datas.GetInstance().EnemyDataDic.Add(newData.id, newData);
        }
    }
    public void LoadRewardXml(string path)
    {
        if (Datas.GetInstance().RewardDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            RewardData newData = new RewardData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.name = item.SelectSingleNode("name").InnerText;
            newData.path = item.SelectSingleNode("path").InnerText;
            newData.reward = int.Parse(item.SelectSingleNode("reward").InnerText);

            Datas.GetInstance().RewardDataDic.Add(newData.id, newData);
        }
    }
    public void LoadWeaponXml(string path)
    {
        if (Datas.GetInstance().WeaponDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            WeaponData newData = new WeaponData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.spritePath = item.SelectSingleNode("spritePath").InnerText;
            newData.bulletPath = item.SelectSingleNode("bulletPath").InnerText;
            newData.type = (Weapon)int.Parse(item.SelectSingleNode("type").InnerText);
            newData.bulletNum = int.Parse(item.SelectSingleNode("bulletNum").InnerText);
            newData.ammunitionChangeTime = float.Parse(item.SelectSingleNode("ammunitionChangeTime").InnerText);
            newData.atk = float.Parse(item.SelectSingleNode("atk").InnerText);
            newData.bulletSpeed = float.Parse(item.SelectSingleNode("bulletSpeed").InnerText);
            newData.shootLen = float.Parse(item.SelectSingleNode("shootLen").InnerText);
            newData.shootNextTime = float.Parse(item.SelectSingleNode("shootNextTime").InnerText);

            Datas.GetInstance().WeaponDataDic.Add(newData.id, newData);
        }
    }
    public void LoadUpgradeXml(string path)
    {
        if (Datas.GetInstance().UpgradeDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            UpgradeData newData = new UpgradeData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.beforeId = int.Parse(item.SelectSingleNode("beforeUpgrade").InnerText);
            newData.afterId = int.Parse(item.SelectSingleNode("afterUpgrade").InnerText);
            newData.cost = int.Parse(item.SelectSingleNode("cost").InnerText);

            Datas.GetInstance().UpgradeDataDic.Add(newData.id, newData);
        }
    }
    public void LoadSkillXml(string path)
    {
        if (Datas.GetInstance().SkillDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            SkillData newData = new SkillData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.name = item.SelectSingleNode("name").InnerText;
            newData.path = item.SelectSingleNode("path").InnerText;
            newData.tips = item.SelectSingleNode("tips").InnerText;
            newData.speed = int.Parse(item.SelectSingleNode("speed").InnerText);
            newData.hurt = int.Parse(item.SelectSingleNode("hurt").InnerText);

            Datas.GetInstance().SkillDataDic.Add(newData.id, newData);
        }
    }
    public void LoadYWXml(string path)
    {
        if (Datas.GetInstance().YWDataDic.Count != 0)
            return;

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.persistentDataPath + path);

        XmlNodeList xmlList = xmlDoc.SelectSingleNode("Data").ChildNodes;

        foreach (XmlNode item in xmlList)
        {
            YWData newData = new YWData();
            newData.id = int.Parse(item.Attributes["id"].InnerText);

            newData.name = item.SelectSingleNode("name").InnerText;
            newData.imgPath = item.SelectSingleNode("imgPath").InnerText;
            newData.tips = item.SelectSingleNode("tip").InnerText;
            newData.type = (YWType)int.Parse(item.SelectSingleNode("ywType").InnerText);
            newData.effect = float.Parse(item.SelectSingleNode("effect").InnerText);

            Datas.GetInstance().YWDataDic.Add(newData.id, newData);
        }
    }
    #endregion
}
