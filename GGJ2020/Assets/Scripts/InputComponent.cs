using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;

public class InputComponent : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Vector2 _clickDirection;

    private void Update()
    {
        if (!Application.isMobilePlatform)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 InputPosition = Input.mousePosition;
                _clickDirection = ComputeClickDirection(InputPosition);
                _camera.DOShakePosition(0.2f, 0.5f, 6);
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