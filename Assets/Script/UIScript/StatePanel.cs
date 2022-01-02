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
    private int currentHp;
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
    }

    void Update()
    {
        if (data == null)
            return;

        // 更新血量
        GetControl<Image>("HpBar").fillAmount = currentHp / data.MaxHp;
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
        GetControl<Text>("CurrentHp").text = currentHp.ToString();
    }
}
