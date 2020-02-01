using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class shipManager : MonoBehaviour
{
    [SerializeField] private InputComponent _input;
    [SerializeField] private MovementComponent _movement;
    [SerializeField] private FXComponent _FX;

    [HideInInspector] public UnityEvent OnStartMovement;
    [HideInInspector] public UnityEvent OnFinishMovement;
    [HideInInspector] public UnityEvent OnShipMoving;

    public Vector3 GetMovementDirection()
    {
        return _input.GetClickDirection().ToXZPlane();
    }

    public Vector3 GetMovementDisplacementVector()
    {
        return _movement.GetDisplacementVector(_input.GetClickDirection().ToXZPlane());
    }

    public float GetMovementPercent()
    {
        return _movement.MovementPercent;
    }
}

public static class VectorExtensions
{
    public static Vector3 ToXZPlane(this Vector2 position)
    {
        return new Vector3(position.x, 0, position.y);
    }
}