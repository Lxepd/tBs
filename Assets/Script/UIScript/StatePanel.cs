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

    /// <summary>
    /// 界面展开时执行
    /// </summary>
    public override void ShowMe()
    {
        // 获取数据
        EventCenter.GetInstance().AddEventListener<int>("角色信息", (x) =>
        {
            data = GameTool.GetDicInfo(Datas.GetInstance().PlayerDataDic, x);
            // 初始化UI
            InitStateUI();
        });
        EventCenter.GetInstance().AddEventListener<ThrowItemData>("玩家扣血", (x) =>
        {
            ChangeHp((int)x.hurt);
        });
        EventCenter.GetInstance().AddEventListener<int>("状态界面清空", (x)=>
        {
            data = null;
            BagItemIDList.Clear();
            BagItem.Clear();
            index = 0;
        });
    }

    void Update()
    {
        if (data == null)
            return;

        // 更新血量
        GetControl<Image>("HpBar").fillAmount = currentHp / data.MaxHp;
        GetBagItem();
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
        GetControl<Text>("CurrentHp").text = data.MaxHp.ToString();
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
            currentHp = 0;

        GetControl<Text>("CurrentHp").text = currentHp.ToString();
    }
    private void GetBagItem()
    {
        Transform content = GameTool.FindTheChild(UIMgr.GetInstance().GetLayerFather(E_UI_Layer.Above).gameObject, "背包界面");
        // 获取背包内的可消耗道具
        for (int i = 0; i < content.childCount; i++)
        {
            ItemClick ic = content.GetChild(i).GetComponent<ItemClick>();
            ItemData itemd = GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, ic.id);
            // 排除背包内<非消耗品>
            if (itemd.itemType == ItemType.非消耗品)
                continue;
            // 检测字典有没有
            // 有则更新数量
            if (BagItem.ContainsKey(ic.id))
            {
                BagItem[ic.id] = ic.currentNum;
            }
            // 无则添加
            else
            {
                BagItem.Add(ic.id, ic.currentNum);
                BagItemIDList.Add(ic.id);
            }
        }

    }
    /// <summary>
    /// 将<一次性消耗品>与<可重复使用消耗品>放置状态边上的物品格子中
    /// </summary>
    /// <param name="index"></param>
    private void SetBagItem(int index)
    {
        if (BagItemIDList.Count == 0)
            return;

        ResMgr.GetInstance().LoadAsync<Sprite>(GameTool.GetDicInfo(Datas.GetInstance().ItemDataDic, BagItemIDList[index]).path, (x) =>
        {
            GetControl<Image>("Item").sprite = x;
            GetControl<Text>("ItemNum").text = BagItem[BagItemIDList[index]].ToString();
        });
    }
}
