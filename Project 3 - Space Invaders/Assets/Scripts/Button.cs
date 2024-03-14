using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
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

    public void StartButton()
    {
        _manager.ButtonStart();
    }

    public void CreditsButton()
    {
        _manager.LoadCreditsScene();
    }
}
