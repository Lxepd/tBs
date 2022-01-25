using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    None,
    һ��������Ʒ,
    ���ظ�ʹ������Ʒ,
    ������Ʒ
}
public enum ItemActType
{
    None,
    Ѫ���ָ�,
    ����ǿ��
}
public class ItemData
{
    public int id; // id
    public string name; // ��
    public string path; // ͼƬ·��
    public string tips; // ˵��
    public bool canSuperPosition; // �ܷ����
    public int maxNum; // �������
    public int cost;
    public ItemType itemType; // ��������
    public ItemActType actType; // ���߹���
    public int recovery; // �ָ�
    public float increaseAttackSpeed; // ���ӹ���
}
