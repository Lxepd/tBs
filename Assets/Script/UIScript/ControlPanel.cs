using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CtrlType
{
    ���,
    ���̵�,
    ����
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
    WeaponData weaponData;
    Timer shootTimer, ammunitionChangeTimer;

    private float currentBulletNum;

    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<Vector2>("PlayerPos", (x) =>
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
        EventCenter.GetInstance().AddEventListener<WeaponData>("ǹ֧����", (x) =>
        {
            weaponData = x;
            currentBulletNum = weaponData.bulletNum;
            ammunitionChangeTimer = new Timer(weaponData.ammunitionChangeTime, false, false);
            shootTimer = new Timer(x.shootNextTime, true);
        });
        EventCenter.GetInstance().AddEventListener<Collider2D>("������Npc", (x) =>
        {
            // TODO:  ��<Ͷ��>�����ϵ�ͼƬ���ĳ�<����>ͼƬ

            if (x != null)
            {
                ctrlType = CtrlType.���̵�;
                npcComponent = x.GetComponent<Npc>();
            }
            else
            {
                ctrlType = CtrlType.���;
            }
        });
        EventCenter.GetInstance().AddEventListener<bool>("�������", (x) =>
        {
            if (x)
            {
                SwitchBtoAct();
            }
        });

    }
    private void Update()
    {
        if (weaponData == null)
            return;

        if (currentBulletNum > 0)
        {
            GetControl<Image>("�ӵ�����").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            GetControl<Image>("�ӵ�����").fillAmount = currentBulletNum / weaponData.bulletNum;
        }
        else
        {
            GetControl<Image>("�ӵ�����").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 100 / 255f);
            GetControl<Image>("�ӵ�����").fillAmount = Mathf.Lerp(GetControl<Image>("�ӵ�����").fillAmount, weaponData.ammunitionChangeTime - ammunitionChangeTimer.nowTime, Time.deltaTime * 10f);
        }

        if(GetControl<Image>("�ӵ�����").fillAmount == 1)
        {
            currentBulletNum = weaponData.bulletNum;
        }
    }
    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "TakeBto":
                break;
            case "BagBto":
                OpenBag();
                break;
        }
    }
    private void Shoot()
    {
        PoolMgr.GetInstance().GetObj(weaponData.bulletPath, (x) =>
         {
             x.transform.position = gun.position;
             Rigidbody2D rg = x.GetComponent<Rigidbody2D>();

             Vector2 enemyDir = (nearEnemy.transform.position - PlayerPos).normalized;
             // �����ٶ�
             rg.velocity = weaponData.bulletSpeed * enemyDir;
             x.transform.rotation = Quaternion.FromToRotation(Vector3.right,enemyDir);

             ThrowItem ti = x.GetComponent<ThrowItem>();
             ti.ws = WhoShoot.Player;
             ti.hurt = weaponData.atk;
         });

        currentBulletNum = (currentBulletNum <= 0) ? 0 : currentBulletNum - 1;

        if(currentBulletNum == 0)
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
            case CtrlType.���:
                if (nearEnemy == null || gun.GetComponent<SpriteRenderer>().sprite == null || !shootTimer.isTimeUp || currentBulletNum == 0)
                    return;

                Shoot();
                break;
            case CtrlType.���̵�:
                npcComponent.InitShop();
                break;
        }
    }
}
