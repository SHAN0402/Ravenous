using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
   private Vector2 targetPosition;
   private Transform player;
   private Rigidbody2D rigidbody;
   
   private float smoothing = 3;
   public int lossFood = 10;
   
   
   private BoxCollider2D collider;
   private Animator animator;
   
   //音效
   public AudioClip attackAudio;

   void Start()
   {
      player = GameObject.FindGameObjectWithTag("Player").transform;
      rigidbody = GetComponent<Rigidbody2D>();
      collider = GetComponent<BoxCollider2D>();
      animator = GetComponent<Animator>();
      targetPosition = transform.position;
      GameManager.Instance.enemyList.Add(this);
   }

   private void Update()
   {
      rigidbody.MovePosition( Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime));
   }

   public void Move()
   {
      Vector2 offset = player.position - transform.position;
      if(offset.magnitude<1.1f)
      {
         //attack
         animator.SetTrigger("Attack");
         AudioManager.Instance.RandomPlay(attackAudio);
         player.SendMessage("TakeDamage",lossFood);
         
      }else{
         //follow
         float x = 0;
         float y = 0;
         if (Mathf.Abs(offset.y) > Mathf.Abs(offset.x))
         {
            if (offset.y < 0)
            {
               y = -1;
            }
            else
            {
               y = 1;
            }
         }
         else
         {
            if (offset.x > 0)
            {
               x = 1;
            }
            else
            {
               x = -1;
            }
         }
        
         
         collider.enabled = false;
         RaycastHit2D hit = Physics2D.Linecast(targetPosition,targetPosition+new Vector2(x,y));
         collider.enabled = true;
         Debug.Log(hit.transform);
         if (hit.transform == null)
         {
            targetPosition += new Vector2(x, y);
         }
         else
         {
            if (hit.collider.tag == "Food" || hit.collider.tag == "Cola")
            {
               targetPosition += new Vector2(x, y);
            }
            
         }

         
      }

   }
}
