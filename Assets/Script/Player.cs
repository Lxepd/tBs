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
        EventCenter.GetInstance().EventTrigger<Vector2>("PlayerPos", transform.position);

        rg.velocity = dir*speed;
        CheckMissileScope();
        FindProximityOfEnemy();
    }
    /// <summary>
    /// ��ȡҡ�˷�������
    /// </summary>
    /// <param name="dir"></param>
    private void CheckDirChange(Vector2 dir)
    {
        this.dir = dir;
    }
    /// <summary>
    /// ��鸽����û�г���Ͷ����
    /// </summary>
    private void CheckMissileScope()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, new Vector2(2, 2), .5f,LayerMask.GetMask("Ͷ����"));
        // ��Ϣ���Ĵ洢 <����Ͷ����> ��Ϣ
        EventCenter.GetInstance().EventTrigger<Collider2D[]>("����Ͷ����", cols);
    }
    /// <summary>
    /// Ѱ�������������һ������
    /// </summary>
    private void FindProximityOfEnemy()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 3f, LayerMask.GetMask("����"));

        QuickSortArray(cols, 0, cols.Length - 1);

        Debug.Log(cols[0].name);
        // ��Ϣ���Ĵ洢 <��������ĵ���> ��Ϣ
        EventCenter.GetInstance().EventTrigger<Collider2D>("��������ĵ���", cols[0]);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 3f);
    }
    /// <summary>
    /// ��������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array">��������</param>
    /// <param name="start">�������</param>
    /// <param name="end">�����յ�</param>
    public void QuickSortArray(Collider2D[] array,int start, int end)
    {
        // ������ >= �յ㣬����
        if (start >= end)
            return;

        int left = start, right = end;
        float m = GetDis(array[start]);
        Collider2D temp = array[start];

        while (left<right)
        {
            while (left < right && GetDis(array[right]) >= m)
            {
                right--;
            }

            array[left] = array[right];

            while (left < right && GetDis(array[left]) <= m)
            {
                left++;
            }

            array[right] = array[left];
        }

        array[right] = temp;

        QuickSortArray(array, start, left - 1);
        QuickSortArray(array, left + 1, end);
    }

    public float GetDis(Collider2D array)
    {
        return Vector2.Distance(array.gameObject.transform.position, transform.position);
    }
}
