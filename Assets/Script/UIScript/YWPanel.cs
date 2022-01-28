using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YWPanel : UIBase
{
    Transform parent;
    private void Start()
    {
        parent = GameTool.FindTheChild(gameObject, "parent");
    }
    public override void ShowMe()
    {
        Time.timeScale = 0;

        for (int i = 0; i < 3; i++)
        {
            PoolMgr.GetInstance().GetObj("Prefabs/YW选项", (x) =>
            {
                x.transform.SetParent(parent);
                x.transform.localScale = Vector3.one;
                x.GetComponent<Toggle>().group = parent.GetComponent<ToggleGroup>();
                x.GetComponent<Toggle>().onValueChanged.AddListener((y) =>
                {
                    Debug.Log("选择该遗物");
                });
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
