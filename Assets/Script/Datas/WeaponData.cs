using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    ��ǹ,
    ��ǹ,
    �ѻ�ǹ
}
public class WeaponData
{
    public int id; // id
    public string spritePath; // ǹ֧ͼƬ·��
    public string bulletPath; // �ӵ�Ԥ����·��
    public Weapon type; // ǹ֧����
    public int bulletNum; // �ӵ�����
    public float ammunitionChangeTime; // װ��ʱ��
    public float atk; // ǹ֧������
    public float bulletSpeed; // ����
    public float shootLen; // ǹ֧�������
    public float shootNextTime; // ������
}