using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class ResourcePlacer : MonoBehaviour
{
    public int totalVolume;
    public TempMaterialTypes types;

    [Space(10)] 
    public bool useWeighting;
    public float[] objWeighting;
    
    [Space(10)] 
    
    public int winAmount;
    public ResourceContainer[] resources;

    private List<ResourceContainer> _generatedResources;
    
    private float depth = 1000f;
    private Vector3[] _screenCorners;
    private Vector3[] _worldCorners;
    
    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    private void OnValidate()
    {
        //Init();
    }

    private void Init()
    {
        _worldCorners = GetWorldBounds();
        GenerateResources();
        PlaceResources(_generatedResources.ToArray());
    }
    
    private Vector3[] GetWorldBounds()
    {
        var cam = Camera.main;
        depth = cam.transform.position.y;
            
        _screenCorners = new [] {new Vector3(0, 0, depth), //bottom left
                            new Vector3(0, 1, depth), //top lef
                            new Vector3(1, 1, depth), //top right
                            new Vector3( 1, 0, depth)}; //bottom right;
        
        var worldCorners = new Vector3[_screenCorners.Length];

        for (var i = 0; i < _screenCorners.Length; i++){
            var corner = cam.ViewportToWorldPoint(_screenCorners[i]);
            corner.y = 0f;
            worldCorners[i] = corner;
            
            Debug.Log($"Corner {i} position: {worldCorners[i]}");
            Debug.DrawRay(worldCorners[i], Vector3.up * 10f, Color.white, 5f);
        }

        return worldCorners;
    }

    private void GenerateResources()
    {
        _generatedResources = new List<ResourceContainer>();

        if (!useWeighting)
        {
            for (var i = 0; i < totalVolume; i++){
                var generated = new ResourceContainer();
                var source = resources[0];
                
                generated.prefab = source.prefab;
                generated.volume = source.volume;
                
                _generatedResources.Add(generated);
            }
        }
        else
        {
            //TODO: calculate weighting distribution
            return;
        }
    }

    private void PlaceResources(ResourceContainer[] resources)
    {
        List<GameObject> placedObjects = new List<GameObject>();
        
        foreach (var resource in resources)
        {
            var posX = UnityEngine.Random.Range(_worldCorners[0].x, _worldCorners[2].x);
            var posZ = UnityEngine.Random.Range(_worldCorners[0].z, _worldCorners[2].z);
            var spawnPos = new Vector3(posX, 0f, posZ);

            var spawnedObj = Instantiate(resource.prefab, spawnPos, Quaternion.identity);
            placedObjects.Add(spawnedObj);
        }
    }
}

[Serializable]
public struct ResourceContainer
{
    public GameObject prefab;
    public int volume;
}

public enum TempMaterialTypes
{
    Fluff,
    Stuff
}
