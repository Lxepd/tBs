using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : UIBase
{
    // Ͷ�����б�
    private Collider2D[] missiles;
    // Ͷ��������
    private Text numText;
    // Ͷ���ﵯϻ
    // ����ȡ�� Ͷ������� ����
    private Queue<GameObject> throwMagazine;
    // ��ȡͶ������������
    private int maxThingNum = 5;

    // ��ǰ���λ��
    private Vector3 playerPos;
    // ����ĵ���
    private GameObject nearEnemy;
    // ���˷�������
    private Vector3 enemyDir;
    TimeAction timeAction = new TimeAction();
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
        // ����Ϣ������ȡ��Ϣ
        EventCenter.GetInstance().AddEventListener<Collider2D[]>("����Ͷ����", (x) => { missiles = x; });
        EventCenter.GetInstance().AddEventListener<Vector2>("PlayerPos", (x) => { playerPos = x; });
        EventCenter.GetInstance().AddEventListener<Collider2D>("��������ĵ���", (x) =>
        {
            if (x == null)
            {
                nearEnemy = null;
                return;
            }

            nearEnemy = x.gameObject;
        });
        // ע��Ͷ���ļ�ʱ���¼�
        timeAction.RegisterTimerTask("��ʯ��", .5f, ThrowBase);

    }

    // Update is called once per frame
    void Update()
    {
        timeAction.OnUpdate();

    }
    private void PushQueue(GameObject thing)
    {
        if (throwMagazine.Count < maxThingNum)
            throwMagazine.Enqueue(thing);
    }

    /// <summary>
    /// ��ť����¼�ע��
    /// </summary>
    /// <param name="btnName">��ť��</param>
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
    /// ��ȡ
    /// </summary>
    private void TakeThing()
    {
        Debug.Log("��ȡ  Ͷ�����");
        // �������� ��ҽ�ɫ������ collider��
        foreach (var item in missiles)
        {
            // һ��ֻ��ȡһ������������
            PushQueue(item.gameObject);
            item.gameObject.SetActive(false);
            Debug.Log(throwMagazine.Count);
            return;
        }
    }
    /// <summary>
    /// Ͷ��
    /// </summary>
    private void ThrowThing()
    {
        if (TimeAction.GetInstance().isTimer)
            return;

        if (throwMagazine.Count == 0)
        {
            Debug.Log("û�и������Ķ���������ֻ��Ͷ��ʯ����");
            //TimerAction.GetInstance().PlayerAction("��ʯ��");
            timeAction.PlayTimerTask("��ʯ��");

            return;
        }

        GameObject th = throwMagazine.Dequeue();

        Debug.Log("����  Ͷ�����        " + th.name);
        PoolMgr.GetInstance().PushObj(th.name,th);
    }
    /// <summary>
    /// �򿪱���
    /// </summary>
    private void OpenBag()
    {
        Debug.Log("�򿪱�������");
        UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
    }
    /// <summary>
    /// ����Ͷ����---ʯ��Ͷ��
    /// </summary>
    private void ThrowBase()
    {
        PoolMgr.GetInstance().GetObj("Prefabs/ʯ��", (x) =>
        {
            x.transform.position = playerPos;

            Rigidbody2D rg = x.GetComponent<Rigidbody2D>();
            // Ĭ�����ҷ���
            if (nearEnemy == null)
            {
                rg.velocity = 5 * Vector2.right;
            }
            // �е��˾ͳ����˷���
            else
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
