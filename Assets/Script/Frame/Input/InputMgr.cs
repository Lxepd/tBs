using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 输入输出
/// 1. Input类
/// 2. 事件中心
/// 3. 公共Mono使用
/// </summary>
public class InputMgr : InstanceNoMono<InputMgr>
{
    /// <summary>
    /// 在构造函数中，添加Update监听
    /// </summary>
    public InputMgr()
    {
        MonoMgr.GetInstance().AddUpdateListener(MyUpdate);
    }
    private void MyUpdate()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            
        }
        if (Input.GetKeyUp(KeyCode.W))
        {

        }
    }
}
