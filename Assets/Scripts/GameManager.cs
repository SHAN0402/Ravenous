using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }
    public int level = 1;

    public int food = 100;
    public List<Enemy> enemyList = new List<Enemy>();
    
    private bool sleep = true;
    public bool isEnd = false;


    private Image dayImage;
    private Text dayText;
    
    private Text foodText;
    private Text failText;
    private Player player;
    private MapManager _mapManager;

    public AudioClip dieClip;
    
   
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitGame();

    }

    void InitGame()
    {//初始化地图
        _mapManager = GetComponent<MapManager>();
        _mapManager.InitMap();
        
        //初始化UI
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        UpdateFoodText(0);
        failText = GameObject.Find("FailText").GetComponent<Text>();
        failText.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        dayImage = GameObject.Find("DayImage").GetComponent<Image>();
        dayText = GameObject.Find("DayText").GetComponent<Text>();
        dayText.text = "Day " + level;
        Invoke("HideBackGround",1);

        //参数初始化
        isEnd = false;
        enemyList.Clear();
        

    }
    

    void UpdateFoodText(int foodchange)
    {
      
        if (foodchange == 0)
        {
             foodText.text = "Food:" + food;
            
        }
        else
        {
        string str = "";
        if (foodchange < 0)
        {
            str = foodchange.ToString();
        }
        else
        {
            str = "+ " + foodchange;
           
        }
        
        foodText.text = str + " Food:" + food;
      
        }
    }

    public void ReduceFood(int count)
    {
        food -= count;
        UpdateFoodText(-count);
        if (food <= 0)
        {
            failText.enabled = true;
            AudioManager.Instance.stopMusic();
            AudioManager.Instance.RandomPlay(dieClip);
        }
    }

    public void AddFood(int count)
    {
        food += count;
        UpdateFoodText(count);
    }

    public void OnPlayerMove()
    {
        if (sleep==true)
        {
            sleep = false;
        }
        else
        {
            foreach (var enemy in enemyList)
            {
                enemy.Move();
            }
            sleep = true;
        }
        //是否到达终点
        if (player.targetPos.x == _mapManager.cols - 2 && player.targetPos.y == _mapManager.rows - 2)
        {
            isEnd = true;
            
            //加载下一个关卡
           Application.LoadLevel(Application.loadedLevel);

        }
    }

    void OnLevelWasLoaded(int sceneLevel)
    {
        level++;
        InitGame();// set up the new Game
    }

    void HideBackGround()
    {
        dayImage.gameObject.SetActive(false);
    }
  
}
