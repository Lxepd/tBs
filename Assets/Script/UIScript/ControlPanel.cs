using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CtrlType
{
    ���̵�
}
public class ControlPanel : UIBase
{
    // ���λ��
    private Vector3 PlayerPos;
    // ����ĵ���
    private GameObject nearEnemy;
    private Transform gun;

    CtrlType ctrlType;
    Npc npcComponent;
    Timer shootTimer, ammunitionChangeTimer, reShootNextTime;

    bool isAddBullet;

    private float currentBulletNum;

    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<Vector2>("������", (x) =>
        {
            PlayerPos = x;
        });
        EventCenter.GetInstance().AddEventListener<Collider2D>("��������ĵ���", (x) =>
        {
            if (x == null)
            {
                nearEnemy = null;
                return;
            }

            nearEnemy = x.gameObject;
        });
        EventCenter.GetInstance().AddEventListener<Transform>("�Ƿ���ǹ֧", (x) =>
        {
            gun = x;
        });
        EventCenter.GetInstance().AddEventListener("��ɫ���ݳ�ʼ", () =>
        {
            currentBulletNum = Datas.GetInstance().weaponData.bulletNum;
            ammunitionChangeTimer = new Timer(Datas.GetInstance().weaponData.ammunitionChangeTime, false, false);
            shootTimer = new Timer(Datas.GetInstance().weaponData.shootNextTime, true);
        });
        EventCenter.GetInstance().AddEventListener<bool>("ǹ֧���ݸı�", (x) =>
        {
        if (x)
        {
            currentBulletNum = Datas.GetInstance().weaponData.bulletNum;
            ammunitionChangeTimer = new Timer(Datas.GetInstance().weaponData.ammunitionChangeTime, false, false);
        }

            float atkSp = Mathf.Max(Datas.GetInstance().weaponData.shootNextTime - (Datas.GetInstance().itemAddAtkSpd + Datas.GetInstance().YWAddAtkSpd), 0);
            shootTimer = new Timer(atkSp, true);

            if(Datas.GetInstance().isEatItem)
            {
                reShootNextTime = new Timer(Datas.GetInstance().itemReShootTimer);
            }
        });
        EventCenter.GetInstance().AddEventListener<Collider2D>("������Npc", (x) =>
        {
            // TODO:  ��<Ͷ��>�����ϵ�ͼƬ���ĳ�<����>ͼƬ

            if (x != null)
            {
                ctrlType = CtrlType.���̵�;
                npcComponent = x.GetComponent<Npc>();

            }

        });
        EventCenter.GetInstance().AddEventListener<bool>("�������", (x) =>
        {
            if (x)
            {
                if (nearEnemy == null || gun.GetComponent<SpriteRenderer>().sprite == null || !shootTimer.isTimeUp || currentBulletNum <= 0)
                    return;

                Shoot();
            }
        });
        EventCenter.GetInstance().AddEventListener("��ɫ�ָ�", () =>
        {
            GetControl<Image>("�ӵ�����").fillAmount = 1;
        });

    }
    private void Update()
    {
        if (Datas.GetInstance().weaponData == null)
            return;

        if (reShootNextTime!=null && reShootNextTime.isTimeUp && Datas.GetInstance().isEatItem)
        {
            Datas.GetInstance().isEatItem = false;
            reShootNextTime = null;
            Datas.GetInstance().itemAddAtkSpd = 0;
            EventCenter.GetInstance().EventTrigger<bool>("ǹ֧���ݸı�", false);
        }

        if (currentBulletNum > 0)
        {
            GetControl<Image>("�ӵ�����").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            GetControl<Image>("�ӵ�����").fillAmount = currentBulletNum / Datas.GetInstance().weaponData.bulletNum;
        }
        else
        {
            GetControl<Image>("�ӵ�����").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 100 / 255f);
            Debug.Log(GetControl<Image>("�ӵ�����"));
            GetControl<Image>("�ӵ�����").fillAmount = Mathf.Lerp(GetControl<Image>("�ӵ�����").fillAmount, Datas.GetInstance().weaponData.ammunitionChangeTime - ammunitionChangeTimer.nowTime, Time.deltaTime * 10f);
        }

        if(GetControl<Image>("�ӵ�����").fillAmount == 1)
        {
            currentBulletNum = Datas.GetInstance().weaponData.bulletNum;
        }


    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "TakeBto":
                SwitchBtoAct();
                break;
            case "BagBto":
                OpenBag();
                break;
        }
    }
    private void Shoot()
    {
        PoolMgr.GetInstance().GetObj(Datas.GetInstance().weaponData.bulletPath, (x) =>
         {
             x.transform.position = gun.position + new Vector3(0,.45f,0);
             Rigidbody2D rg = x.GetComponent<Rigidbody2D>();

             Vector3 randomPosOff = new Vector3(0, Random.Range(-1,2), 0);
             Vector2 enemyDir = (nearEnemy.transform.position + randomPosOff / 2 - PlayerPos).normalized;
             // �����ٶ�
             rg.velocity = Datas.GetInstance().weaponData.bulletSpeed * enemyDir;
             x.transform.rotation = Quaternion.FromToRotation(Vector3.right,enemyDir);

             ThrowItem ti = x.GetComponent<ThrowItem>();
             ti.ws = WhoShoot.Player;
             ti.hurt = Datas.GetInstance().weaponData.atk;
         });

        currentBulletNum = (currentBulletNum <= 0) ? 0 : currentBulletNum - 1;

        if (currentBulletNum <= 0)
        {
            ammunitionChangeTimer.Start();
        }
    }
    /// <summary>
    /// �򿪱���
    /// </summary>
    private void OpenBag()
    {
        Debug.Log("�򿪱�������");
        UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
    }
    private void SwitchBtoAct()
    {
        switch (ctrlType)
        {
            case CtrlType.���̵�:
                npcComponent.OpenShop();
                break;
        }
    }
}
