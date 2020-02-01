using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ShipManager : MonoBehaviour
{
    [SerializeField] private InputComponent _input;
    [SerializeField] private MovementComponent _movement;
    [SerializeField] private SlicingComponent _slicer;
    [SerializeField] private float _actionDistance;
    public HealthComponent health;

    public UnityEvent OnShipStartMovement;
    public UnityEvent OnShipFinishMovement;
    public UnityEvent OnShipMoving;

    public float BuildUpThreshHold = 0.13f;

    private void Start()
    {
        OnShipStartMovement.AddListener(Shake);
    }

    public Vector3 GetMovementDirection()
    {
        return _input.GetClickDirection().ToXZPlane();
    }

    public Vector3 GetMovementDisplacementVector()
    {
        return _movement.GetTotalDisplacementVector(_input.GetClickDirection().ToXZPlane());
    }
    
    public float GetActionDistance()
    {
        return _actionDistance;
    }

    public bool IsShipInBuildUp()
    {
        return _movement.MovementPhases == ShipMovementPhases.BuildUp;
    }
    
    private void Shake()
    {
        Camera.main.DOShakePosition(0.2f, 0.7f);
    }
}

public static class VectorExtensions
{
    public static Vector3 ToXZPlane(this Vector2 position)
    {
        return new Vector3(position.x, 0, position.y);
    }
}