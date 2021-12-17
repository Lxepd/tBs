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
        // ��ȡ��Ϣ������ Joystick ����Ϣ��Ȼ��ִ�� ί��
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
        // ��Ϣ���Ĵ洢 <����Ͷ����> ��Ϣ
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("Missile", cols);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, new Vector2(2, 2));
    }
}
