using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialResourcePlacer : MonoBehaviour
{
    public bool useWeighting;

//    public AsteroidLevelProfile[] levelProfiles;
    public Vector3[] WorldCorners => _worldCorners;
    public List<Asteroid> generatedAsteroids;
    public float edgeBufferRange = 5f;
    public ShipManager _shipManager;
    public float spawnRadius = 3f;

    private float _depth;
    private Vector3[] _screenCorners;
    private Vector3[] _worldCorners;
    private int materialPerAsteroid = 2;
    

//    void Start()
//    {
//        Init();
//    }

//    private void Init()
//    {
////        _worldCorners = GetWorldBounds();
//        
//        //Test only since not linked to game manager
////        SetupMaterialResources(DifficultyController.difficulty - 1);
//    }

    public void SetupMaterialResources(AsteroidLevelProfile profile)
    {
//        if (levelId < levelProfiles.Length){
//            var materialProfile = levelProfiles[levelId];
        _worldCorners = Camera.main.GetWorldBounds(edgeBufferRange);
        GenerateResources(profile);

        DifficultyController.winValue = profile.victoryCount;
//        }
//        else{
//            Debug.LogWarning($"Material profile for Level {levelId} is missing");
//        }
    }


    private void GenerateResources(AsteroidLevelProfile profile)
    {
        generatedAsteroids = new List<Asteroid>();

        if (!useWeighting || profile.asteroidTypes.Length < 2)
        {
            for (var i = 0; i < profile.volume; i++)
            {
                generatedAsteroids.Add(SpawnAsteroid(profile.asteroidTypes[0], materialPerAsteroid));
            }
        }
        else
        {
            var volumes = VolumeWeighted(profile.volume, profile.objWeighting);

            for (int i = 0; i < volumes.Length; i++)
            {
                for (int j = 0; j < volumes[i]; j++)
                {
                    generatedAsteroids.Add(SpawnAsteroid(profile.asteroidTypes[i], materialPerAsteroid));
                }
            }
        }
    }

    private Asteroid SpawnAsteroid(Asteroid source, int amount)
    {
        var spawnPos = RandomSpawnPosition(_worldCorners);
        var generated = Instantiate(source.gameObject, spawnPos, Quaternion.identity).GetComponent<Asteroid>();
        generated.Amount = amount;

        return generated;
    }

    private Vector3 RandomSpawnPosition(Vector3[] corners)
    {
        Vector3 newPosition = Vector3.zero;
        do {
            var posX = Random.Range(corners[0].x, corners[2].x);
            var posZ = Random.Range(corners[0].z, corners[2].z);

            newPosition = new Vector3(posX, 0f, posZ);
        } while (!CheckPositionAvailable(newPosition));

        return newPosition;
    }

    private bool CheckPositionAvailable(Vector3 newPosition)
    {
        if (Vector3.Distance(_shipManager.transform.position, newPosition) < spawnRadius) {
            return false;
        }

        foreach (var asteroid in generatedAsteroids) {
            if (Vector3.Distance(asteroid.transform.position, newPosition) < spawnRadius) {
                return false;
            }    
        }

        return true;
    }
    
    private int[] VolumeWeighted(int volume, float[] weights)
    {
        var remainingVolume = volume;

        var volumeWeighted = new int[weights.Length];
        var normalizedWeights = new float[weights.Length];

        var increment = 1f / weights.Sum();
        var lowestWeightedIndex = 0;
        var lowestWeight = 1f;

        for (int i = 0; i < weights.Length; i++)
        {
            var weight = weights[i] * increment;

            if (weight < lowestWeight)
            {
                lowestWeight = weight;
                lowestWeightedIndex = i;
            }

            normalizedWeights[i] = weight;
        }

        for (int i = 0; i < normalizedWeights.Length; i++)
        {
            if (i == lowestWeightedIndex) continue;

            var weightedVolume = (int) (normalizedWeights[i] * volume);
            volumeWeighted[i] = weightedVolume;
            remainingVolume -= weightedVolume;
        }

        volumeWeighted[lowestWeightedIndex] = remainingVolume;

        return volumeWeighted;
    }
}