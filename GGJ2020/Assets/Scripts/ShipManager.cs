using UnityEngine;
using UnityEngine.Events;

public class ShipManager : MonoBehaviour
{
    [SerializeField] private InputComponent _input;
    [SerializeField] private MovementComponent _movement;
    [SerializeField] private float _actionDistance;
    public HealthComponent health;
    public MaterialCollector collector;

    public UnityEvent OnShipStartMovement;
    public UnityEvent OnShipFinishMovement;
    public UnityEvent OnShipMoving;

    public float BuildUpThreshHold = 0.13f;

    public void Init()
    {
        if (health == null)
            health = GetComponentInChildren<HealthComponent>();

        if (collector == null)
            collector = GetComponentInChildren<MaterialCollector>();
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
}

public static class VectorExtensions
{
    public static Vector3 ToXZPlane(this Vector2 position)
    {
        return new Vector3(position.x, 0, position.y);
    }
}