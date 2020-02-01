using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
        var cam = Camera.main;
        _depth = cam.transform.position.y;
            
        _screenCorners = new [] {new Vector3(0, 0, _depth), //bottom left
                            new Vector3(0, 1, _depth), //top lef
                            new Vector3(1, 1, _depth), //top right
                            new Vector3( 1, 0, _depth)}; //bottom right;
        
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

    private void GenerateResources(MaterialLevelProfile profile)
    {
        generatedResources = new List<MaterialResource>();

        if (!useWeighting){
            for (var i = 0; i < profile.volume; i++){
                var posX = UnityEngine.Random.Range(_worldCorners[0].x, _worldCorners[2].x);
                var posZ = UnityEngine.Random.Range(_worldCorners[0].z, _worldCorners[2].z);
                var spawnPos = new Vector3(posX, 0f, posZ);
                
                var source = profile.materialResourceTypes[0];
                var generated = Instantiate(source.gameObject, spawnPos, Quaternion.identity).GetComponent<MaterialResource>();
                
                generated.Amount = 1;
                generatedResources.Add(generated);
            }
        }
        else{
            //TODO: calculate weighting distribution
            return;
        }
    }
}
