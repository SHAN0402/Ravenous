using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    //攻击音效
    public AudioClip chop1;
    public AudioClip chop2;
    //脚步音效
    public AudioClip step1Audio;
    public AudioClip step2Audio;
    //喝可乐
    public AudioClip cola1Audio;
    public AudioClip cola2Audio;
    //吃食物
    public AudioClip food1Audio;
    public AudioClip food2Audio;
    
    private int positionx = 1;
    private int positiony = 1;
    [HideInInspector] public Vector2 targetPos = new Vector2(1, 1);
    private Rigidbody2D rigidbody;
    public float smoothing = 4;
    public float restTime = 1;
    public float restTimer;
    
    private BoxCollider2D collider;
    private Animator _animator;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {//每次操作之间空一秒
        rigidbody.MovePosition(Vector2.Lerp(transform.position,targetPos,smoothing*Time.deltaTime));
        
        if (GameManager.Instance.food <= 0|| GameManager.Instance.isEnd==true)
        {
           return;
        }
        
        restTimer += Time.deltaTime;
        if (restTimer < restTime)
        {
            return;
        }
        
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h > 0)
        {
            v = 0;
        }

       
        if (h != 0 || v != 0)
        {
            GameManager.Instance.ReduceFood(1);
            //检测是否碰到障碍物，是什么障碍物
            collider.enabled = false;
           RaycastHit2D hit = Physics2D.Linecast(targetPos,targetPos+new Vector2(h,v));
            collider.enabled = true;
            if (hit.transform == null)
            {
                targetPos += new Vector2(h, v);
            
                AudioManager.Instance.RandomPlay(step1Audio,step2Audio);
                restTimer = 0;
                
            }
            else
            {
                switch (hit.collider.tag)
                {
                    case "outWall" :
                        break;
                    case "Wall":
                        _animator.SetTrigger("Attack");
                        AudioManager.Instance.RandomPlay(chop1,chop2);
                        hit.collider.SendMessage("TakeDamage");
                        break;
                    case "Food":
                        GameManager.Instance.AddFood(10);
                        targetPos += new Vector2(h, v);
                        AudioManager.Instance.RandomPlay(step1Audio,step2Audio);
                        
                        Destroy(hit.transform.gameObject);
                        AudioManager.Instance.RandomPlay(food1Audio,food2Audio);
                        break;
                    case"Cola":
                        GameManager.Instance.AddFood(20);
                         targetPos += new Vector2(h, v); 
                        AudioManager.Instance.RandomPlay(step1Audio,step2Audio);
                        
                        Destroy(hit.transform.gameObject);
                        AudioManager.Instance.RandomPlay(cola1Audio,cola2Audio);
                        break;
                    case "Enemy":
                        break;
                }
                GameManager.Instance.OnPlayerMove();
                restTimer = 0;
                
            }
           
        }

      
        
       
    }

    public void TakeDamage(int lossFood)
    {
        GameManager.Instance.ReduceFood(lossFood);
        _animator.SetTrigger("Damage");
    }
}
