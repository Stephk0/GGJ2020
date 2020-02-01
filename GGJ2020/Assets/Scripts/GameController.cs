using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;
    public ShipManager shipManager;
    private bool haveWon;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Debug.Log("Start Game");
    }

    private void Init()
    {
        shipManager = FindObjectOfType<ShipManager>();
        shipManager.health.IsDestroyed += levelLoss;
    }

    void Update()
    {
        if (DifficultyController.collectedMaterials >= DifficultyController.winValue)
        {
            DifficultyController.difficulty += 1;
            levelWin();
        }
    }

    void levelWin()
    {
        winScreen.SetActive(true);
    }

    void levelLoss()
    {
        loseScreen.SetActive(true);
    }

}
