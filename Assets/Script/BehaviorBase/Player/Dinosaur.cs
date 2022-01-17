using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinosaur : Player
{
    Transform gun;
    float angle;
    protected override void Update()
    {
        base.Update();
        Rotate(angle);
    }
    protected override void Rotate(float angle)
    {
        if (nearEnemy == null)
        {
            if(dir.x<0)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            if (angle > 90 || angle < -90)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
            }
        }
    }
    protected override void RoleArms(Transform tran)
    {
        gun = GameTool.FindTheChild(gameObject, "Gun");

        Vector3 gunDir = (tran.position - transform.position).normalized * transform.localScale.x;
        float angle = Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg;
        this.angle = angle;
        Rotate(angle);
        angle = Mathf.Clamp(angle, -90, 90);      
        gun.eulerAngles = new Vector3(0, 0, angle);

    }
}
