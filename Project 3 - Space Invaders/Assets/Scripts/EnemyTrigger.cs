using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    public delegate void EnemyWin();
    public static event EnemyWin OnEnemyWin;
    public bool CanDestroy;
    
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        CanDestroy = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("enemy"))
        {
            if (CanDestroy)
            {
                CanDestroy = false;
                OnEnemyWin.Invoke();
                manager.EndGame();
            }
        }
    }
}
