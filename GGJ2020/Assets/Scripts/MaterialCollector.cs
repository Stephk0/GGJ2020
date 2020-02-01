﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialCollector : MonoBehaviour
{
    [SerializeField] private ShipManager _manager;
    [SerializeField] private SphereCollider _materialCollectorCollider;

    private List<GameObject> CollidedMaterials = new List<GameObject>();
    private bool collected = false;

    public void Start()
    {
        _manager.OnShipMoving.AddListener(TryDisableSelf);
    }

    public void OnTriggerStay(Collider other)
    {
        var material = other.transform.GetComponent<MaterialResource>();
        if (material != null)
        {
            CollidedMaterials.Add(other.gameObject);
            other.transform.gameObject.SetActive(false);
            DifficultyController.collectedMaterials += 1;
        }
    }

    public void TryDisableSelf()
    {
        this._materialCollectorCollider.enabled = _manager.IsShipInBuildUp();
    }
}