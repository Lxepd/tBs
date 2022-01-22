using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 状态界面
/// </summary>
public class StatePanel : UIBase
{
    // 玩家数据
    private PlayerData data;
    // 当前血量
    private float currentHp;
    // 存放背包消耗品的id与数量
    Dictionary<int, int> BagItem = new Dictionary<int, int>();
    // 存放id的顺序表
    List<int> BagItemIDList = new List<int>();
    // 下标
    int index = 0;

    private void Start()
    {
        // 获取数据
        EventCenter.GetInstance().AddEventListener<PlayerData>("角色初始", (x) =>
        {
            data = x;
            // 初始化UI
            InitStateUI();
        });
        EventCenter.GetInstance().AddEventListener<float>("玩家扣血", (x) =>
        {
            //MusicMgr.GetInstance().PlaySound("damaged1", false);
            ChangeHp((int)x);
        });
        EventCenter.GetInstance().AddEventListener<ItemClick>("成功购买的道具", (x) =>
        {
            GetBagItem(x);
        });
        EventCenter.GetInstance().AddEventListener<int>("道具栏清空", (x) =>
        {
            data = null;
            BagItem.Clear();
            BagItemIDList.Clear();
            index = 0;

            GetControl<Image>("Item").sprite = null;
            GetControl<Image>("Item").color = new Color(43 / 255f, 43 / 255f, 43 / 255f);
            GetControl<Text>("ItemNum").text = "";
        });
        EventCenter.GetInstance().AddEventListener("角色恢复", () =>
         {
             // 初始化UI
             InitStateUI();
         });
    }
    void Update()
    {
        if (data == null)
            return;

        // 更新血量
        GetControl<Text>("CurrentHp").text = currentHp.ToString();
        GetControl<Image>("HpBar").fillAmount = Mathf.Lerp(GetControl<Image>("HpBar").fillAmount, currentHp / data.MaxHp, Time.deltaTime * 10f);

        List<int> zeroList = new List<int>();
        foreach (var item in BagItem)
        {
            if(item.Value <= 0)
            {
                zeroList.Add(item.Key);
            }
        }
        for (int i = 0; i < zeroList.Count; i++)
        {
            BagItem.Remove(zeroList[i]);
            BagItemIDList.Remove(zeroList[i]);
        }

        SetBagItem(index);
    }
    /// <summary>
    /// 对应按钮点击
    /// </summary>
    /// <param name="btnName"></param>
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "Bto_List":
                // 展开<列表>界面
                UIMgr.GetInstance().ShowPanel<ListPanel>("ListPanel", E_UI_Layer.Above);
                break;
            // 道具栏的按钮
            // 左切
            case "Bto_Left":
                Debug.Log("道具左切");
                // 排除背包没有任何消耗道具的情况
                if (BagItemIDList.Count == 0)
                    return;
                if (index > 0)
                {
                    --index;
                }
                break;
            // 右切
            case "Bto_Right":
                Debug.Log("道具右切");
                // 排除背包没有任何消耗道具的情况
                if (BagItemIDList.Count == 0)
                    return;
                if (index < BagItemIDList.Count - 1)
                {
                    ++index;
                }
                break;
            // 使用
            case "Bto_":
                Debug.Log("道具使用");
                int itemId = BagItemIDList[index];
                switch (Datas.GetInstance().ItemDataDic[itemId].actType)
                {
                    case ItemActType.血量恢复:
                        Debug.Log(itemId);
                        currentHp += Datas.GetInstance().ItemDataDic[BagItemIDList[index]].recovery;
                        break;
                    case ItemActType.攻速强化:
                        break;
                }

                switch (Datas.GetInstance().ItemDataDic[itemId].itemType)
                {
                    case ItemType.一次性消耗品:
                        BagItem[itemId]--;
                        break;
                    case ItemType.可重复使用消耗品:
                        break;
                    case ItemType.非消耗品:
                        break;
                }

                EventCenter.GetInstance().EventTrigger<int>("道具使用消耗", itemId);
                break;

        }
    }
    /// <summary>
    /// UI初始化
    /// </summary>
    private void InitStateUI()
    {
        // 获取最大血量
        currentHp = data.MaxHp;
        // 初始化文本
        GetControl<Text>("MaxHp").text = data.MaxHp.ToString();
    }
    /// <summary>
    /// 扣血执行
    /// </summary>
    /// <param name="hurt">扣除的血量</param>
    private void ChangeHp(int hurt)
    {
        currentHp -= hurt;

        if (currentHp <= 0)
        {
            currentHp = 0;
            EventCenter.GetInstance().EventTrigger("玩家死亡");
        }
        else
        {
            EventCenter.GetInstance().EventTrigger("玩家受伤");
        }

    }
    private void GetBagItem(ItemClick item)
    {
        if (!BagItem.ContainsKey(item.id))
        {
            BagItem.Add(item.id, 1);
            BagItemIDList.Add(item.id);
        }
        else
        {
            BagItem[item.id] += 1;
        }
    }
    /// <summary>
    /// 将<一次性消耗品>与<可重复使用消耗品>放置状态边上的物品格子中
    /// </summary>
    /// <param name="index"></param>
    private void SetBagItem(int index)
    {
        if (BagItemIDList.Count == 0)
        {
            GetControl<Image>("Item").sprite = null;
            GetControl<Image>("Item").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
            GetControl<Text>("ItemNum").text = "";
            return;
        }

        ResMgr.GetInstance().LoadAsync<Sprite>(Datas.GetInstance().ItemDataDic[ BagItemIDList[index]].path, (x) =>
        {
            GetControl<Image>("Item").sprite = x;
            GetControl<Image>("Item").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            GetControl<Text>("ItemNum").text = BagItem[BagItemIDList[index]].ToString();
        });
    }
}
