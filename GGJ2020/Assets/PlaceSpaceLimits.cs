using System.Collections;
using System.Collections.Generic;
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

        Vector3[] bounds = _cam.GetWorldBounds();

        float depth = Mathf.Abs(bounds[0].z - bounds[1].z);
        float Width = Mathf.Abs(bounds[1].x - bounds[2].x);
        _planes[0].transform.position = Vector3.Lerp(bounds[0], bounds[1], 0.5f);
        _planes[1].transform.position = Vector3.Lerp(bounds[1], bounds[2], 0.5f);
        _planes[2].transform.position = Vector3.Lerp(bounds[2], bounds[3], 0.5f);
        _planes[3].transform.position = Vector3.Lerp(bounds[3], bounds[0], 0.5f);

        _planes[0].transform.rotation = Quaternion.Euler(90, 90, 0);
        _planes[1].transform.rotation = Quaternion.Euler(90, 0, 0);
        _planes[2].transform.rotation = Quaternion.Euler(90, 90, 0);
        _planes[3].transform.rotation = Quaternion.Euler(90, 0, 0);

        _planes[0].transform.localScale = new Vector3(10, 2, 14);
        _planes[1].transform.localScale = new Vector3(10, 2, 14);
        _planes[2].transform.localScale = new Vector3(10, 2, 14);
        _planes[3].transform.localScale = new Vector3(10, 2, 14);
    }
}