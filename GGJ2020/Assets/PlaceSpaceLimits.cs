using System;
using UnityEngine;

public class PlaceSpaceLimits : MonoBehaviour
{
    [SerializeField] GameObject[] _planes;
    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _layer;

    [ContextMenu("locatePlanes")]
    public void LocateCameraPlanes()
    {
        float w = Screen.width;
        float h = Screen.height;
        Bounds b = new Bounds(new Vector3(w * 0.5f, h * 0.5f, w), new Vector3(w, h, w));

        Vector3[] bounds = _cam.GetWorldBounds(5f);

        float depth = Mathf.Abs(bounds[0].z - bounds[1].z);
        float Width = Mathf.Abs(bounds[1].x - bounds[2].x);
        _planes[0].transform.position = Vector3.Lerp(bounds[0], bounds[1], 0.5f);
        _planes[1].transform.position = Vector3.Lerp(bounds[1], bounds[2], 0.5f);
        _planes[2].transform.position = Vector3.Lerp(bounds[2], bounds[3], 0.5f);
        _planes[3].transform.position = Vector3.Lerp(bounds[3], bounds[0], 0.5f);

        _planes[0].transform.position += -Vector3.up * 50;
        _planes[1].transform.position += -Vector3.up * 50;
        _planes[2].transform.position += -Vector3.up * 50;
        _planes[3].transform.position += -Vector3.up * 50;

        _planes[0].transform.rotation = Quaternion.Euler(90, 90, 0);
        _planes[1].transform.rotation = Quaternion.Euler(90, 0, 0);
        _planes[2].transform.rotation = Quaternion.Euler(90, 90, 0);
        _planes[3].transform.rotation = Quaternion.Euler(90, 0, 0);

        _planes[0].transform.localScale = new Vector3(100, 2, 14);
        _planes[1].transform.localScale = new Vector3(100, 2, 14);
        _planes[2].transform.localScale = new Vector3(100, 2, 14);
        _planes[3].transform.localScale = new Vector3(100, 2, 14);
    }

    public void Start()
    {
        LocateCameraPlanes();
    }

    public bool IsPointInVisible(Vector3 worldPoint)
    {
        Camera cam = Camera.main;

        GetMinMaxPlane(out var Min, out var Max);

        Max = cam.WorldToScreenPoint(Max);
        Min = cam.WorldToScreenPoint(Min);
        var viewpoint = cam.WorldToScreenPoint(worldPoint);

        bool outside = viewpoint.x > Max.x ||
                       viewpoint.x < Min.x ||
                       viewpoint.y > Max.y ||
                       viewpoint.y < Min.y;
        return outside;
    }

    private void GetMinMaxPlane(out Vector3 Min, out Vector3 Max)
    {
        Max = Vector3.one * float.MinValue;
        Min = Vector3.one * float.MaxValue;
        foreach (var plane in _planes)
        {
            Max = Vector3.Max(plane.transform.position, Max);
            Min = Vector3.Min(plane.transform.position, Min);
        }
    }

    public Vector3 ClampToBoundsXZ(Vector3 point, float radius)
    {
        GetMinMaxPlane(out var Min, out var Max);

        point.x = Mathf.Clamp(point.x, Min.x + radius, Max.x - radius);
        point.z = Mathf.Clamp(point.z, Min.z + radius, Max.z - radius);
        return point;
    }
}

public static class CameraExtensions
{
    public static Vector3[] GetWorldBounds(this Camera cam, float edgeBufferRange)
    {
        float _depth = cam.transform.position.y - edgeBufferRange;

        var _screenCorners = new[]
        {
            new Vector3(0, 0, _depth), //bottom left
            new Vector3(0, 1, _depth), //top lef
            new Vector3(1, 1, _depth), //top right
            new Vector3(1, 0, _depth)
        }; //bottom right;

        var worldCorners = new Vector3[_screenCorners.Length];

        for (var i = 0; i < _screenCorners.Length; i++)
        {
            var corner = Camera.main.ViewportToWorldPoint(_screenCorners[i]);
            corner.y = 0f;
            worldCorners[i] = corner;

            //Debug.Log($"Corner {i} position: {worldCorners[i]}");
            Debug.DrawRay(worldCorners[i], Vector3.up * 10f, Color.white, 5f);
        }

        return worldCorners;
    }
}