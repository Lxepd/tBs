using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public int id;
    private NpcData data;
    // ���۵ĵ��߱�
    public List<NpcSell> itemList = new List<NpcSell>();

    public float checkRadius = 3f;

    private void Start()
    {
        data = GameMgr.GetInstance().GetNpcInfo(id);
    }
    private void Update()
    {

    }
}
