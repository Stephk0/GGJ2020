using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCollector : MonoBehaviour
{
    [SerializeField] private ShipManager _manager;
    [SerializeField] private SphereCollider _materialCollectorCollider;

    private List<GameObject> _collidedMaterials = new List<GameObject>();
    private bool _collected = false;

    public void Start()
    {
        _manager.OnShipMoving.AddListener(TryDisableSelf);
    }

    public void OnTriggerStay(Collider other)
    {
        var material = other.transform.GetComponent<MaterialResource>();
        if (material == null) 
            return;
        
        _collidedMaterials.Add(other.gameObject);
        other.transform.gameObject.SetActive(false);
        _manager.health.Apply(-material.healthRepairAmount);    
        DifficultyController.collectedMaterials += 1;
    }

    public void TryDisableSelf()
    {
        _materialCollectorCollider.enabled = _manager.IsShipInBuildUp();
    }
}