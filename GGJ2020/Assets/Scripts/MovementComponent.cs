using System;
using System.Collections;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    [SerializeField] AnimationCurve interPolationCurve;
    [SerializeField] private shipManager _manager;
    [SerializeField] private float _duration = 0.5f;
    public float MovementPercent { get; private set; } = 0;

    private Vector3 _initialScale = new Vector3(1, 1, 1);

    private Coroutine _movementCoroutine = null;

    private bool IsMoving { get; set; } = false;

    private void Start()
    {
        _manager.OnFinishMovement.AddListener(() =>
        {
            IsMoving = false;
            MovementPercent = 0;
        });
        _manager.OnStartMovement.AddListener(() => { IsMoving = true; });
    }

    private void Update()
    {
        Vector3 movementDirection = _manager.GetMovementDirection();
        if (!movementDirection.sqrMagnitude.Equals(0) && !IsMoving)
        {
            if (_movementCoroutine != null)
            {
                StopCoroutine(_movementCoroutine);
            }

            _movementCoroutine = StartCoroutine(MoveShip(_manager.GetMovementDirection()));
        }
    }

    IEnumerator MoveShip(Vector3 direction)
    {
        Vector3 initialPos = this.transform.position;

        this.transform.right = direction;

        _manager.OnStartMovement.Invoke();
        for (float t = 0; t <= _duration; t += Time.deltaTime)
        {
            MovementPercent = (t / _duration);

            this.transform.position = Vector3.Lerp(initialPos, initialPos + GetDisplacementVector(direction),
                interPolationCurve.Evaluate(MovementPercent));

            if (MovementPercent < 0.13f) {
                this.transform.localScale = new Vector3(1f + 30f * interPolationCurve.Evaluate(MovementPercent), 1,
                    1f - 10f * interPolationCurve.Evaluate(MovementPercent));
            }
            else
                this.transform.localScale = _initialScale;

            yield return new WaitForEndOfFrame();
        }

        MovementPercent = 1;
        this.transform.position = Vector3.Lerp(initialPos, initialPos + direction * _manager.GetActionDistance(),
            interPolationCurve.Evaluate(MovementPercent));

        this.transform.localScale = _initialScale;
        _manager.OnFinishMovement.Invoke();
    }

    public Vector3 GetDisplacementVector(Vector3 direction)
    {
        return (direction * _manager.GetActionDistance());
    }
}