using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject loseScreen;
    private bool haveWon = false;

    // Start is called before the first frame update
    void Start()
    {
        print("Start Game");
        //Initialise UI

    }

    void Update()
    {
        if (DifficultyController.collectedMaterials >= DifficultyController.winValue)
        {
            if(haveWon == false)
            {
                haveWon = true;
                DifficultyController.difficulty += 1;
                levelWin();
            }
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
