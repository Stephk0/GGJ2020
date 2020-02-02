using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


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

    public ShipMovementPhases MovementPhases
    {
        get => _movementPhases;
    }

    [SerializeField] Transform _shipMeshTransform;
    [SerializeField] private ShipManager _manager;
    [SerializeField] private LayerMask _shipRouteCollision;
    [SerializeField] private List<MovementPhaseData> _phasesData;

    private Vector3 _initialScale;

    private Coroutine _movementCoroutine = null;
    private ShipMovementPhases _movementPhases = ShipMovementPhases.Idle;
    private Vector3 initialDirection;

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
            (this.MovementPhases == ShipMovementPhases.Idle ||
             this.MovementPhases == ShipMovementPhases.Resting))
        {
            if (_movementCoroutine != null)
            {
                StopAllCoroutines();
                if (MovementPhases > 0)
                {
                    _manager.OnShipFinishMovement.Invoke();
                    this._shipMeshTransform.localScale = this._initialScale;
                }
            }

            initialDirection = _manager.GetMovementDirection();
            _movementCoroutine = StartCoroutine(MoveShip());
        }
    }

    IEnumerator MoveShip()
    {
        _manager.OnShipStartMovement.Invoke();

        MovementPercent = 0;
        foreach (var data in _phasesData)
        {
            bool isbuildUp = data.InterpolationPhase == ShipMovementPhases.BuildUp;
            if (isbuildUp)
            {
                StartCoroutine(ScalingRoutine(data));
            }

            yield return StartCoroutine(DisplacementCoroutine(data, !isbuildUp));
            MovementPercent = data.TravelDistance / TotalCurveTime;
        }

        _manager.OnShipFinishMovement.Invoke();
    }

    private IEnumerator ScalingRoutine(MovementPhaseData movementData)
    {
        var routineDuration = movementData.InterpolationDurationTime;

        for (float dt = 0; dt <= movementData.InterpolationDurationTime; dt += Time.deltaTime)
        {
            var percent = Mathf.Clamp01(dt / routineDuration);
            var interpolationCurve = movementData.InterPolationCurve;
            this._shipMeshTransform.localScale = new Vector3(
                1f - .5f * interpolationCurve.Evaluate(percent), 1,
                1f + 2f * interpolationCurve.Evaluate(percent));
            yield return new WaitForEndOfFrame();
        }

        this._shipMeshTransform.localScale = this._initialScale;
    }

    private IEnumerator DisplacementCoroutine(MovementPhaseData movementData, bool clamps = true)
    {
        _movementPhases = movementData.InterpolationPhase;
        Vector3 initialPos = this.transform.position;
        Vector3 finalPosition = initialPos + GetDisplacementVector(initialDirection, movementData.TravelDistance);

        float routineDuration = movementData.InterpolationDurationTime;

        AnimationCurve interpolationCurve = movementData.InterPolationCurve;
        List<Vector3> Route = new List<Vector3>();

        if (Camera.main.IsPointInVisible(finalPosition))
        {
            Route = PlanRoute(initialPos, finalPosition);
        }
        else
        {
            Route.Add(finalPosition);
        }

        for (float dt = 0; dt <= routineDuration; dt += Time.deltaTime)
        {
            var percent = Mathf.Clamp01(dt / routineDuration);

            _manager.OnShipMoving.Invoke();
            this.transform.position = GetPointInRoute(percent, initialPos, Route, interpolationCurve);

            yield return new WaitForEndOfFrame();
        }
    }

    private Vector3 GetPointInRoute(float currentPercent, Vector3 initial, List<Vector3> path, AnimationCurve animation)
    {
        float accumulatedpercent = 0;
        Vector3 ini = this.transform.position;
        for (int i = 0; i < path.Count; i++)
        {
            Vector3 currentTarget = path[i];

            Debug.DrawLine(ini, currentTarget, i == 0 ? Color.magenta : i == 1 ? Color.red : Color.cyan, 3);
            ini = currentTarget;
        }

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 currentTarget = path[i];
            Vector3 DisplacementVector = currentTarget - initial;

            float distance = DisplacementVector.magnitude;
            float previousPercent = accumulatedpercent;
            accumulatedpercent += distance / TotalMovementDistance;
            float percentReRanged = (currentPercent - previousPercent) / (accumulatedpercent - previousPercent);
            Mathf.Clamp01(percentReRanged);
            if (currentPercent <= accumulatedpercent)
            {
                Vector3 dir = (currentTarget - initial).normalized;
                this.transform.forward = dir;
                this.initialDirection = dir;
                return Vector3.Lerp(initial, currentTarget, animation.Evaluate(percentReRanged));
            }

            initial = currentTarget;
        }

        return Vector3.Lerp(initial, path.Last(), animation.Evaluate(1));
    }

    private List<Vector3> PlanRoute(Vector3 initial, Vector3 final)
    {
        var distanceVector = (final - initial);
        float totalDistance = distanceVector.magnitude;
        Ray r = new Ray(initial, distanceVector.normalized);
        RaycastHit hit;
        int bounces = 0;
        List<Vector3> hitPoints = new List<Vector3>();

        for (int i = 0; i < 3; i++)
        {
            if (Physics.Raycast(r, out hit, 1000, _shipRouteCollision))
            {
                Vector3 d = r.direction;
                Vector3 reflected = Vector3.Reflect(d, hit.normal);
                if (hit.distance > totalDistance)
                {
                    hitPoints.Add(r.GetPoint(totalDistance));
                    break;
                }

                hitPoints.Add(hit.point);
                totalDistance -= hit.distance;


                // Debug.DrawLine(r.origin, hit.point, Color.red, 10);
                r = new Ray(hit.point - reflected * 0.1f, reflected);
            }
            else
            {
                Debug.Log(i);
                break;
            }
        }

        return hitPoints;
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