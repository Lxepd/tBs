using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : UIBase
{
    // 投掷物列表
    private Collider2D[] missiles;
    // 投掷物数量
    private Text numText;
    // 投掷物弹匣
    // 将拿取的 投掷物放入 队列
    private Queue<GameObject> throwMagazine;
    // 拿取投掷物的最大数量
    private int maxThingNum = 5;

    // 当前玩家位置
    private Vector2 playerPos;

    protected override void Awake()
    {
        base.Awake();

        if (throwMagazine == null)
        {
            throwMagazine = new Queue<GameObject>();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        numText = GetControl<Text>("ThrowNum");
        EventCenter.GetInstance().AddEventListener<Collider2D[]>("附近投掷物", (x) => { missiles = x; });
        EventCenter.GetInstance().AddEventListener<Vector2>("PlayerPos", (x)=> { playerPos = x; });

        TimerAction.GetInstance().AddTimerActionDic("扔石子", .5f, Throw);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void PushQueue(GameObject thing)
    {
        if (throwMagazine.Count < maxThingNum)
            throwMagazine.Enqueue(thing);
    }

    /// <summary>
    /// 按钮点击事件注册
    /// </summary>
    /// <param name="btnName">按钮名</param>
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "TakeBto":
                TakeThing();
                break;
            case "ThrowBto":
                ThrowThing();
                break;
            case "BagBto":
                OpenBag();
                break;
        }
    }
    /// <summary>
    /// 拿取
    /// </summary>
    private void TakeThing()
    {
        Debug.Log("拿取  投掷物！！");
        // 遍历所有 玩家角色附近的 collider物
        foreach (var item in missiles)
        {
            // 一次只拿取一个，拿完跳出
            PushQueue(item.gameObject);
            item.gameObject.SetActive(false);
            Debug.Log(throwMagazine.Count);
            return;
        }
    }
    /// <summary>
    /// 投掷
    /// </summary>
    private void ThrowThing()
    {
        if (TimerAction.GetInstance().isPlayer)
            return;

        if (throwMagazine.Count == 0)
        {
            Debug.Log("没有更厉害的东西掷出，只能投掷石子了");
            TimerAction.GetInstance().PlayerAction("扔石子");

            return;
        }

        GameObject th = throwMagazine.Dequeue();

        Debug.Log("掷出  投掷物！！        " + th.name);
        PoolMgr.GetInstance().PushObj(th.name,th);
    }
    /// <summary>
    /// 打开背包
    /// </summary>
    private void OpenBag()
    {
        Debug.Log("打开背包！！");
        UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
    }

    private void Throw()
    {
        PoolMgr.GetInstance().GetObj("Prefabs/石子", (x) =>
        {
            x.transform.position = playerPos;

            Rigidbody2D rg = x.GetComponent<Rigidbody2D>();
            rg.velocity = Vector2.right * 5;

            if (x.GetComponent<ThrowItem>() == null)
            {
                x.AddComponent<ThrowItem>();
            }
        });
    }
}
