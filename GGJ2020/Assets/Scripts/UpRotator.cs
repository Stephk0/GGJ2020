using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpRotator : MonoBehaviour
{
    public float roationSpeed = 0.2f;
    public bool stop;

    void Update()
    {
        if (stop) return;
        Spin();
    }

    private void Spin()
    {
        transform.RotateAround(transform.position, Vector3.up, roationSpeed);
    }
}
