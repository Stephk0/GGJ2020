using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public enum MaterialResourceType
{
    Type1,
    Type2,
    Type3
}

public class MaterialResource : MonoBehaviour
{
    [SerializeField] private MaterialResourceType materialResourceType;
    public int healthRepairAmount = 4;
    public int Amount
    {
        get => _amount;
        set => _amount = value;
    }

    private int _amount;
    
    private const float AbsorptionDuration = 1f;

    public MaterialResourceType GetMaterialResourceType()
    {
        return materialResourceType;
    }
}