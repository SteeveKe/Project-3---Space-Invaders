using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void Enemydied(int point, Enemy enemy);
    public static event Enemydied OnEnemyDied;
    public delegate void EnemyAdd(Enemy enemy);
    public static event EnemyAdd OnEnemyStart;
    public GameObject bullet;
    public Transform shottingOffset;
    
    public int life = 1;

    public int pointWorth = 10;
    private AudioSource explosion;
    
    // Start is called before the first frame update
    private void Start()
    {
        explosion = GameObject.Find("explosion").GetComponent<AudioSource>();
        Player.OnEnemyWin += KillEnemy;
        EnemyTrigger.OnEnemyWin += KillEnemy;
        if (gameObject.CompareTag("enemy"))
        {
            OnEnemyStart.Invoke(this);
        }
    }

    public void KillEnemy()
    {
        EnemyTrigger.OnEnemyWin -= KillEnemy;
        Player.OnEnemyWin -= KillEnemy;
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            Destroy(collision.gameObject);
            life -= 1;
            if (gameObject.CompareTag("boss"))
            {
                if (life == 1)
                {
                    GetComponent<Animator>().SetTrigger("disable shield");
                }
            }
            if (life <= 0)
            {
                OnEnemyDied.Invoke(pointWorth, this);
                GetComponent<Animator>().SetTrigger("died");
                explosion.Play();
            }
        }
    }

    void DeathAnimationComplete()
    {
        EnemyTrigger.OnEnemyWin -= KillEnemy;
        Player.OnEnemyWin -= KillEnemy;
        Destroy(gameObject);
    }

    public void Shoot()
    {
        GameObject shot = Instantiate(bullet, shottingOffset.position, Quaternion.identity);
        Destroy(shot, 3f);
    }
}
