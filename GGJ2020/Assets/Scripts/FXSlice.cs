using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSlice : MonoBehaviour
{
    private Asteroid _asteroid;

    private ShipManager ship;
    public ParticleSystem sliceEffect;
    // Start is called before the first frame update
    void Start()
    {

        ship = GameObject.FindWithTag(Tags.SHIP).GetComponent<ShipManager>();
        _asteroid = GetComponent<Asteroid>();
        _asteroid.OnSlice.AddListener(PlaySliceEffect);
        
    }

    // Update is called once per frame
    void PlaySliceEffect()
    {
        Vector3 dir = ship.transform.position -  transform.position ;
        sliceEffect.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        sliceEffect.Play(true);
    }
}
