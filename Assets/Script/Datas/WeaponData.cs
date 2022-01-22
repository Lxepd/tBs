using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    手枪,
    步枪,
    狙击枪
}
public class WeaponData
{
    public int id; // id
    public string spritePath; // 枪支图片路径
    public string bulletPath; // 子弹预制体路径
    public Weapon type; // 枪支类型
    public int bulletNum; // 子弹数量
    public float ammunitionChangeTime; // 装弹时间
    public float atk; // 枪支攻击力
    public float bulletSpeed; // 弹速
    public float shootLen; // 枪支射击距离
    public float shootNextTime; // 射击间隔
}