using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;

public class MainPanel : UIBase
{
    List<ThrowItemData> itemDataList = new List<ThrowItemData>();
    public Text text;

    private void Update()
    {
        text.text = itemDataList.Count.ToString();
    }
    public override void ShowMe()
    {
        string filePath = Application.streamingAssetsPath + "/DataXml.xml";

        if (File.Exists(filePath))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

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

                itemDataList.Add(newData);

            }
        }

    }
    protected override void OnClick(string btnName)
    {
        switch(btnName)
        {
            case "Bto_Play":
                UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Above);
                SceneMgr.GetInstance().LoadSceneAsyn("Game", () => 
                {
                    UIMgr.GetInstance().HidePanel("MainPanel");
                    UIMgr.GetInstance().HidePanel("LoadingPanel");
                    UIMgr.GetInstance().ShowPanel<JoyStickPanel>("JoyStickPanel", E_UI_Layer.Normal);
                    UIMgr.GetInstance().ShowPanel<ControlPanel>("ControlPanel", E_UI_Layer.Normal);
                });
                break;
        }
    }

}
