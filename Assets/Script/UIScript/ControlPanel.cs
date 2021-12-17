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
        EventCenter.GetInstance().AddEventListener<Collider2D[]>("Missile", (x) => { missiles = x; });
        EventCenter.GetInstance().AddEventListener<Vector2>("PlayerPos", (x)=> { playerPos = x; });
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
            // �ų����
            if (item.CompareTag("Ͷ����"))
            {
                // һ��ֻ��ȡһ������������
                PushQueue(item.gameObject);
                item.gameObject.SetActive(false);
                Debug.Log(throwMagazine.Count);
                return;
            }
        }
    }
    /// <summary>
    /// Ͷ��
    /// </summary>
    private void ThrowThing()
    {
        if (throwMagazine.Count == 0)
        {

            Debug.Log("û�и������Ķ���������ֻ��Ͷ��ʯ����");

            PoolMgr.GetInstance().GetObj("Prefabs/ʯ��", (x) =>
            {
                TimerAction.GetInstance().PlayerAction("��ʯͷ", .5f, () =>
                {
                    x.transform.position = playerPos;
                    Rigidbody2D rg = x.GetComponent<Rigidbody2D>();
                    rg.velocity = Vector2.right * 5;

                    if (x.GetComponent<ThrowItem>() == null)
                    {
                        x.AddComponent<ThrowItem>();
                    }

                });
            });

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


}
