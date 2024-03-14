using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChangeDirection : MonoBehaviour
{
    private GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("enemy"))
        {
            manager.StepDown();
        }
    }
}
