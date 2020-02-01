using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.TestTools;

public class FXComponent : MonoBehaviour
{
    [SerializeField] private ShipManager _ship;
    [SerializeField] private LineRenderer _shipLineRenderer;

    public ParticleSystem sucktionParticle;
    public ParticleSystem leftoverParticle;


    private void Start()
    {
        //_ship.OnStartMovement.AddListener(DrawLine);
        //_ship.OnShipMoving.AddListener(UpdateLine);
        _ship.OnShipStartMovement.AddListener(StartParticle);
    }

    public void DrawLine()
    {
        UpdateLineRenderer();
    }

    public void UpdateLine()
    {
        UpdateLineRenderer();
    }

    public void UpdateLineRenderer()
    {
        var positions = GetLinePositions();
        _shipLineRenderer.SetPositions(positions.ToArray());
    }

    public List<Vector3> GetLinePositions()
    {
        
        List<Vector3> positions = new List<Vector3>();
        positions.Add(this.transform.position);
        //positions.Add(this.transform.position + _ship.GetMovementDisplacementVector() * (1 - _ship.GetActionDistance()()));
        return positions;
        
    }


    public void StartParticle()
    {
        //ParticleSystem.EmissionModule emissionModule = sucktionParticle.emission;
        //emissionModule.enabled = true;
        sucktionParticle.Play(true);
        leftoverParticle.Play(true);
    }
}