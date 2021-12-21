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
    private Queue<string> throwMagazine;
    // 拿取投掷物的最大数量
    private int maxThingNum = 10;

    // 当前玩家位置
    private Vector3 playerPos;
    // 最近的敌人
    private GameObject nearEnemy;
    // 敌人方向向量
    private Vector3 enemyDir;

    Timer cdTimer;
    public float throwSpeed = .5f;
    private float lastThrow;

    protected override void Awake()
    {
        base.Awake();

        if (throwMagazine == null)
        {
            throwMagazine = new Queue<string>();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        numText = GetControl<Text>("ThrowNum");
        // 从消息中心拿取消息
        EventCenter.GetInstance().AddEventListener<Collider2D[]>("附近投掷物", (x) => { missiles = x; });
        EventCenter.GetInstance().AddEventListener<Vector2>("PlayerPos", (x) => { playerPos = x; });
        EventCenter.GetInstance().AddEventListener<Collider2D>("距离最近的敌人", (x) =>
        {
            if (x == null)
            {
                nearEnemy = null;
                return;
            }

            nearEnemy = x.gameObject;
        });

        cdTimer = new Timer(throwSpeed, true);
        lastThrow = throwSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(lastThrow != throwSpeed)
        {
            cdTimer.Reset(throwSpeed, false);
            lastThrow = throwSpeed;
        }
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
                if (nearEnemy == null || !cdTimer.isTimeUp)
                {
                    Debug.Log("附近没怪 或者 CD没好");
                    return;
                }

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
        if(missiles.Length<1)
        {
            Debug.Log("拿了个空气！！");
            return;
        }

        Debug.Log("拿取  投掷物！！");
        // 一次只拿取一个，拿完跳出
        if (throwMagazine.Count < maxThingNum)
        {
            throwMagazine.Enqueue(missiles[0].name);
            PoolMgr.GetInstance().PushObj(missiles[0].name, missiles[0].gameObject);
            Debug.Log(throwMagazine.Count);
        }

    }
    /// <summary>
    /// 投掷
    /// </summary>
    private void ThrowThing()
    {
        // 如果弹匣里没有，则投掷石头进行攻击
        if (throwMagazine.Count == 0)
        {
            Debug.Log("没有更厉害的东西掷出，只能投掷石子了");
            ThrowBase("Prefabs/石子");
        }
        // 否则，扔弹匣中第一个投掷物
        else
        {
            string firstThrow = throwMagazine.Dequeue();
            Debug.Log(throwMagazine.Count);

            Debug.Log("掷出  投掷物！！        " + firstThrow);
            ThrowBase(firstThrow);
        }
    }
    /// <summary>
    /// 打开背包
    /// </summary>
    private void OpenBag()
    {
        Debug.Log("打开背包！！");
        UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
    }
    /// <summary>
    /// 投掷
    /// </summary>
    private void ThrowBase(string name)
    {
        Debug.Log(name);
        PoolMgr.GetInstance().GetObj(name, (x) =>
        {
            x.transform.position = playerPos;

            Rigidbody2D rg = x.GetComponent<Rigidbody2D>();

            // 有敌人就朝敌人发射
            if(nearEnemy != null)
            {
                enemyDir = (nearEnemy.transform.position - playerPos).normalized;
                rg.velocity = 5 * enemyDir;
            }

            if (x.GetComponent<ThrowItem>() == null)
            {
                x.AddComponent<ThrowItem>();
            }

        });
    }
}
