using System.Runtime.CompilerServices;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Vector2 _clickDirection;

    private void OnDrawGizmos()
    {
        if (_clickDirection.magnitude != 0)
        {
            Gizmos.DrawLine(this.transform.position,
                (transform.position + Vector3.right * _clickDirection.x + Vector3.forward * _clickDirection.y) * 100);
        }
    }

    private void Update()
    {
        if (!Application.isMobilePlatform)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 InputPosition = Input.mousePosition;
                _clickDirection = ComputeClickDirection(InputPosition);
            }
            else
            {
                _clickDirection = Vector2.zero;
            }
        }
        else
        {
            Debug.LogWarning("Nothing implemented for mobile");
        }
    }

    private Vector2 ComputeClickDirection(Vector3 pixelCoords)
    {
        Vector3 shipViewportPos = _camera.WorldToScreenPoint(this.transform.position);
        Vector2 direction = (pixelCoords - shipViewportPos);

        return direction.normalized;
    }

    public Vector2 GetClickDirection()
    {
        return this._clickDirection;
    }
}