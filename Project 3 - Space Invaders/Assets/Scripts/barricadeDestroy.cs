using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class barricadeDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("EnemyBullet") || col.gameObject.CompareTag("bullet"))
        {
            Destroy(col.gameObject);
            Destroy(gameObject);
        }
    }
}
