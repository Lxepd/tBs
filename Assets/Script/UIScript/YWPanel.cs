using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YWPanel : UIBase
{
    Transform parent;
    Transform showParent;

    int switchYW;

    private void Start()
    {
        parent = GameTool.FindTheChild(gameObject, "parent");
        showParent = GameTool.FindTheChild(gameObject, "YWShowParent");
    }
    public override void ShowMe()
    {
        Time.timeScale = 0;

        if(Datas.GetInstance().haveYWDic.Count!=0)
        {
            foreach (var item in Datas.GetInstance().haveYWDic)
            {
                PoolMgr.GetInstance().GetObj("Prefabs/YWShow", (x) =>
                {
                    x.transform.SetParent(showParent);
                    x.transform.localScale = Vector3.one;

                    x.GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(Datas.GetInstance().YWDataDic[item.Key].imgPath);

                    if (item.Value > 1)
                        GameTool.FindTheChild(x, "showText").GetComponent<Text>().text = item.Value.ToString();
                });
            }
        }

        Dictionary<int, YWData> randomYW = new Dictionary<int, YWData>();
        List<int> randomId = new List<int>();
        List<int> readyRd = new List<int>();

        foreach (var item in Datas.GetInstance().YWDataDic)
        {
            readyRd.Add(item.Key);
        }

        do
        {
            if (readyRd.Count == 0)
                break;

            int rd = Random.Range(0, readyRd.Count);

            randomYW.Add(readyRd[rd], Datas.GetInstance().YWDataDic[readyRd[rd]]);
            randomId.Add(readyRd[rd]);
            readyRd.RemoveAt(rd);

        } while (randomYW.Count < 3);

        foreach (var item in randomId)
        {
            PoolMgr.GetInstance().GetObj("Prefabs/YW选项", (x) =>
            {
                x.transform.SetParent(parent);
                x.transform.localScale = Vector3.one;
                x.GetComponent<Toggle>().group = parent.GetComponent<ToggleGroup>();
                x.GetComponent<Toggle>().onValueChanged.AddListener((y) =>
                {
                    switchYW = randomYW[item].id;
                    Debug.Log("选择该遗物：   ID = " + randomYW[item].id + "  遗物名 = " + randomYW[item].name);
                });

                GameTool.FindTheChild(x, "YWImg").GetComponent<Image>().sprite = ResMgr.GetInstance().Load<Sprite>(randomYW[item].imgPath);
                GameTool.FindTheChild(x, "YWText").GetComponent<Text>().text = randomYW[item].tips;
            });
        }
    }
    public override void HideMe()
    {
        Time.timeScale = 1;
        for (int i = parent.childCount-1; i >= 0; i--)
        {
            PoolMgr.GetInstance().PushObj(parent.GetChild(i).name, parent.GetChild(i).gameObject);
        }
        for (int i = showParent.childCount - 1; i >= 0; i--)
        {
            PoolMgr.GetInstance().PushObj(showParent.GetChild(i).name, showParent.GetChild(i).gameObject);
        }
    }
    protected override void OnClick(string btnName)
    {
        switch(btnName)
        {
            case "Bto_Choice":
                AddRelicsToPlayers();
                UIMgr.GetInstance().HidePanel("YWPanel");
                break;
        }
    }
    private void AddRelicsToPlayers()
    {
        if (!Datas.GetInstance().haveYWDic.ContainsKey(switchYW))
        {
            Datas.GetInstance().haveYWDic.Add(switchYW, 1);
        }
        else
        {
            Datas.GetInstance().haveYWDic[switchYW]++;
        }

        float addRoleSpd = 0, addAtkSpd = 0;

        foreach (var item in Datas.GetInstance().haveYWDic)
        {
            
            switch (Datas.GetInstance().YWDataDic[item.Key].type)
            {
                case YWType.攻速:
                    addAtkSpd += Datas.GetInstance().YWDataDic[item.Key].effect * item.Value;
                    break;
                case YWType.移速:
                    addRoleSpd += Datas.GetInstance().YWDataDic[item.Key].effect * item.Value;
                    break;
                case YWType.移转攻:
                    addAtkSpd += Datas.GetInstance().YWDataDic[item.Key].effect * item.Value;
                    addRoleSpd -= Datas.GetInstance().YWDataDic[item.Key].effect * item.Value;
                    break;
            }

        }
    }
}
