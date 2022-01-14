using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    С��,
    Boss
}
/// <summary>
/// ��������
/// </summary>
public class EnemyData
{
    public int id; // ID
    public string name; // ��
    public EnemyType type;
    public string path; // Ԥ����λ��
    public string tips; // ˵��
    public float hp; // Ѫ��
    public float speed;
    public float atk; // �˺�
    public float hitLen; //�ܻ���ⷶΧ
    public float moveLen; // �ƶ���ⷶΧ
    public float atkLen; // ������ⷶΧ
}
