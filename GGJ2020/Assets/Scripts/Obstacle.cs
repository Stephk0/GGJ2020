using UnityEngine;

enum AsteroidType
{
    DestroyableAsteroid, UnbreakableAsteroid
}

public class Obstacle : MonoBehaviour
{
    [SerializeField] private AsteroidType type;
    
    
}
