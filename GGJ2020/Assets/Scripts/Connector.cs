using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Connector : MonoBehaviour
{
    public Transform anchor;
    public Transform hook;

    public LineRenderer line;

    private bool _initialized;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (_initialized)
        {
            LinePositions();
        }
    }

    private void OnValidate()
    {
        Init();
    }

    private void Init()
    {
        if (line == null)
        {
            GetComponentInChildren<LineRenderer>();
        }

        LinePositions();
        _initialized = true;
    }

    private void LinePositions()
    {
        //line.SetPosition(0, anchor.GetChild(0).position);
        //line.SetPosition(1, hook.GetChild(0).position);
    }
}