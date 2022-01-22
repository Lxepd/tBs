using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ɫ�����
/// </summary>
public class RoleClick : ImgClickBase
{
    [Tooltip("��ɫ��id")]
    public int RoleId;
    protected override void Start()
    {
        base.Start();
        GameTool.FindTheChild(gameObject, "Text").GetComponent<Text>().text = Datas.GetInstance().PlayerDataDic[RoleId].name;
    }

    private void Update()
    {
        // �ƶ��˵��
        PhoneTouch(() =>
        {
            EventCenter.GetInstance().EventTrigger<int>("ѡ���ɫ", RoleId);
        });
        // PC�˵��
        PCMouseDown(() => 
        {
            EventCenter.GetInstance().EventTrigger<int>("ѡ���ɫ", RoleId);
        });
    }
}
