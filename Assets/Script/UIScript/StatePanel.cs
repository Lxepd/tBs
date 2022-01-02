using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ״̬����
/// </summary>
public class StatePanel : UIBase
{
    // �������
    private PlayerData data;
    // ��ǰѪ��
    private int currentHp;
    /// <summary>
    /// ����չ��ʱִ��
    /// </summary>
    public override void ShowMe()
    {
        // ��ȡ����
        EventCenter.GetInstance().AddEventListener<int>("��ɫ��Ϣ", (x) =>
        {
            data = GameTool.GetDicInfo(Datas.GetInstance().PlayerDataDic, x);
            // ��ʼ��UI
            InitStateUI();
        });   
    }

    void Update()
    {
        if (data == null)
            return;

        // ����Ѫ��
        GetControl<Image>("HpBar").fillAmount = currentHp / data.MaxHp;
    }
    /// <summary>
    /// ��Ӧ��ť���
    /// </summary>
    /// <param name="btnName"></param>
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "Bto_List":
                // չ��<�б�>����
                UIMgr.GetInstance().ShowPanel<ListPanel>("ListPanel", E_UI_Layer.Above);
                break;
        }
    }
    /// <summary>
    /// UI��ʼ��
    /// </summary>
    private void InitStateUI()
    {
        // ��ȡ���Ѫ��
        currentHp = data.MaxHp;
        // ��ʼ���ı�
        GetControl<Text>("CurrentHp").text = data.MaxHp.ToString();
        GetControl<Text>("MaxHp").text = data.MaxHp.ToString();

    }
    /// <summary>
    /// ��Ѫִ��
    /// </summary>
    /// <param name="hurt">�۳���Ѫ��</param>
    private void ChangeHp(int hurt)
    {
        currentHp -= hurt;
        GetControl<Text>("CurrentHp").text = currentHp.ToString();
    }
}
