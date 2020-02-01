using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Resource_Profile_0", menuName = "Resources/Profile", order = 1)]
public class MaterialLevelProfile : ScriptableObject
{
    public string name;
    public int level;

    [Space(10)] 
    
    public int volume;
    public int victoryCount;
    
    [FormerlySerializedAs("materialTypes")] public MaterialResource[] materialResourceTypes;
    public float[] objWeighting;
}
