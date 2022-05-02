using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public enum TileType
{
    Load,
    Wall
}
public class TileSet : MonoBehaviour
{
    // 墙壁RuleTile
    [SerializeField] RuleTile wallRuleTile;
    // 非墙壁RuleTile
    [SerializeField] RuleTile roadRuleTile;
    // 瓦片地图
    [SerializeField] Tilemap tilemap;
    // 地图大小
    public int row = 10;
    public int col = 10;
    // 融合次数
    public int num = 0;

    TileType[,] mapArray;
    [SerializeField] public List<GameObject> monsterList = new List<GameObject>();
    bool isCreateTpPoint;

    public GameObject aaa;

    Timer check;
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<GameObject>("怪物表减少", (x) =>
        {
            monsterList.Remove(x);
        });
        EventCenter.GetInstance().AddEventListener<GameObject>("玩家物体", (x) =>
        {
            aaa = x;
        });

        check = new Timer(3f);
        mapArray = new TileType[row, col];
        SetMap();

    }
    private void FixedUpdate()
    {
        if (check.isTimeUp)
            CheckMonsterNum();
    }
    private void SetMap()
    {
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                if (i < 3 || i >= row - 3 || j < 3 || j >= col - 3)
                    mapArray[i, j] = TileType.Wall;
                else
                    //mapArray[i, j] = (TileType)Random.Range(0, System.Enum.GetNames(new TileType().GetType()).Length);
                    mapArray[i, j] = Random.Range(0, 101) < 40 ? TileType.Wall : TileType.Load;
            }
        }

        ReduceTile(num);
        CreateTile();

        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/出现点", (x) =>
        {
            x.transform.position = GetBarrierFreeArea();
            x.transform.SetParent(transform);

            aaa.transform.position = x.transform.position;
        });

        CreateMonsters(5);
        CreateNpc();
    }
    private void ReduceTile(int num)
    {
        int c = 0;
        while (c < num)
        {
            for (int i = 0; i < mapArray.GetLength(0); i++)
            {
                for (int j = 0; j < mapArray.GetLength(1); j++)
                {
                    if (i < 1 || i >= row - 1 || j < 1 || j >= col - 1)
                    {
                        mapArray[i, j] = TileType.Wall;
                        continue;
                    }

                    // 规则
                    mapArray[i, j] = (CheckNeighborWalls(i, j, 1) >= 5) ? TileType.Wall : TileType.Load;
                }
            }

            c++;
        }

    }
    private void CreateTile()
    {
        Vector3 size = new Vector3(0, 0, 0);
        for (int i = 0; i < mapArray.GetLength(0); i++)
        {
            for (int j = 0; j < mapArray.GetLength(1); j++)
            {
                if (mapArray[i, j] == TileType.Wall)
                {
                    tilemap.SetTile(new Vector3Int(i - (int)size.x, j - (int)size.y, 0), wallRuleTile);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(i - (int)size.x, j - (int)size.y, 0), roadRuleTile);
                }
            }
        }
    }
    private int CheckNeighborWalls(int x, int y, int t)
    {
        int count = 0;

        for (int i = x - t; i < x + t + 1; i++)
        {
            for (int j = y - t; j < y + t + 1; j++)
            {
                if (i < 0 || i >= row || j < 0 || j >= col)
                    continue;

                if (mapArray[i, j] == TileType.Wall)
                    count++;
            }
        }

        return count;
    }
    private void CheckMonsterNum()
    {
        if (monsterList.Count != 0 || isCreateTpPoint)
            return;

        isCreateTpPoint = true;

        UIMgr.GetInstance().ShowPanel<YWPanel>("YWPanel");

        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/传送点", (x) =>
        {
            x.transform.position = GetBarrierFreeArea();
            x.transform.SetParent(transform);
        });
    }
    private Vector2 GetBarrierFreeArea()
    {
        int cr, cc;
        do
        {
            cr = Random.Range(0, row);
            cc = Random.Range(0, col);
        } while (CheckNeighborWalls(cr, cc, 3) != 0);

        return new Vector2(cr, cc);
    }
    public static string GetRandomEnemyPath(bool normal = true)
    {
        int id;
        do
        {
            id = 15000 + Random.Range(1, Datas.GetInstance().EnemyDataDic.Count + 1);
        } while ((normal) ? Datas.GetInstance().EnemyDataDic[id].type == EnemyType.Boss : Datas.GetInstance().EnemyDataDic[id].type == EnemyType.小怪);

        return Datas.GetInstance().EnemyDataDic[id].path;
    }
    private void CreateMonsters(int num)
    {
        if(num<0)
        {
            return;
        }

        int monsterCount = 0;
        bool hasBoss = false;

        while (monsterCount < Mathf.Min(num,10))
        {
            if (LevelMgr.GetInstance().level % 5 == 0 && !hasBoss)
            {
                ResMgr.GetInstance().LoadAsync<GameObject>(GetRandomEnemyPath(false), (x) =>
                {
                    x.transform.position = GetBarrierFreeArea();
                    x.transform.SetParent(transform);
                    monsterList.Add(x);
                });

                hasBoss = true;
                monsterCount += 5;
            }

            ResMgr.GetInstance().LoadAsync<GameObject>(GetRandomEnemyPath(), (x) =>
            {
                x.transform.position = GetBarrierFreeArea();
                x.transform.SetParent(transform);
                monsterList.Add(x);
            });

            monsterCount++;
        }
    }
    private void CreateNpc()
    {
        if (Random.Range(0, 101) > 20)
            return;

        string path = Datas.GetInstance().NpcDataDic[13000 + Random.Range(1, Datas.GetInstance().NpcDataDic.Count + 1)].path;
        ResMgr.GetInstance().LoadAsync<GameObject>(path, (x) =>
         {
             x.transform.position = GetBarrierFreeArea();
             x.transform.SetParent(transform);
         });
    }
}
