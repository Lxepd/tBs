using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色卡点击
/// </summary>
public class RoleClick : ImgClickBase
{
    [Tooltip("角色卡id")]
    public int RoleId;
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        // 移动端点击
        PhoneTouch(() =>
        {
            EventCenter.GetInstance().EventTrigger<int>("选择角色", RoleId);
        });
        // PC端点击
        PCMouseDown(() => 
        {
            EventCenter.GetInstance().EventTrigger<int>("选择角色", RoleId);
        });
    }
}
