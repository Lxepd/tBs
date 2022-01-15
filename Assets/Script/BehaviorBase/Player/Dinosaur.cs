using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dinosaur : Player
{
    Transform gun;
    protected override void Rotate(float dir)
    {
        if (dir > 90 || dir < -90)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
        }
    }
    protected override void RoleArms(Transform tran)
    {
        gun = GameTool.FindTheChild(gameObject, "Gun");

        Vector3 gunDir = (tran.position - transform.position).normalized * transform.localScale.x;
        float angle = Mathf.Atan2(gunDir.y, gunDir.x) * Mathf.Rad2Deg;

        Rotate(angle);
        angle = Mathf.Clamp(angle, -90, 90);      
        gun.eulerAngles = new Vector3(0, 0, angle);

    }
}
