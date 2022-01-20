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
    // ǽ��RuleTile
    [SerializeField] RuleTile wallRuleTile;
    // ��ǽ��RuleTile
    [SerializeField] RuleTile roadRuleTile;
    // ��Ƭ��ͼ
    [SerializeField] Tilemap tilemap;
    // ��ͼ��С
    public int row = 10;
    public int col = 10;
    // �ںϴ���
    public int num = 0;

    TileType[,] mapArray;
    [SerializeField] public List<GameObject> monsterList = new List<GameObject>();
    bool isCreateTpPoint;

    int bossNum, unBossNum;
    public GameObject aaa;

    Timer check;
    private void Start()
    {
        EventCenter.GetInstance().AddEventListener<GameObject>("��������", (x) =>
        {
            monsterList.Remove(x);
        });
        EventCenter.GetInstance().AddEventListener<GameObject>("�������", (x) =>
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

        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/���ֵ�", (x) =>
        {
            x.transform.position = GetBarrierFreeArea();
            x.transform.SetParent(transform);

            aaa.transform.position = x.transform.position;
        });

        CreateMonsters(10);
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

                    // ����
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
        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/���͵�", (x) =>
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
    private void CreateMonsters(int num)
    {
        int monsterCount = 0;

        while (monsterCount < num)
        {
            if (LevelMgr.GetInstance().level % 5 == 0)
            {
                if (unBossNum <= 4)
                {
                    unBossNum++;
                    ResMgr.GetInstance().LoadAsync<GameObject>(GameTool.GetRandomEnemyPath(), (x) =>
                    {
                        x.transform.position = GetBarrierFreeArea();
                        x.transform.SetParent(transform);
                        monsterList.Add(x);
                    });

                    monsterCount++;
                }

                if(bossNum <1)
                {
                    bossNum++;
                    ResMgr.GetInstance().LoadAsync<GameObject>(GameTool.GetRandomEnemyPath(false), (x) =>
                    {
                        x.transform.position = GetBarrierFreeArea();
                        x.transform.SetParent(transform);
                        monsterList.Add(x);
                    });

                    monsterCount+=2;
                }
            }
            else
            {
                ResMgr.GetInstance().LoadAsync<GameObject>(GameTool.GetRandomEnemyPath(), (x) =>
                {
                    x.transform.position = GetBarrierFreeArea();
                    x.transform.SetParent(transform);
                    monsterList.Add(x);
                });

                monsterCount++;
            }

        }
    }
}
