using System;
using System.Collections;
using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    public float MovementDistance = 10;
    public float MovementPercent { get; private set; } = 0;

    [SerializeField] AnimationCurve interPolationCurve;
    [SerializeField] Transform _shipMeshTransform;
    [SerializeField] private ShipManager _manager;
    [SerializeField] private float _duration = 0.5f;

    private Vector3 _initialScale;

    private Coroutine _movementCoroutine = null;

    private bool IsMoving { get; set; } = false;

    private void Start()
    {
        _initialScale = this._shipMeshTransform.localScale;
        _manager.OnShipFinishMovement.AddListener(() =>
        {
            IsMoving = false;
            MovementPercent = 0;
        });
        _manager.OnShipStartMovement.AddListener(() => { IsMoving = true; });
    }

    private void Update()
    {
        Vector3 movementDirection = _manager.GetMovementDirection();
        if (!movementDirection.sqrMagnitude.Equals(0))
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

        this.transform.forward = direction;

        _manager.OnShipStartMovement.Invoke();
        
        for (float t = 0; t <= _duration; t += Time.deltaTime)
        {
            _manager.OnShipMoving.Invoke();
            
            MovementPercent = (t / _duration);
            this.transform.position = Vector3.Lerp(initialPos, initialPos + GetDisplacementVector(direction),
                interPolationCurve.Evaluate(MovementPercent));

            if (MovementPercent < _manager.BuildUpThreshHold)
                this._shipMeshTransform.localScale = new Vector3(
                    1f - 10f * interPolationCurve.Evaluate(MovementPercent), 1,
                    1f + 30f * interPolationCurve.Evaluate(MovementPercent));
            else
                this._shipMeshTransform.localScale = _initialScale;

            yield return new WaitForEndOfFrame();
        }

        MovementPercent = 1;
        this.transform.position = Vector3.Lerp(initialPos, initialPos + direction * MovementDistance,
            interPolationCurve.Evaluate(MovementPercent));

        this._shipMeshTransform.localScale = _initialScale;
        
        _manager.OnShipFinishMovement.Invoke();
    }

    public Vector3 GetDisplacementVector(Vector3 direction)
    {
        return (direction * MovementDistance);
    }
}