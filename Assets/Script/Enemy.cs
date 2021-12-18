using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 需要扣多少血
    private float hurtHpNum;
    private void Start()
    {
        // 获取<敌人扣血>的消息
        EventCenter.GetInstance().AddEventListener<float>("敌人扣血", (x) => { hurtHpNum = x; });
    }
    private void Update()
    {
        if(hurtHpNum!=0)
        {
            Debug.Log("敌人扣血：    " + hurtHpNum);
            hurtHpNum = 0;
        }
    }
}
