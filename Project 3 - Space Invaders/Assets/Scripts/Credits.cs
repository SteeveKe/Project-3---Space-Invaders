using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public GameObject player;

    private GameManager _manager;
    // Start is called before the first frame update
    void Start()
    {
        _manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisplayPlayer()
    {
        player.SetActive(true);
    }

    void ChangeScene()
    {
        _manager.LoadMainMenu();
    }
}
