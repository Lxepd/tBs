using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : BehaviorBase
{
    protected EnemyData data;
    public EnemyData Data { get => data; }

    public Rigidbody2D Rg { get => rg; }
    public Animator Animator { get => anim; }
    public float AnimaTime { get => nowAnimatorTime; }
    // 有限状态机
    [HideInInspector] protected StateMachine machine;

    // 玩家子弹来向
    [HideInInspector] protected Vector2 nearThrow;

    public bool stopMono;

    protected Timer forceStopTimer;
    public Collider2D player;

    protected int id;
    protected bool isHit;
    public bool Hit { get => isHit; set => isHit = value; }

    protected Image hp;
    protected float currentHp;
    protected float nowAnimatorTime;

    // 注册状态计时器
    public Timer moveTimer, atkTimer, HitTimer, DeadTimer;
    public bool playerIsDeath;

    protected override void Start()
    {
        base.Start();

        data = Datas.GetInstance().EnemyDataDic[id];

        hp = GameTool.FindTheChild(gameObject, "Hp").GetComponent<Image>();
        hp.fillAmount = (currentHp = data.hp) / data.hp;

        // 获取<敌人扣血>的消息
        EventCenter.GetInstance().AddEventListener<float>("敌人扣血", (x) =>
        {
            if (nearThrow == Vector2.zero)
                return;

            currentHp -= (int)x;
            isHit = true;
        });
        EventCenter.GetInstance().AddEventListener("玩家死亡", () =>
        {
            playerIsDeath = true;
        });
    }
    protected virtual void Update()
    {
        if (stopMono)
            return;

        nowAnimatorTime = anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
        // 状态选择
        CheckState();
        // 检查玩家攻击
        FindThrowDir();
        // 有限状态机更新
        machine.Update();

        hp.fillAmount = Mathf.Lerp(hp.fillAmount, currentHp / data.hp, Time.deltaTime * 10f);
    }
    // 改变朝向
    protected override void Rotate(Vector3 dir)
    {
        Vector3 playerDir = (dir - transform.position).normalized;
        if (playerDir.x <= 0)
        {
            transform.localScale = new Vector2(-1, transform.localScale.y);
            hp.fillOrigin = (int)Image.OriginHorizontal.Right;
        }
        else
        {
            transform.localScale = new Vector2(1, transform.localScale.y);
            hp.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
    }
    // 状态改变
    public virtual void CheckState()
    {
        player = Physics2D.OverlapCircle(transform.position, data.moveLen, LayerMask.GetMask("玩家"));

    }
    /// <summary>
    /// 寻找来向投掷物
    /// </summary>
    private void FindThrowDir()
    {
        LayerMask mask = LayerMask.GetMask("基础攻击") | LayerMask.GetMask("强化攻击");
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 2f, mask);
        if (cols.Length == 0)
        {
            nearThrow = Vector2.zero;
            return;
        }

        GameTool.QuickSortArray(transform.position, cols, 0, cols.Length - 1);
        nearThrow = (transform.position - cols[0].transform.position).normalized;
    }
    private void OnDrawGizmosSelected()
    {
        if (data == null)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, data.hitLen);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, data.moveLen);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, data.atkLen);
    }
}

