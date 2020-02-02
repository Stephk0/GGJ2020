using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Asteroid_Profile_0", menuName = "Resources/AsteroidProfile", order = 1)]
public class AsteroidLevelProfile : ScriptableObject
{
    public string name;
    public int level;

    [Space(10)] 
    
    public int volume;
    public int victoryCount;

    public int initialHealth = 100;
    public int healthDecreasePerTick = 1;
    public float healthDecreaseTickTime = 1f;
    
    public Asteroid[] asteroidTypes;
    public float[] objWeighting;
}
