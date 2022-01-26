using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    float tpTime = 3f;
    bool isStart;

    TpType tp;
    private void Start()
    {
        tpTimer = new Timer(tpTime, false, false);
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
        else if(col==null && !tpTimer.isStop)
        {
            isStart = false;
            tpTimer.Reset(tpTime);
            Debug.Log("取消传送");
        }

        tp = (LevelMgr.GetInstance().level < 15) ? TpType.下一关 : TpType.回去;

        if (tpTimer.isTimeUp)
        {
            PoolMgr.GetInstance().Clear();
            switch (tp)
            {
                case TpType.回去:
                    LevelMgr.GetInstance().isInLevel = false;
                    SceneMgr.GetInstance().LoadSceneAsyn("Game", () =>
                    {
                        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/RoomPrefabs/准备房", (z) =>
                        {
                            col.transform.position = GameTool.FindTheChild(z,"返回点").transform.position;
                        });
                    });
                    break;
                case TpType.下一关:
                    LevelMgr.GetInstance().level++;
                    LevelMgr.GetInstance().isInLevel = true;
                    PoolMgr.GetInstance().Clear();
                    SceneMgr.GetInstance().LoadScene("Level", () =>
                     {
                         ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/Room",(x)=>
                         {

                         });
                     });

                    break;
            }
            tpTimer.Reset(tpTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().bounds.size);
    }
}
