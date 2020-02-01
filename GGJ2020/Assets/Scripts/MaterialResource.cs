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
    [SerializeField] private Color _color;
    [SerializeField] private MeshRenderer _mesh;

    public int Amount
    {
        get => _amount;
        set => _amount = value;
    }

    private int _amount;
    
    private const float AbsorptionDuration = 1f;

    public void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        _mesh.sharedMaterial.color = _color;
    }

    public MaterialResourceType GetMaterialResourceType()
    {
        return materialResourceType;
    }
}