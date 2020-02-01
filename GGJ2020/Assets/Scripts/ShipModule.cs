using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ShipModule : MonoBehaviour
{
    public Connector[] connectors;

    private Rigidbody _body;
    
    void Start()
    {
        Init();
    }

    void Update()
    {
        
    }

    private void Init()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision mover)
    {
        
    }

    private void OnCollisionExit(Collision mover)
    {
        
    }
}
