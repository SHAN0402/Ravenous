using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public GameObject gameManager;
    // Start is called before the first frame update
    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            GameObject.Instantiate(gameManager);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
