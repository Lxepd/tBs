using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CtrlType
{
    打开商店
}
public class ControlPanel : UIBase
{
    // 玩家位置
    private Vector3 PlayerPos;
    // 最近的敌人
    private GameObject nearEnemy;
    private Transform gun;

    CtrlType ctrlType;
    Npc npcComponent;
    Timer shootTimer, ammunitionChangeTimer, reShootNextTime;

    bool isAddBullet;

    private float currentBulletNum;

    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<Vector2>("射击起点", (x) =>
        {
            PlayerPos = x;
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
        EventCenter.GetInstance().AddEventListener<Transform>("是否有枪支", (x) =>
        {
            gun = x;
        });
        EventCenter.GetInstance().AddEventListener("角色数据初始", () =>
        {
            currentBulletNum = Datas.GetInstance().weaponData.bulletNum;
            ammunitionChangeTimer = new Timer(Datas.GetInstance().weaponData.ammunitionChangeTime, false, false);
            shootTimer = new Timer(Datas.GetInstance().weaponData.shootNextTime, true);
        });
        EventCenter.GetInstance().AddEventListener<bool>("枪支数据改变", (x) =>
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
        EventCenter.GetInstance().AddEventListener<Collider2D>("附近的Npc", (x) =>
        {
            // TODO:  把<投掷>按键上的图片，改成<聊天>图片

            if (x != null)
            {
                ctrlType = CtrlType.打开商店;
                npcComponent = x.GetComponent<Npc>();

            }

        });
        EventCenter.GetInstance().AddEventListener<bool>("射击长按", (x) =>
        {
            if (x)
            {
                if (nearEnemy == null || gun.GetComponent<SpriteRenderer>().sprite == null || !shootTimer.isTimeUp || currentBulletNum <= 0)
                    return;

                Shoot();
            }
        });
        EventCenter.GetInstance().AddEventListener("角色恢复", () =>
        {
            GetControl<Image>("子弹数量").fillAmount = 1;
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
            EventCenter.GetInstance().EventTrigger<bool>("枪支数据改变", false);
        }

        if (currentBulletNum > 0)
        {
            GetControl<Image>("子弹数量").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            GetControl<Image>("子弹数量").fillAmount = currentBulletNum / Datas.GetInstance().weaponData.bulletNum;
        }
        else
        {
            GetControl<Image>("子弹数量").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 100 / 255f);
            Debug.Log(GetControl<Image>("子弹数量"));
            GetControl<Image>("子弹数量").fillAmount = Mathf.Lerp(GetControl<Image>("子弹数量").fillAmount, Datas.GetInstance().weaponData.ammunitionChangeTime - ammunitionChangeTimer.nowTime, Time.deltaTime * 10f);
        }

        if(GetControl<Image>("子弹数量").fillAmount == 1)
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
             // 设置速度
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
    /// 打开背包
    /// </summary>
    private void OpenBag()
    {
        Debug.Log("打开背包！！");
        UIMgr.GetInstance().ShowPanel<BagPanel>("BagPanel", E_UI_Layer.Above);
    }
    private void SwitchBtoAct()
    {
        switch (ctrlType)
        {
            case CtrlType.打开商店:
                npcComponent.OpenShop();
                break;
        }
    }
}
