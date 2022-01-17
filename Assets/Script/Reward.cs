using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour
{
    private float moveTime = 2f;
    private float moveSpeed = .02f;
    private int dir = 1;

    float currentTime = 0;

    public int id;
    private RewardData data;

    private Vector2 startPos;
    private float checkLen = 5f;

    private void Start()
    {
        startPos = transform.position;
        data = GameTool.GetDicInfo(Datas.GetInstance().RewardDataDic, id);
    }
    private void FixedUpdate()
    {
        currentTime += .1f;

        transform.position = new Vector2(transform.position.x, transform.position.y + dir * moveSpeed);
        if (currentTime >= moveTime)
        {
            dir *= -1;
            currentTime = 0;
        }

        Collider2D player = Physics2D.OverlapCircle(transform.position, checkLen,LayerMask.GetMask("玩家"));
        if(player!=null)
        {
            transform.position = Vector3.Lerp(transform.position, player.transform.position, Vector2.Distance(transform.position, player.transform.position) / 10f * Time.fixedDeltaTime);
        }
    }

    public void InitPos()
    {
        transform.position = startPos;
        currentTime = 0;
        dir = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            EventCenter.GetInstance().EventTrigger<int>("获得金币", data.reward);
            PoolMgr.GetInstance().PushObj(data.path, gameObject);
        }
    }
}
