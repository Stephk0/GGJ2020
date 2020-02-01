using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipManager : MonoBehaviour
{
    [SerializeField] private InputComponent _input;
    [SerializeField] private MovementComponent _movement;


    public Vector3 GetMovementDirection()
    {
        var direction = _input.GetClickDirection();

        return Vector3.right * direction.x + Vector3.forward * direction.y;
    }
}