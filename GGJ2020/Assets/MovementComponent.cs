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
    private Vector3 initialScale = new Vector3(1, 1, 1);

    private Coroutine movementCoroutine = null;

    private void Update()
    {
        Vector3 movementDirection = _manager.GetMovementDirection();
        if (!movementDirection.sqrMagnitude.Equals(0))//!isMoving && 
        {
               movementCoroutine = StartCoroutine(MoveShip(_manager.GetMovementDirection()));
        }
    }

    IEnumerator MoveShip(Vector3 direction)
    {
        isMoving = true;
        Vector3 initialPos = this.transform.position;

        this.transform.right = direction;

        for (float t = 0; t <= _duration; t += Time.deltaTime)
        {
            float percent = (t / _duration);

            this.transform.position = Vector3.Lerp(initialPos, initialPos + direction * MovementDistance,
                interPolationCurve.Evaluate(percent));

            if (percent < 0.13f)
                this.transform.localScale = new Vector3(1f + 30f * interPolationCurve.Evaluate(percent), 1, 1f-10f*interPolationCurve.Evaluate(percent));
            else
                this.transform.localScale = initialScale;

            yield return new WaitForEndOfFrame();
        }

        this.transform.position = Vector3.Lerp(initialPos, initialPos + direction * MovementDistance,
            interPolationCurve.Evaluate(1));

        this.transform.localScale = initialScale;

        isMoving = false;
    }
}