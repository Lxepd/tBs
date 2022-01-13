using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    public RoomData data;
    public Tilemap map;

    void Start()
    {
        if (data.monsters == null)
        {
            data.monsters = new List<GameObject>();
        }

        Tilemap map = GameTool.FindTheChild(gameObject, "地面").GetComponent<Tilemap>();
        Vector3 mapSize = map.size;

        int num = 0;
        while (num < 10)
        {
            int ranId = Random.Range(15001, 15001);
            Debug.Log(ranId);
            GameObject enemy = Instantiate(Resources.Load<GameObject>(GameTool.GetDicInfo(Datas.GetInstance().EnemyDataDic, ranId).path));
            enemy.transform.position = new Vector3(Random.Range(0, mapSize.x) - mapSize.x / 2, Random.Range(0, mapSize.y) - mapSize.y / 2);
            data.monsters.Add(enemy);
            num++;
        }
        //Player.instance.transform.position = transform.Find("出现点").position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
