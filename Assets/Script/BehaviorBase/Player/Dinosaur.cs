using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinosaur : Player
{
    float angle;

    protected override void Update()
    {
        base.Update();

        Rotate(angle);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
    protected override void Rotate(float angle)
    {
        if (nearEnemy == null)
        {
            if(dir.x<0)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
            else if(dir.x > 0)
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
    protected override void RoleArms()
    {
        Vector3 gunDir = (nearEnemy == null) ? Vector3.right : (nearEnemy.transform.position - transform.position).normalized * transform.localScale.x;
        float angle = Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg;
        this.angle = angle;
        Rotate(angle);
        angle = Mathf.Clamp(angle, -90, 90);      
        gun.eulerAngles = new Vector3(0, 0, angle);

    }
}
