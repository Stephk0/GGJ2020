using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public int health = 100;
    public int healthDecrease = 4;
    public float timeToDecrease = 0.5f;
    public bool decreaseHealth = true;
    public Action IsDestroyed;

    private float currentTime;

    void Update()
    {
        if (decreaseHealth)
        {
            Timers();
        }
    }

    public void Decrease(int amount)
    {
        health -= amount;

        if (IsDeathReady())
            DestroyShip();
    }

    private void Timers()
    {
        currentTime += Time.deltaTime;

        if (!(currentTime > timeToDecrease))
            return;

        currentTime = 0f;
        Decrease(healthDecrease);
    }


    private bool IsDeathReady()
    {
        return health <= 0;
    }

    private void DestroyShip()
    {
        IsDestroyed?.Invoke();
        //Debug.Log("Ship Destroyed");
    }
}