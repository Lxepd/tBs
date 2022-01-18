using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : UIBase
{
    Transform parent;
    WeaponData weaponData;

    int afterId;
    string compareImgPath = "Sprites/UIsprite/��ͷ/";

    private int coinNum;
    private int cost;

    void Start()
    {
        parent = GameTool.FindTheChild(gameObject, "Content");

        EventCenter.GetInstance().AddEventListener<WeaponData>("ǹ֧����", (x) => { weaponData = x; });
        EventCenter.GetInstance().AddEventListener<int>("��ǰ���", (x) =>
        {
            coinNum = x;
            GetControl<Text>("CoinNum").text = x.ToString();
        });
    }
    private void Update()
    {
        UpdateUpgrade();
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "UpgradeBto":
                if (coinNum < cost)
                {
                    Debug.Log("Ǯ����");
                    return;
                }
                Debug.Log("����");
                EventCenter.GetInstance().EventTrigger<int>("ǹ֧����", afterId);
                EventCenter.GetInstance().EventTrigger<int>("��ý��", -cost);
                break;
            case "CloseBto":
                UIMgr.GetInstance().HidePanel("UpgradePanel");
                break;
        }
    }
    private void UpdateUpgrade()
    {
        if (weaponData == null)
            return;

        int updateCount = 0;
        foreach (var item in Datas.GetInstance().UpgradeDataDic)
        {
            if (item.Value.beforeId != weaponData.id)
                continue;

            updateCount++;
            if (updateCount <= parent.childCount)
            {
                GameObject go = parent.GetChild(updateCount - 1).gameObject;
                ChangeUpgradeGo(go, item.Value);
            }
            else
            {
                PoolMgr.GetInstance().GetObj("Prefabs/����ѡ��", (x) =>
                {
                    x.transform.SetParent(parent);
                    x.transform.localScale = Vector3.one;
                    x.GetComponent<Toggle>().group = parent.GetComponent<ToggleGroup>();
                    ChangeUpgradeGo(x, item.Value);
                });
            }
        }

        for (int i = parent.childCount - 1; i > updateCount - 1; i--)
        {
            PoolMgr.GetInstance().PushObj(parent.GetChild(i).name, parent.GetChild(i).gameObject);
        }
    }
    private void ChangeUpgradeGo(GameObject go, UpgradeData data)
    {
        Image before = GameTool.FindTheChild(go, "ԭ����Img").GetComponent<Image>();
        Image after = GameTool.FindTheChild(go, "����������Img").GetComponent<Image>();

        before.sprite = ResMgr.GetInstance().Load<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().WeaponDataDic, data.beforeId).spritePath);
        after.sprite = ResMgr.GetInstance().Load<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().WeaponDataDic, data.afterId).spritePath);

        go.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        go.GetComponent<Toggle>().onValueChanged.AddListener((x) =>
        {
            Debug.Log(data.id);
            cost = data.cost;
            afterId = data.afterId;
        });

        ChangeGunCompareImg(go, data);
    }
    private void ChangeGunCompareImg(GameObject go, UpgradeData data)
    {
        WeaponData beforeGun = GameTool.GetDicInfo(Datas.GetInstance().WeaponDataDic, data.beforeId);
        WeaponData afterGun = GameTool.GetDicInfo(Datas.GetInstance().WeaponDataDic, data.afterId);

        GameTool.FindTheChild(go, "CostText").GetComponent<Text>().text = data.cost.ToString();
        GameTool.FindTheChild(go, "������Img").GetComponent<Image>().sprite = GunCompare(beforeGun.atk, afterGun.atk);
        GameTool.FindTheChild(go, "����Img").GetComponent<Image>().sprite = GunCompare(beforeGun.bulletSpeed, afterGun.bulletSpeed);
        GameTool.FindTheChild(go, "���Img").GetComponent<Image>().sprite = GunCompare(beforeGun.shootLen, afterGun.shootLen);
        GameTool.FindTheChild(go, "���Img").GetComponent<Image>().sprite = GunCompare(afterGun.shootNextTime, beforeGun.shootNextTime);
    }
    private Sprite GunCompare(float before, float after)
    {
        if (before == after)
            return ResMgr.GetInstance().Load<Sprite>(compareImgPath + "����");

        return (before < after) ? ResMgr.GetInstance().Load<Sprite>(compareImgPath + "����") : ResMgr.GetInstance().Load<Sprite>(compareImgPath + "����"); ;
    }
}
