using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CtrlType
{
    射击,
    打开商店,
    传送
}
public class ControlPanel : UIBase
{
    // 强化物列表
    private Collider2D[] strengthen;
    // 强化物数量
    private Text numText;
    // 强化物弹匣
    // 将拿取的 强化物放入 队列
    private Queue<BulletBag> strengthenQueue;
    // 拿取强化物的最大数量
    private int maxThingNum = 10;
    // 当前玩家位置
    private Vector3 playerPos;
    // 最近的敌人
    private GameObject nearEnemy;
    // 敌人方向向量
    private Vector3 enemyDir;
    // 计时器
    private Timer cdTimer;
    // 投掷速度
    public float throwSpeed = .2f;
    // 改变前的投掷速度
    private float lastThrow;

    CtrlType ctrlType;
    Npc npcComponent;
    protected override void Awake()
    {
        base.Awake();
        // 初始化队列
        if (strengthenQueue == null)
        {
            strengthenQueue = new Queue<BulletBag>();
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        // 获取text组件
        numText = GetControl<Text>("ThrowNum");
        // 从消息中心拿取消息
        EventCenter.GetInstance().AddEventListener<Collider2D[]>("强化物", (x) => { strengthen = x; });
        EventCenter.GetInstance().AddEventListener<Vector2>("PlayerPos", (x) =>
        {
            playerPos = x + new Vector2(0, .5f);
        });
        EventCenter.GetInstance().AddEventListener<Collider2D>("距离最近的敌人", (x) =>
        {
            if (x == null)
            {
                nearEnemy = null;
                return;
            }

            nearEnemy = x.gameObject;
        });
        EventCenter.GetInstance().AddEventListener<Collider2D>("附近的Npc", (x) =>
        {
            // TODO:  把<投掷>按键上的图片，改成<聊天>图片

            if (x != null)
            {
                ctrlType = CtrlType.打开商店;
                npcComponent = x.GetComponent<Npc>();
            }
            else
            {
                ctrlType = CtrlType.射击;
            }

        });
        // 计时器
        cdTimer = new Timer(throwSpeed, true);
        lastThrow = throwSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        // 根据队列数量修改text的文本
        if (strengthenQueue.Count < 1)
        {
            numText.text = "∞";
        }
        else
        {
            numText.text = strengthenQueue.Count + "/" + maxThingNum;
        }
        // 投掷速度修改时
        if (lastThrow != throwSpeed)
        {
            // 重置计时器
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
                SwitchThrowKeyAct();
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
        if (strengthen.Length < 1)
        {
            Debug.Log("拿了个空气！！");
            return;
        }

        Debug.Log("拿取强化物！！");
        // 一次只拿取一个，拿完跳出
        if (strengthenQueue.Count < maxThingNum)
        {
            BulletBag bb = strengthen[0].GetComponent<BulletBag>();
            bb.InitPos();
            strengthenQueue.Enqueue(bb);
            PoolMgr.GetInstance().PushObj(strengthen[0].name, strengthen[0].gameObject);
            Debug.Log(strengthenQueue.Count);
        }

    }
    /// <summary>
    /// 射击
    /// </summary>
    private void Shoot()
    {
        // 如果弹匣里没有，则进行基础攻击
        if (strengthenQueue.Count == 0)
        {
            // 基础攻击
            ThrowItemData data = GameTool.GetDicInfo(Datas.GetInstance().ThrowItemDataDic, 10001);
            ShootBase(data.path, data.speed);
        }
        // 否则，使用弹匣中第一个强化物
        else
        {
            BulletBag first = strengthenQueue.Dequeue();

            Debug.Log("强化射击！！        " + first);
            ThrowItemData tid = GameTool.GetDicInfo(Datas.GetInstance().ThrowItemDataDic, first.id);
            ShootBase(tid.path, tid.speed);
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
    /// 射击
    /// </summary>
    private void ShootBase(string name, float speed)
    {
        PoolMgr.GetInstance().GetObj(name, (x) =>
        {
            x.transform.position = playerPos;

            Rigidbody2D rg = x.GetComponent<Rigidbody2D>();

            // 有敌人就朝敌人发射
            if (nearEnemy != null)
            {
                enemyDir = (nearEnemy.transform.position - playerPos).normalized;
                EventCenter.GetInstance().EventTrigger<Vector2>("Joystick", Vector2.zero);

                Player.instance.Animator.SetFloat("AtkX", enemyDir.x);
                Player.instance.Animator.SetFloat("AtkY", enemyDir.y);
                // 播放挥砍动画
                Player.instance.Animator.Play("Atk");
                // 设置速度
                rg.velocity = speed * enemyDir;
                // 设置朝向
                x.transform.rotation = Quaternion.FromToRotation(Vector3.right, enemyDir);
            }

            x.GetComponent<ThrowItem>().ws = WhoShoot.Player;
        });
    }

    private void SwitchThrowKeyAct()
    {
        switch (ctrlType)
        {
            case CtrlType.射击:
                if (nearEnemy == null || !cdTimer.isTimeUp)
                {
                    Debug.Log("附近没怪 或者 CD没好");
                    return;
                }
                Shoot();
                break;
            case CtrlType.打开商店:
                Debug.Log("打开商店");
                //UIMgr.GetInstance().ShowPanel<ShopPanel>("ShopPanel", E_UI_Layer.Above);
                npcComponent.InitShop();
                break;
            case CtrlType.传送:
                break;
            default:
                break;
        }
    }
}
