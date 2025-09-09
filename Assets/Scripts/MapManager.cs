using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] outWallArray;

    public GameObject[] floorArray;
    public GameObject[] wallArray;
    public GameObject[] foodArray;
    public GameObject[] enemyArray;
    public GameObject exit;

    public int rows = 10;

    public int cols = 10;

    private Transform mapHolder;

    private List<Vector2> positionList = new List<Vector2>();

    public int minCountWall = 3;

    public int maxCountWall = 8;

    private GameManager gameManager;
    // Start is called before the first frame update
   

    // Update is called once per frame
    
    //initialize the map
    public void InitMap()
    {
        gameManager = this.GetComponent<GameManager>();
        mapHolder = new GameObject("map").transform;
        //创建外墙和内部地砖
        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (i == 0 || j == 0 || i == cols - 1 || j == rows - 1)
                {
                    int index = Random.Range(0, outWallArray.Length);
                   GameObject go= GameObject.Instantiate(outWallArray[index],new Vector3(i,j,0),Quaternion.identity) as GameObject;
                   go.transform.SetParent(mapHolder);
                }
                else
                {
                    int index = Random.Range(0, floorArray.Length);
                   GameObject go = GameObject.Instantiate(floorArray[index],new Vector3(i,j,0),Quaternion.identity) as GameObject;
                   go.transform.SetParent(mapHolder);
                    
                }
                
            }
        }
        positionList.Clear();
        for (int x = 2; x < cols-2; x++)
        {
            for (int y = 2; y < rows-2; y++)
            {
                positionList.Add(new Vector2(x,y));
                
            }
        }
        
        //创建地图内部的障碍物，敌人，食物
        int wallCount = Random.Range(minCountWall, maxCountWall + 1);//障碍物个数2-8
        for (int i = 0; i < wallCount; i++)
        {
            //shengchenzhangaiwu
            int positionIndex = Random.Range(0, positionList.Count);
            Vector2 pos = positionList[positionIndex];
            positionList.RemoveAt(positionIndex);//移除该位置
            //随机取得障碍物
            int wallIndex = Random.Range(0, wallArray.Length);
            GameObject go1 = GameObject.Instantiate(wallArray[wallIndex],pos,Quaternion.identity) as GameObject;
            go1.transform.SetParent(mapHolder);

        }
        //创建食物，至少两个，关卡越高食物越多 2-level*2
        int foodCount = Random.Range(2, gameManager.level * 2 + 1);
        for (int i = 0; i < foodCount; i++)
        {
            Vector2 pos = RandomPosition();
            GameObject foodPrefab = RandomPrefab(foodArray);
            GameObject go2 =  Instantiate(foodPrefab, pos, Quaternion.identity) as GameObject;
            go2.transform.SetParent(mapHolder);

        }
        //创建敌人,个数：level/2
        int enemyCount = gameManager.level / 2;
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 pos = RandomPosition();
            GameObject enemyPrefab = RandomPrefab(enemyArray);
            GameObject go3 = Instantiate(enemyPrefab,pos,Quaternion.identity) as GameObject;
            go3.transform.SetParent(mapHolder);
        }
        //set up exist
       GameObject go4 = Instantiate(exit, new Vector2(cols - 2, rows - 2), Quaternion.identity) as GameObject;
       go4.transform.SetParent(mapHolder);





    }
    private void InstantiateItems( int count,GameObject[] prefabs){}

    private Vector2 RandomPosition()
    {
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[positionIndex];
        positionList.RemoveAt(positionIndex);
        return pos;
    }

    private GameObject RandomPrefab(GameObject[] prefabs)
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }
}
