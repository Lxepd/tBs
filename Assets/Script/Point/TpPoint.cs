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
    bool isStart;

    TpType tp;

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
        else if(col==null && !tpTimer.isStop)
        {
            isStart = false;
            tpTimer.Reset(5f);
            Debug.Log("取消传送");
        }

        tp = (LevelMgr.GetInstance().level < 15) ? TpType.下一关 : TpType.回去;

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
                            col.transform.position = GameObject.Find("返回点").transform.position;
                        });
                    });
                    break;
                case TpType.下一关:
                    LevelMgr.GetInstance().level++;
                    Debug.Log(LevelMgr.GetInstance().level);
                    SceneMgr.GetInstance().LoadScene("Level", () =>
                     {
                         ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/Room", (x) =>
                         {
                             col.transform.position = x.GetComponent<TileSet>().appearPoint.position;
                         });
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
