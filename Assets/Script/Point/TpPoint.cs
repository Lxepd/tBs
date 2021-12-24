using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public enum TpType
{
    ��ȥ,
    ��һ��
}
[Serializable]
public class TpData
{
    [Tooltip("����type")]
    public TpType type;
    [Tooltip("��һ��id")]
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
        Collider2D col = Physics2D.OverlapBox(transform.position, GetComponent<BoxCollider2D>().bounds.size, .1f, LayerMask.GetMask("���"));
        if (col != null && !isStart)
        {
            isStart = true;
            tpTimer.Start();
            Debug.Log("����ڴ��͵㣬׼������");
        }

        if (tpTimer.isTimeUp)
        {

            switch (tp.type)
            {
                case TpType.��ȥ:
                    SceneMgr.GetInstance().LoadSceneAsyn("Game", () =>
                    {
                        Player.instance.transform.position = GameObject.Find("���ص�").transform.position;
                    });
                    break;
                case TpType.��һ��:
                    Debug.Log(GameMgr.GetInstance().RoomDataDic[tp.roomID].name);
                    SceneMgr.GetInstance().LoadSceneAsyn("Level", () =>
                    {
                        GameObject room = Instantiate(Resources.Load<GameObject>(GameMgr.GetInstance().RoomDataDic[tp.roomID].prefabPath));
                        Player.instance.transform.position = room.transform.FindChild("���ֵ�").position;
                        ResetRoom();
                    });
                    break;
            }
            //PoolMgr.GetInstance().GetObj(GameMgr.GetInstance().GetRoomInfo(tp.roomID).prefabPath, (x) =>
            //{
            //    Player.instance.transform.position = x.transform.FindChild("���ֵ�").position;
            //    ResetRoom();
            //});
        }
    }

    private void ResetRoom()
    {
        isStart = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider2D>().bounds.size);
    }
}
