using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialResourcePlacer : MonoBehaviour
{
    public bool useWeighting;
    public MaterialLevelProfile[] levelProfiles;
    public Vector3[] WorldCorners => _worldCorners;
    public List<MaterialResource> generatedResources;
    
    private float _depth;
    private Vector3[] _screenCorners;
    private Vector3[] _worldCorners;
    
    void Start()
    {
        Init();
    }

    private void Init()
    {
        _worldCorners = GetWorldBounds();
        
        //Test only since not linked to game manager
        SetupMaterialResources(0);
    }

    public void SetupMaterialResources(int levelId)
    {
        if (levelId < levelProfiles.Length){
            var materialProfile = levelProfiles[levelId];
            GenerateResources(materialProfile);
        }
        else{
            Debug.LogWarning($"Material profile for Level {levelId} is missing");
        }
    }
    
    public Vector3[] GetWorldBounds()
    {
        _depth = Camera.main.transform.position.y;
            
        _screenCorners = new [] {new Vector3(0, 0, _depth), //bottom left
                            new Vector3(0, 1, _depth), //top lef
                            new Vector3(1, 1, _depth), //top right
                            new Vector3( 1, 0, _depth)}; //bottom right;
        
        var worldCorners = new Vector3[_screenCorners.Length];

        for (var i = 0; i < _screenCorners.Length; i++){
            var corner = Camera.main.ViewportToWorldPoint(_screenCorners[i]);
            corner.y = 0f;
            worldCorners[i] = corner;
            
            Debug.Log($"Corner {i} position: {worldCorners[i]}");
            Debug.DrawRay(worldCorners[i], Vector3.up * 10f, Color.white, 5f);
        }

        return worldCorners;
    }

    private void GenerateResources(MaterialLevelProfile profile)
    {
        generatedResources = new List<MaterialResource>();

        if (!useWeighting || profile.materialResourceTypes.Length < 2){
            for (var i = 0; i < profile.volume; i++){
                generatedResources.Add(SpawnResource(profile.materialResourceTypes[0], 1));
            }
        }
        else{
            var volumes = VolumeWeighted(profile.volume, profile.objWeighting);

            for(int i = 0; i < volumes.Length; i++){
                for (int j = 0; j < volumes[i]; j++){
                    generatedResources.Add(SpawnResource(profile.materialResourceTypes[i], 1));
                }
            }
        }
    }

    private MaterialResource SpawnResource(MaterialResource source, int amount)
    {
        var spawnPos = RandomSpawnPosition(_worldCorners);
        var generated = Instantiate(source.gameObject, spawnPos, Quaternion.identity).GetComponent<MaterialResource>();
        generated.Amount = amount;
        
        return generated;
    }

    private Vector3 RandomSpawnPosition(Vector3[] corners)
    {
        var posX = Random.Range(corners[0].x, corners[2].x);
        var posZ = Random.Range(corners[0].z, corners[2].z);
        return new Vector3(posX, 0f, posZ);
    }

    private int[] VolumeWeighted(int volume, float[] weights)
    {
        var remainingVolume = volume;

        var volumeWeighted = new int[weights.Length];
        var normalizedWeights = new float[weights.Length];
        
        var increment = 1f / weights.Sum();
        var lowestWeightedIndex = 0;
        var lowestWeight = 1f;

        for (int i = 0; i < weights.Length; i++){
            var weight = weights[i] * increment;

            if (weight < lowestWeight){
                lowestWeight = weight;
                lowestWeightedIndex = i;
            }
            normalizedWeights[i] = weight;
        }
        
        for (int i = 0; i < normalizedWeights.Length; i++){
            if (i == lowestWeightedIndex) continue;
            
            var weightedVolume = (int)(normalizedWeights[i] * volume);
            volumeWeighted[i] = weightedVolume;
            remainingVolume -= weightedVolume;
        }

        volumeWeighted[lowestWeightedIndex] = remainingVolume;
        
        return volumeWeighted;
    }
}
