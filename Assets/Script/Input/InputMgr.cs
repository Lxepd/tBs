using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������
/// 1. Input��
/// 2. �¼�����
/// 3. ����Monoʹ��
/// </summary>
public class InputMgr : InstanceNoMono<InputMgr>
{
    /// <summary>
    /// �ڹ��캯���У����Update����
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
