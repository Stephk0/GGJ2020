using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int levelNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        print("Start Game");
        //Determine Level
        loadLevel(levelNumber);
        //Initialise UI
        //Spawn Ship


        //Later or never
        //Generate Obstacles
        //Generate Dropoff
    }

    void loadLevel(int num)
    {
        print("Load Level " + num);

        //spawnObstcles();
        //spawnDropoff();

    }

    void spawnObstacles()
    {

    }

    void spawnDropoff()
    {

    }

}
