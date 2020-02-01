using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;

enum AsteroidType
{
    DestroyableAsteroid, UnbreakableAsteroid
}

public class Asteroid : MonoBehaviour, ISliceable
{
    [SerializeField] private AsteroidType type;
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _leftPiece;
    [SerializeField] private Rigidbody _rightPiece;
    [SerializeField] private float _force;
    [SerializeField] private float angle = 15;

    public UnityEvent OnSlice;
    
    private bool isSliced;

    public void OnSliced(Vector3 startPosition, Vector3 hitPosition)
    {
        if (isSliced) {
            return;
        }
        
        OnSlice.Invoke();
        isSliced = true;
        Vector3 direction = hitPosition - startPosition;
        direction.Normalize();
        
        Quaternion left = Quaternion.Euler(0, -angle, 0);
        Quaternion right  = Quaternion.Euler(0, angle, 0);
        AddForceToPiece(_leftPiece, (left * direction), hitPosition);
        AddForceToPiece(_rightPiece, (right * direction), hitPosition);
        SpawnMaterials();
    }

    private void AddForceToPiece(Rigidbody piece, Vector3 direction, Vector3 hitPosition)
    {
        Debug.DrawLine(hitPosition, hitPosition + direction, Color.magenta, 2f);
        piece.isKinematic = false;
        piece.AddForce(direction * _force, ForceMode.Impulse);
    }

    private void SpawnMaterials()
    {
        var materialResource = GetComponent<MaterialResource>();
    }
    
    public int Amount
    {
        get => _amount;
        set => _amount = value;
    }

    private int _amount;
}
