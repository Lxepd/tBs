using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public enum TpType
{
    回去,
    下一关
}
[Serializable]
public class TpData
{
    [Tooltip("传送type")]
    public TpType type;
    [Tooltip("下一关id")]
    public int roomID;
}
public class TpPoint : MonoBehaviour
{
    Timer tpTimer;
    bool isStart;

    public TpData tp;

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

        if (tpTimer.isTimeUp)
        {
            PoolMgr.GetInstance().Clear();
            switch (tp.type)
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
                    Debug.Log(GameTool.GetDicInfo(Datas.GetInstance().RoomDataDic, tp.roomID).name);
                    SceneMgr.GetInstance().LoadSceneAsyn("Level", () =>
                    {
                        GameObject room = Instantiate(Resources.Load<GameObject>(GameTool.GetDicInfo(Datas.GetInstance().RoomDataDic, tp.roomID).prefabPath));
                        Player.instance.transform.position = room.transform.Find("出现点").position;
                    });
                    break;
            }
            tpTimer.Reset(5f);
            //PoolMgr.GetInstance().GetObj(GameMgr.GetInstance().GetRoomInfo(tp.roomID).prefabPath, (x) =>
            //{
            //    Player.instance.transform.position = x.transform.FindChild("出现点").position;
            //    ResetRoom();
            //});
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
