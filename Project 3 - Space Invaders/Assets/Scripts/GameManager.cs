using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Range(1,11)]public int enemyPerLayer = 10;
    public float secondPerStep = 1f;
    public float step = 0.25f;
    public float stepIncrease = 0.95f;
    public GameObject enemyRoot;
    public List<GameObject> enemy;
    public bool goDown;
    public TMP_Text ScoreText;
    public TMP_Text hi_scoreText;
    public GameObject player;
    public List<Enemy> enemyList;
    public float enemyShootDelay;
    public TMP_Text lifeText;
    public Transform barricadeSpawnPosition;
    public GameObject barricade;
    public Transform bossSpawnPosition;
    public GameObject Boss;
    public AudioSource explosionSound;
    public AudioSource enemyShootSound;

    private GameObject barricadeObject;
    private Player p;
    private bool isRunning = false;
    private bool goLeft;
    private int score = 0;
    private int hi_score;
    private float init_SecondPerStep;
    private GameObject _boss;
    private bool isCreditScene = false;
    private static bool isAlreadyInstantiate = false;
    
    private void Awake()
    {
        if (isAlreadyInstantiate)
        {
            Destroy(gameObject);
        }
        else
        {
            isAlreadyInstantiate = true;
        }
        DontDestroyOnLoad(gameObject);
        init_SecondPerStep = secondPerStep;
        enemyList = new List<Enemy>();
        isRunning = false;
        Enemy.OnEnemyDied += AddPoint;
        Enemy.OnEnemyStart += AddEnemyList;
        goDown = false;
        goLeft = false;
        hi_score = PlayerPrefs.GetInt("hi_score");
        hi_scoreText.text = "HI_SCORE\n" + hi_score.ToString("D4");
        score = 0;
        ScoreText.text = "SCORE\n" + score.ToString("D4");
    }

    private void AddEnemyList(Enemy enemy)
    {
        enemyList.Add(enemy);
    }

    private void AddPoint(int point, Enemy enemy)
    {
        //explosionSound.Play();
        enemyList.Remove(enemy);
        secondPerStep *= stepIncrease;
        score += point;
        ScoreText.text = "SCORE\n" + score.ToString("D4");
    }

    private IEnumerator EnemyMouvement()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(secondPerStep);
            MoveStep();
        }
    }

    private IEnumerator RandomEnemyShoot()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(enemyShootDelay);
            if (enemyList.Count > 0)
            {
                Enemy enemy = enemyList[Random.Range(0, enemyList.Count)];
                enemy.GetComponent<Animator>().SetTrigger("shoot");
                enemy.Shoot();
                enemyShootSound.Play();
            }
        }
    }

    public void SetIsCreditsScene(bool isCreditsScene)
    {
        isCreditScene = isCreditsScene;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isCreditScene)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerPrefs.SetInt("hi_score", 0);
                hi_score = 0;
                hi_scoreText.text = "HI_SCORE\n" + hi_score.ToString("D4");
            } 
            if (isRunning)            
            {
                if (enemyList.Count == 0 && p.getLife() > 0)
                {
                    if (_boss.IsDestroyed())
                    {
                        _boss = Instantiate(Boss, bossSpawnPosition);
                    }
                    secondPerStep = init_SecondPerStep;
                    InstantiateEnemy();
                }
            }
        }
    }

    public void StartGame()
    {
        _boss = Instantiate(Boss, bossSpawnPosition);
        secondPerStep = init_SecondPerStep;
        p.InitLife();
        SetLife();
        lifeText.gameObject.SetActive(true);
        score = 0;
        UpdateScore();
        isRunning = true;
        goDown = false;
        goLeft = false;
        InstantiateEnemy();
        StartCoroutine(EnemyMouvement());
        StartCoroutine(RandomEnemyShoot());
        barricadeObject = Instantiate(barricade, barricadeSpawnPosition);
    }

    public void SetLife()
    {
        lifeText.text = "LIFE\nx" + p.getLife();
    }

    private void InstantiateEnemy()
    {
        Vector3 pos;
        if (enemyPerLayer % 2 == 0)
        {
            for (int i = 0; i < enemyPerLayer / 2; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    pos = new Vector3(2 * i + 1, -j * 2 + 5, 0f);
                    Instantiate(enemy[j], pos, Quaternion.identity, enemyRoot.transform);
                    pos = new Vector3(-2 * i - 1, -j * 2 + 5, 0f);
                    Instantiate(enemy[j], pos, Quaternion.identity, enemyRoot.transform);
                }
            }
        }
        else
        {
            pos = new Vector3(0, 0 + 5, 0);
            Instantiate(enemy[0], pos, Quaternion.identity, enemyRoot.transform);
            pos = new Vector3(0, -2 + 5, 0);
            Instantiate(enemy[1], pos, Quaternion.identity, enemyRoot.transform);
            pos = new Vector3(0, -4 + 5, 0);
            Instantiate(enemy[2], pos, Quaternion.identity, enemyRoot.transform);
            
            for (int i = 1; i < enemyPerLayer / 2 + 1; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    pos = new Vector3(2 * i, -j * 2 + 5, 0f);
                    Instantiate(enemy[j], pos, Quaternion.identity, enemyRoot.transform);
                    pos = new Vector3(-2 * i, -j * 2 + 5, 0f);
                    Instantiate(enemy[j], pos, Quaternion.identity, enemyRoot.transform);
                }
            }
        }
    }

    public void MoveStep()
    {
        if (isRunning)
        {
            Vector3 pos;
            pos = enemyRoot.transform.position;
            if (goDown)
            {
                goDown = false;
                pos.y -= 0.5f;
                enemyRoot.transform.position = pos;
                goLeft = !goLeft;
            }
            else
            {
                if (goLeft)
                {
                    pos.x -= step;
                    enemyRoot.transform.position = pos;
                }
                else
                {
                    pos.x += step;
                    enemyRoot.transform.position = pos;
                }
            }
        }
    }

    public void StepDown()
    {
        goDown = true;
    }

    public void EndGame()
    {
        if (!_boss.IsDestroyed())
        {
            Destroy(_boss);
        }
        foreach (Enemy enemy in enemyList)
        {
            enemy.KillEnemy();
        }
        enemyList = new List<Enemy>();
        isRunning = false;
        goDown = false;
        goLeft = false;
        if (score > hi_score)
        {
            hi_score = score;
            PlayerPrefs.SetInt("hi_score", hi_score);
            hi_scoreText.text = "HI_SCORE\n" + hi_score.ToString("D4");
        }
        
        StopCoroutine(EnemyMouvement());
        StopCoroutine(RandomEnemyShoot());
        Destroy(barricadeObject);

        LoadCreditsScene();
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("Credits Scene");
        isCreditScene = true;
    }

    public void ButtonStart()
    {
        SceneManager.LoadScene("Game Scene");
        StartCoroutine(FindObjectGameScene());
    }

    IEnumerator FindObjectGameScene()
    {
        GameObject temp = GameObject.Find("BossSpawnPosition");
        while (temp == null)
        {
            yield return null;
            temp = GameObject.Find("BossSpawnPosition");
        }
        bossSpawnPosition = temp.GetComponent<Transform>();
        
        temp = GameObject.Find("BarricadeSpawn");
        while (temp == null)
        {
            yield return null;
            temp = GameObject.Find("BarricadeSpawn");
        }
        barricadeSpawnPosition = temp.GetComponent<Transform>();
        
        temp = GameObject.Find("LifeText");
        while (temp == null)
        {
            yield return null;
            temp = GameObject.Find("LifeText");
        }
        lifeText = temp.GetComponent<TMP_Text>();
        
        temp = GameObject.Find("Score");
        while (temp == null)
        {
            yield return null;
            temp = GameObject.Find("Score");
        }
        ScoreText = temp.GetComponent<TMP_Text>();
        
        temp = GameObject.Find("Hi-score");
        while (temp == null)
        {
            yield return null;
            temp = GameObject.Find("Hi-score");
        }
        hi_scoreText = temp.GetComponent<TMP_Text>();
        
        temp = GameObject.Find("Player");
        while (temp == null)
        {
            yield return null;
            temp = GameObject.Find("Player");
        }
        player = temp;
        
        temp = GameObject.Find("EnemyHandler");
        while (temp == null)
        {
            yield return null;
            temp = GameObject.Find("EnemyHandler");
        }
        enemyRoot = temp;
        
        p = player.GetComponent<Player>();
        StartGame();
    }

    public IEnumerator FindScore()
    {
        GameObject temp = GameObject.Find("Score");
        while (temp == null)
        {
            yield return null;
            temp = GameObject.Find("Score");
        }
        ScoreText = temp.GetComponent<TMP_Text>();
        
        temp = GameObject.Find("Hi-score");
        while (temp == null)
        {
            yield return null;
            temp = GameObject.Find("Hi-score");
        }
        hi_scoreText = temp.GetComponent<TMP_Text>();
        UpdateScore();
    }
    public void UpdateScore()
    {
        hi_scoreText.text = "HI_SCORE\n" + hi_score.ToString("D4");
        ScoreText.text = "SCORE\n" + score.ToString("D4");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
        SetIsCreditsScene(false);
        StartCoroutine(FindScore());
    }
}
