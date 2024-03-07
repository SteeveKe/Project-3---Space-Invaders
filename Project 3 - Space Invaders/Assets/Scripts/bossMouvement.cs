using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class bossMouvement : MonoBehaviour
{
    // Start is called before the first frame update
    public bool CanMove = false;
    public bool rightDirection = true;
    public float speed = 10f;
    public float minMoveDelay = 5;
    public float maxMoveDelay = 10;

    private float moveDelay;
    private float timer;
    void Start()
    {
        moveDelay = Random.Range(minMoveDelay, maxMoveDelay);
        timer = moveDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove)
        {
            Move();
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            CanMove = true;
        }
    }

    public void Move()
    {
        transform.position += Vector3.right * speed * Time.deltaTime * (rightDirection ? 1 : -1);
    }

    public void ChangeDirection()
    {
        gameObject.transform.Rotate(0, 0, 180);
        rightDirection = !rightDirection;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("bossWall"))
        {
            moveDelay = Random.Range(minMoveDelay, maxMoveDelay);
            ChangeDirection();
            CanMove = false;
            timer = moveDelay;
        }
    }
}
