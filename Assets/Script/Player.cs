using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2 dir;
    Rigidbody2D rg;

    public float speed=0;

    // Start is called before the first frame update
    void Start()
    {
        rg=GetComponent<Rigidbody2D>();
        // 获取消息中心中 Joystick 的消息，然后执行 委托
        EventCenter.GetInstance().AddEventListener<Vector2>("Joystick", CheckDirChange);
    }

    // Update is called once per frame
    void Update()
    {
        rg.velocity = dir*speed;
        CheckMissileScope();
    }

    private void CheckDirChange(Vector2 dir)
    {
        this.dir = dir;
    }

    private void CheckMissileScope()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 2), .5f);
        // 消息中心存储 <附近投掷物> 消息
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("Missile", cols);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, new Vector2(2, 2));
    }
}
