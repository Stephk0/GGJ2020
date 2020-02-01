using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipManager : MonoBehaviour
{
    [SerializeField] private AnimationCurve interpolationCurve;
    [SerializeField] private InputComponent _input;
    [SerializeField] private MovementComponent _movement;
    [SerializeField] private FXComponent _FX;
    [SerializeField] private SlicingComponent _slicer;
    [SerializeField] private float _actionDistance;

    public UnityEvent OnShipStartMovement;
    public UnityEvent OnShipFinishMovement;
    public UnityEvent OnShipMoving;

    public float BuildUpThreshHold = 0.13f;

    public Vector3 GetMovementDirection()
    {
        return _input.GetClickDirection().ToXZPlane();
    }

    public Vector3 GetMovementDisplacementVector()
    {
        return _movement.GetTotalDisplacementVector(_input.GetClickDirection().ToXZPlane());
    }

    public float GetMovementPercent()
    {
        return _movement.MovementPercent;
    } 
    
    public float GetActionDistance()
    {
        return _actionDistance;
    }

    public bool HasShipMovedBelowThreshold()
    {
        return _movement.MovementPercent < BuildUpThreshHold;
    }
}

public static class VectorExtensions
{
    public static Vector3 ToXZPlane(this Vector2 position)
    {
        return new Vector3(position.x, 0, position.y);
    }
}