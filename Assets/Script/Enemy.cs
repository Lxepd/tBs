using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // ��Ҫ�۶���Ѫ
    private float hurtHpNum;
    private void Start()
    {
        // ��ȡ<���˿�Ѫ>����Ϣ
        EventCenter.GetInstance().AddEventListener<float>("���˿�Ѫ", (x) => { hurtHpNum = x; });
    }
    private void Update()
    {
        if(hurtHpNum!=0)
        {
            Debug.Log("���˿�Ѫ��    " + hurtHpNum);
            hurtHpNum = 0;
        }
    }
}
