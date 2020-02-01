using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXSlice : MonoBehaviour
{
    private Asteroid _asteroid;

    private ShipManager ship;

    private bool markForSlicing;
    private float timeOfSlicing;
    public ParticleSystem sliceEffect;

    public GameObject hideGraphic;
    // Start is called before the first frame update
    void Start()
    {

        ship = GameObject.FindWithTag(Tags.SHIP).GetComponent<ShipManager>();
        _asteroid = GetComponent<Asteroid>();
        _asteroid.OnSlice.AddListener(MarkSlicing);
        
    }

    private void Update()
    {
        if (markForSlicing)
        {
            PlaySliceEffect();
            markForSlicing = false;
        }
    }

    private void MarkSlicing()
    {
        markForSlicing = true;
    }
    // Update is called once per frame
    void PlaySliceEffect()
    {
        Vector3 dir = ship.transform.position -  transform.position ;
        sliceEffect.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        sliceEffect.Play(true);
        hideGraphic.SetActive(false);
    }
}
