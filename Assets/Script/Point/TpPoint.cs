using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;
using System;

public enum TpType
{
    回去,
    下一关
}
public class TpPoint : MonoBehaviour
{
    Timer tpTimer;
    bool isStart;

    TpType tp;
    int level = 0;

    private void Start()
    {
        tpTimer = new Timer(5f, false, false);
    }
    private void FixedUpdate()
    {
        Collider2D col = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().bounds.size, .1f, LayerMask.GetMask("玩家"));
        if (col != null && !isStart)
        {
            isStart = true;
            tpTimer.Start();
            Debug.Log("玩家在传送点，准备传送");
        }

        tp = (level <= 15) ? TpType.下一关 : TpType.回去;

        if (tpTimer.isTimeUp)
        {
            PoolMgr.GetInstance().Clear();
            switch (tp)
            {
                case TpType.回去:
                    SceneMgr.GetInstance().LoadSceneAsyn("Game", () =>
                    {
                        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/RoomPrefabs/准备房", (z) =>
                        {
                            Player.instance.transform.position = GameObject.Find("返回点").transform.position;
                        });
                    });
                    break;
                case TpType.下一关:
                    level++;
                    UIMgr.GetInstance().ShowPanel<LoadingPanel>("LoadingPanel", E_UI_Layer.Load);
                    SceneMgr.GetInstance().LoadSceneAsyn("Level", () =>
                    {
                        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/Room", (x) =>
                         {
                         });

                        UIMgr.GetInstance().HidePanel("LoadingPanel");
                    });
                    break;
            }
            tpTimer.Reset(5f);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().bounds.size);
    }
#endif
}
