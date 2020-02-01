using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class MotherShip : MonoBehaviour
{
    [FormerlySerializedAs("modules")] public ShipModule[] shipModules;
    public float rotationSpeed = 0.1f;

    private Vector3 centre;
    
    void Start()
    {
        Init();
    }

    void Update()
    {
        RotateShip();
    }

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        shipModules = GetComponentsInChildren<ShipModule>();
        if(shipModules.Length > 0)
            GetCenter();
    }

    private void RotateShip()
    {
        //transform.Rotate(Vector3.up, rotationSpeed);
        transform.RotateAround(centre, Vector3.up, rotationSpeed);
    }

    private void GetCenter()
    {
        var modRenderer = shipModules[0].GetComponentInChildren<Renderer>().bounds;
        foreach (var module in shipModules)
        {
            var renderers = module.GetComponentsInChildren<Renderer>();
            foreach (var render in renderers)
            {
                modRenderer.Encapsulate(render.bounds);
            }
        }

        centre = modRenderer.center;
    }
}
