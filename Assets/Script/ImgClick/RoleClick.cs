using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
