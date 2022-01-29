using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YWPanel : UIBase
{
    Transform parent;

    Dictionary<int, YWData> randomYW = new Dictionary<int, YWData>();
    List<int> randomId = new List<int>();

    private void Start()
    {
        parent = GameTool.FindTheChild(gameObject, "parent");
    }
    public override void ShowMe()
    {
        Time.timeScale = 0;
        int safe = 0;

        List<int> readyRd = new List<int>();
        foreach (var item in Datas.GetInstance().YWDataDic)
        {
            readyRd.Add(item.Key);
        }

        while (randomYW.Count < 3 || safe < 10)
        {
            if (readyRd.Count == 0)
                break;

            int rd = Random.Range(0, readyRd.Count);

            Debug.Log(readyRd[rd]);
            if (randomYW.ContainsKey(readyRd[rd]))
            {
                readyRd.RemoveAt(rd);
                safe++;
                continue;
            }

            randomYW.Add(readyRd[rd], Datas.GetInstance().YWDataDic[readyRd[rd]]);
            randomId.Add(readyRd[rd]);
        }

        foreach (var item in randomId)
        {
            PoolMgr.GetInstance().GetObj("Prefabs/YW选项", (x) =>
            {
                x.transform.SetParent(parent);
                x.transform.localScale = Vector3.one;
                x.GetComponent<Toggle>().group = parent.GetComponent<ToggleGroup>();
                x.GetComponent<Toggle>().onValueChanged.AddListener((y) =>
                {
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
    }
    protected override void OnClick(string btnName)
    {
        switch(btnName)
        {
            case "Bto_Choice":
                UIMgr.GetInstance().HidePanel("YWPanel");
                break;
        }
    }
}
