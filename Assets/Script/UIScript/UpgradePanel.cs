using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : UIBase
{
    Transform parent;
    WeaponData beforeWeaponData;
    UpgradeData upgradeWeaponData;

    //int afterId;
    string compareImgPath = "Sprites/UIsprite/箭头/";

    private int coinNum;
    //private int cost;

    bool isInit;
    [HideInInspector] List<升级> upgradeList = new List<升级>();

    void Start()
    {
        parent = GameTool.FindTheChild(gameObject, "Content");

        EventCenter.GetInstance().AddEventListener<WeaponData>("枪支数据", (x) => { beforeWeaponData = x; });
        EventCenter.GetInstance().AddEventListener<int>("当前金币", (x) =>
        {
            coinNum = x;
            GetControl<Text>("CoinNum").text = x.ToString();
        });
        EventCenter.GetInstance().AddEventListener<List<升级>>("武器升级", (x) =>
        {
            if (!isInit)
            {
                upgradeList = x;
                InitUpgradeShop(x);
                isInit = true;
            }
        });
    }
    private void Update()
    {
        UpdateUpgrade();
    }
    public override void ShowMe()
    {
        isInit = false;
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "UpgradeBto":
                if (coinNum < upgradeWeaponData.cost)
                {
                    Debug.Log("钱不够");
                    return;
                }
                if (upgradeWeaponData.beforeId != beforeWeaponData.id)
                {
                    Debug.Log("所需要升级的武器不对应");
                    return;
                }
                EventCenter.GetInstance().EventTrigger<int>("枪支更新", upgradeWeaponData.afterId);
                EventCenter.GetInstance().EventTrigger<int>("获得金币", -upgradeWeaponData.cost);
                break;
            case "CloseBto":
                UIMgr.GetInstance().HidePanel("UpgradePanel");
                break;
        }
    }
    private void InitUpgradeShop(List<升级> upgrade)
    {
        foreach (var item in upgrade)
        {
            PoolMgr.GetInstance().GetObj("Prefabs/升级选项", (x) =>
            {
                x.transform.SetParent(parent);
                x.transform.localScale = Vector3.one;
                x.GetComponent<Toggle>().group = parent.GetComponent<ToggleGroup>();

                UpgradeData ud = Datas.GetInstance().UpgradeDataDic[item.id];
                ChangeUpgradeGo(x, ud);
            });
        }
    }
    private void UpdateUpgrade()
    {
        if (!isInit)
            return;

        for (int i = upgradeList.Count; i < parent.childCount; i++)
        {
            PoolMgr.GetInstance().PushObj(parent.GetChild(i).name, parent.GetChild(i).gameObject);
        }

        for (int i = 0; i < upgradeList.Count; i++)
        {
            UpgradeData ud = Datas.GetInstance().UpgradeDataDic[upgradeList[i].id];
            ChangeUpgradeGo(parent.GetChild(i).gameObject, ud);
        }

        //if (weaponData == null)
        //    return;

        //int updateCount = 0;
        //foreach (var item in Datas.GetInstance().UpgradeDataDic)
        //{
        //    if (item.Value.beforeId != weaponData.id)
        //        continue;

        //    updateCount++;
        //    if (updateCount <= parent.childCount)
        //    {
        //        GameObject go = parent.GetChild(updateCount - 1).gameObject;
        //        ChangeUpgradeGo(go, item.Value);
        //    }
        //    else
        //    {
        //        PoolMgr.GetInstance().GetObj("Prefabs/升级选项", (x) =>
        //        {
        //            x.transform.SetParent(parent);
        //            x.transform.localScale = Vector3.one;
        //            x.GetComponent<Toggle>().group = parent.GetComponent<ToggleGroup>();
        //            ChangeUpgradeGo(x, item.Value);
        //        });
        //    }
        //}

        //for (int i = parent.childCount - 1; i > updateCount - 1; i--)
        //{
        //    PoolMgr.GetInstance().PushObj(parent.GetChild(i).name, parent.GetChild(i).gameObject);
        //}
    }
    private void ChangeUpgradeGo(GameObject go, UpgradeData data)
    {
        Image before = GameTool.FindTheChild(go, "原武器Img").GetComponent<Image>();
        Image after = GameTool.FindTheChild(go, "升级后武器Img").GetComponent<Image>();

        before.sprite = ResMgr.GetInstance().Load<Sprite>(Datas.GetInstance().WeaponDataDic[data.beforeId].spritePath);
        after.sprite = ResMgr.GetInstance().Load<Sprite>(Datas.GetInstance().WeaponDataDic[data.afterId].spritePath);

        go.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        go.GetComponent<Toggle>().onValueChanged.AddListener((x) =>
        {
            upgradeWeaponData = data;
        });

        ChangeGunCompareImg(go, data);
    }
    private void ChangeGunCompareImg(GameObject go, UpgradeData data)
    {
        WeaponData beforeGun = Datas.GetInstance().WeaponDataDic[data.beforeId];
        WeaponData afterGun = Datas.GetInstance().WeaponDataDic[data.afterId];

        GameTool.FindTheChild(go, "CostText").GetComponent<Text>().text = data.cost.ToString();
        GameTool.FindTheChild(go, "攻击力Img").GetComponent<Image>().sprite = GunCompare(beforeGun.atk, afterGun.atk);
        GameTool.FindTheChild(go, "弹速Img").GetComponent<Image>().sprite = GunCompare(beforeGun.bulletSpeed, afterGun.bulletSpeed);
        GameTool.FindTheChild(go, "射程Img").GetComponent<Image>().sprite = GunCompare(beforeGun.shootLen, afterGun.shootLen);
        GameTool.FindTheChild(go, "间隔Img").GetComponent<Image>().sprite = GunCompare(afterGun.shootNextTime, beforeGun.shootNextTime);
    }
    private Sprite GunCompare(float before, float after)
    {
        if (before == after)
            return ResMgr.GetInstance().Load<Sprite>(compareImgPath + "不变");

        return (before < after) ? ResMgr.GetInstance().Load<Sprite>(compareImgPath + "提升") : ResMgr.GetInstance().Load<Sprite>(compareImgPath + "降低"); ;
    }
}
