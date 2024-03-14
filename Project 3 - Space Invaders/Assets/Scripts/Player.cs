using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public GameObject bullet;
  public float shootDelay = 1f;
  public Transform shottingOffset;
  public float speed;
  public int init_life = 3;
  private int life = 1;
  private float canShoot = 0f;
  public AudioSource shootSound;
  public AudioSource explosion;
  
  private GameManager _manager;

  public delegate void EnemyWin();
  public static event EnemyWin OnEnemyWin;

  private void Start()
  {
    _manager = FindObjectOfType<GameManager>() ;
    life = init_life;
  }

  public int getLife()
  {
    return life;
  }
  // Update is called once per frame
  void Update()
  {
    float mouvement = Input.GetAxis("Horizontal");
    Vector3 pos = transform.position + Vector3.right * speed * mouvement * Time.deltaTime;
    pos.x = Mathf.Clamp(pos.x, -12.5f, 12.5f);
    transform.position = pos;
    
    canShoot -= Time.deltaTime;
    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (canShoot < 0)
      {
        GetComponent<Animator>().SetTrigger("Shoot");
        shootSound.Play();
        canShoot = shootDelay;
        GameObject shot = Instantiate(bullet, shottingOffset.position, Quaternion.identity);
        Destroy(shot, 3f);
      }
    }
  }

  private void OnCollisionEnter2D(Collision2D col)
  {
    if (col.gameObject.CompareTag("EnemyBullet"))
    {
      TakeDamage();
      Destroy(col.gameObject);
    }
  }

  private void TakeDamage()
  {
    life -= 1;
    _manager.SetLife();
    if (life == 0)
    {
      explosion.Play();
      OnEnemyWin.Invoke();
      _manager.EndGame();
    }
  }
  
  public void InitLife()
  {
    life = init_life;
  }
}
