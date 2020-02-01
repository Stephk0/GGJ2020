using DefaultNamespace;
using UnityEngine;

public class Asteroid : MonoBehaviour, ISliceable
{
    [SerializeField] private Collider _collider;
    [SerializeField] private Rigidbody _leftPiece;
    [SerializeField] private Rigidbody _rightPiece;
    [SerializeField] private float _force;

    private bool isSliced;
    
    /*private Vector3 enterPosition;
    private float enterTime;
    private Vector3 exitPosition;

    private void OnCollisionEnter(Collision other)
    {
        enterPosition = other.transform.position;
        enterTime = Time.time;
    }

    private void OnCollisionExit(Collision other)
    {
        exitPosition = other.transform.position;
        Slice();
    }

    private void Slice()
    {
        if (isSliced) {
            return;
        }
        
        isSliced = true;
        Vector3 direction = exitPosition - enterPosition;
        direction.Normalize();
    
        AddForceToPiece(_leftPiece, direction * _force);
        AddForceToPiece(_rightPiece, direction * _force);
    } */
    
    public void OnSliced(Vector3 entry, Vector3 exit)
    {
        if (isSliced) {
            return;
        }
        
        isSliced = true;
        Vector3 direction = exit - entry;
        direction.Normalize();
        
        AddForceToPiece(_leftPiece, direction * _force);
        AddForceToPiece(_rightPiece, direction * _force);
    }

    private void AddForceToPiece(Rigidbody piece, Vector3 force)
    {
        piece.isKinematic = false;
        piece.AddForce(force);
    }
}
