using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] private shipManager _manager;
    [SerializeField] AnimationCurve interPolationCurve;
    [SerializeField] private float _duration = 0.5f;
    public float MovementDistance = 10;
    private bool isMoving = false;

    private Coroutine movementCoroutine = null;

    private void Update()
    {
        Vector3 movementDirection = _manager.GetMovementDirection();
        if (!isMoving && !movementDirection.sqrMagnitude.Equals(0))
        {
            movementCoroutine = StartCoroutine(MoveShip(_manager.GetMovementDirection()));
        }
    }

    IEnumerator MoveShip(Vector3 direction)
    {
        isMoving = true;
        Vector3 initialPos = this.transform.position;
        for (float t = 0; t <= _duration; t += Time.deltaTime)
        {
            float percent = (t / _duration);

            this.transform.position = Vector3.Lerp(initialPos, initialPos + direction * MovementDistance,
                interPolationCurve.Evaluate(percent));
            yield return new WaitForEndOfFrame();
        }

        this.transform.position = Vector3.Lerp(initialPos, initialPos + direction * MovementDistance,
            interPolationCurve.Evaluate(1));

        isMoving = false;
    }
}