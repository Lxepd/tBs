using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : UIBase
{
    // 投掷物列表
    private Collider2D[] missiles;
    // 投掷物数量
    private Text numText;

    // Start is called before the first frame update
    void Start()
    {
        numText = GetControl<Text>("ThrowNum");
        EventCenter.GetInstance().AddEventListener<Collider2D[]>("Missile", (x) => { missiles = x; });
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "TakeBto":
                TakeThing();
                break;
            case "ThrowBto":
                ThrowThing();
                break;
            case "BagBto":
                Test();
                break;
        }
    }
    private void TakeThing()
    {
        Debug.Log("拿取  投掷物！！");
        // 遍历所有 玩家角色附近的 collider物
        foreach (var item in missiles)
        {
            // 排除玩家
            if (item.CompareTag("投掷物"))
            {
                Debug.Log(item.name);
            }
        }
    }
    private void ThrowThing()
    {
        Debug.Log("掷出  投掷物！！");
    }

    private void Test()
    {
        Debug.Log("打开背包！！");
        UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
    }
}
