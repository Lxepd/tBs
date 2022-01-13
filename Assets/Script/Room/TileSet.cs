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
    [SerializeField] RuleTile wallRuleTile;
    [SerializeField] RuleTile roadRuleTile;
    [SerializeField] Tilemap tilemap;

    public int row = 10;
    public int col = 10;
    public int num = 0;

    TileType[,] mapArray;
    [SerializeField] public List<GameObject> monsterList = new List<GameObject>();
    bool isCreateTpPoint;

    private void Start()
    {
        mapArray = new TileType[row, col];
        SetMap();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            ReduceTile(num);
            CreateTile();
        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void FixedUpdate()
    {
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
             int cr, cc;
             do
             {
                 cr = Random.Range(-row / 2, row / 2);
                 cc = Random.Range(-col / 2, col / 2);

             } while (CheckNeighborWalls(cr, cc, 1) == 0);

             x.transform.position = new Vector2(cr, cc);
         });
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
                    mapArray[i, j] = (CheckNeighborWalls(i, j, 1) >= 4) ? TileType.Wall : TileType.Load;
                }
            }

            c++;
        }

    }
    private void CreateTile()
    {
        Vector3 size = new Vector3(row / 2, col / 2, 0);
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

        return count - 1;
    }
    private void CheckMonsterNum()
    {
        if (monsterList.Count != 0 || isCreateTpPoint)
            return;

        ResMgr.GetInstance().LoadAsync<GameObject>("Prefabs/传送点", (x) =>
        {
            int cr, cc;
            do
            {
                cr = Random.Range(-row / 2, row / 2);
                cc = Random.Range(-col / 2, col / 2);

            } while (CheckNeighborWalls(cr, cc, 1) != 0);

            x.transform.position = new Vector2(cr, cc);
            isCreateTpPoint = true;
        });
    }
}
