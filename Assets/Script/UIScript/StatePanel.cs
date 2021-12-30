using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatePanel : UIBase
{
    private PlayerData data;
    private int currentHp;

    public override void ShowMe()
    {
        data = GameMgr.GetInstance().GetPlayerInfo(Player.instance.id);

        InitStateUI();
    }

    void Update()
    {
        GetControl<Image>("HpBar").fillAmount = currentHp / data.MaxHp;
    }

    private void InitStateUI()
    {
        currentHp = data.MaxHp;
        GetControl<Text>("CurrentHp").text = data.MaxHp.ToString();
        GetControl<Text>("MaxHp").text = data.MaxHp.ToString();

    }

    private void ChangeHp(int hurt)
    {
        currentHp -= hurt;
        GetControl<Text>("CurrentHp").text = currentHp.ToString();
    }
}
