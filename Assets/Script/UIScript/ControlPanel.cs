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
    // 玩家位置
    private Vector3 PlayerPos;
    // 最近的敌人
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
        EventCenter.GetInstance().AddEventListener<WeaponData>("枪支数据", (x) =>
        {
            weaponData = x;
            currentBulletNum = weaponData.bulletNum;
            ammunitionChangeTimer = new Timer(weaponData.ammunitionChangeTime, false, false);
            shootTimer = new Timer(x.shootNextTime, true);
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
        EventCenter.GetInstance().AddEventListener<bool>("射击长按", (x) =>
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
            GetControl<Image>("子弹数量").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
            GetControl<Image>("子弹数量").fillAmount = currentBulletNum / weaponData.bulletNum;
        }
        else
        {
            GetControl<Image>("子弹数量").color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 100 / 255f);
            GetControl<Image>("子弹数量").fillAmount = Mathf.Lerp(GetControl<Image>("子弹数量").fillAmount, weaponData.ammunitionChangeTime - ammunitionChangeTimer.nowTime, Time.deltaTime * 10f);
        }

        if(GetControl<Image>("子弹数量").fillAmount == 1)
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
             // 设置速度
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
            case CtrlType.射击:
                if (nearEnemy == null || gun.GetComponent<SpriteRenderer>().sprite == null || !shootTimer.isTimeUp || currentBulletNum == 0)
                    return;

                Shoot();
                break;
            case CtrlType.打开商店:
                npcComponent.InitShop();
                break;
        }
    }
}
