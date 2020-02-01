using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum ShipMovementPhases
{
    Idle = 0,
    BuildUp = 1,
    Moving = 2,
    Resting = 3
}

public class MovementComponent : MonoBehaviour
{
    public float TotalMovementDistance = 10;
    public float MovementPercent { get; private set; } = 0;
    public float TotalCurveTime { get; private set; } = 0f;

    [SerializeField] Transform _shipMeshTransform;
    [SerializeField] private ShipManager _manager;

    [SerializeField] private List<MovementPhaseData> _phasesData;

    private Vector3 _initialScale;

    private Coroutine _movementCoroutine = null;
    private ShipMovementPhases _movementPhases = ShipMovementPhases.Idle;


    private void Start()
    {
        TotalCurveTime = _phasesData.Sum(x => x.InterpolationDurationTime);
        TotalMovementDistance = _phasesData.Sum(x => x.TravelDistance);

        _initialScale = this._shipMeshTransform.localScale;
    }

    private void Update()
    {
        Vector3 movementDirection = _manager.GetMovementDirection();
        TryToMoveInPhases(movementDirection);
    }

    private void TryToMoveInPhases(Vector3 movementDirection)
    {
        if (!movementDirection.sqrMagnitude.Equals(0) &&
            (this._movementPhases == ShipMovementPhases.Idle ||
             this._movementPhases == ShipMovementPhases.Resting))
        {
            if (_movementCoroutine != null)
            {
                StopAllCoroutines();
                if (_movementPhases > 0)
                {
                    _manager.OnShipFinishMovement.Invoke();
                    this._shipMeshTransform.localScale = this._initialScale;
                }
            }

            _movementCoroutine = StartCoroutine(MoveShip(_manager.GetMovementDirection()));
        }
    }

    IEnumerator MoveShip(Vector3 direction)
    {
        this.transform.forward = direction;

        _manager.OnShipStartMovement.Invoke();

        foreach (var data in _phasesData)
        {
            if (data.InterpolationPhase == ShipMovementPhases.BuildUp)
            {
                StartCoroutine(ScalingRoutine(data));
            }

            yield return StartCoroutine(DisplacementCoroutine(data, direction));
            MovementPercent = data.TravelDistance / TotalCurveTime;
        }

        _manager.OnShipFinishMovement.Invoke();
    }

    private IEnumerator ScalingRoutine(MovementPhaseData movementData)
    {
        float routineDuration = movementData.InterpolationDurationTime;

        for (float dt = 0; dt <= movementData.InterpolationDurationTime; dt += Time.deltaTime)
        {
            float percent = Mathf.Clamp01(dt / routineDuration);
            var interpolationCurve = movementData.InterPolationCurve;
            this._shipMeshTransform.localScale = new Vector3(
                1f - .5f * interpolationCurve.Evaluate(percent), 1,
                1f + 2f * interpolationCurve.Evaluate(percent));
            yield return new WaitForEndOfFrame();
        }

        this._shipMeshTransform.localScale = this._initialScale;
    }

    private IEnumerator DisplacementCoroutine(MovementPhaseData movementData, Vector3 direction)
    {
        _movementPhases = movementData.InterpolationPhase;
        Vector3 initialPos = this.transform.position;
        float routineDuration = movementData.InterpolationDurationTime;
        AnimationCurve interpolationCurve = movementData.InterPolationCurve;

        Vector3 finalPosition = initialPos + GetDisplacementVector(direction, movementData.TravelDistance);
        float movementPercentStep = movementData.TravelDistance / TotalMovementDistance;
        for (float dt = 0; dt <= routineDuration; dt += Time.deltaTime)
        {
            float percent = Mathf.Clamp01(dt / routineDuration);
            MovementPercent += movementPercentStep * percent;
            _manager.OnShipMoving.Invoke();
            this.transform.position = Vector3.Lerp(initialPos, finalPosition, interpolationCurve.Evaluate(percent));
            yield return new WaitForEndOfFrame();
        }
    }

    public Vector3 GetTotalDisplacementVector(Vector3 direction)
    {
        return (direction * TotalMovementDistance);
    }

    public Vector3 GetDisplacementVector(Vector3 direction, float length)
    {
        return (direction * length);
    }
}

[Serializable]
public class MovementPhaseData
{
    [SerializeField] private AnimationCurve interPolationCurve;
    [SerializeField] private float interpolationDurationTime;
    [SerializeField] private float travelDistance;
    [SerializeField] private ShipMovementPhases interpolationPhase;


    public AnimationCurve InterPolationCurve
    {
        get => interPolationCurve;
        set => interPolationCurve = value;
    }

    public float InterpolationDurationTime
    {
        get => interpolationDurationTime;
        set => interpolationDurationTime = value;
    }

    public float TravelDistance
    {
        get => travelDistance;
        set => travelDistance = value;
    }

    public ShipMovementPhases InterpolationPhase
    {
        get => interpolationPhase;
        set => interpolationPhase = value;
    }
}
//            if (MovementPercent < _manager.BuildUpThreshHold)
//                this._shipMeshTransform.localScale = new Vector3(
//                    1f - 10f * interpolationCurve.Evaluate(MovementPercent), 1,
//                    1f + 30f * interpolationCurve.Evaluate(MovementPercent));
//            else
//                this._shipMeshTransform.localScale = _initialScale;