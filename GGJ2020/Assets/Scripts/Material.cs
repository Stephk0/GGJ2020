using System.Collections;
using UnityEngine;

public enum MaterialType
{
    Type1 = 0,
    Type2 = 1,
    Type3 = 2
}

public class Material : MonoBehaviour
{
    [SerializeField] private MaterialType _materialType;
    [SerializeField] private Color _color;
    [SerializeField] private MeshRenderer _mesh;


    private const float AbsorptionDuration = 1f;

    public void Start()
    {
        _mesh.material.color = _color;
    }

    public MaterialType GetMaterialType()
    {
        return _materialType;
    }
}