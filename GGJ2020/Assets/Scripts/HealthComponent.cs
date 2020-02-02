using System;

using UnityEngine;
using UnityEngine.Serialization;

public class HealthComponent : MonoBehaviour
{
    public int health;
    public int healthDecreaseAmount = 4;
    public float timeToDecrease = 0.5f;
    public bool decreaseHealth = true;

    [FormerlySerializedAs("DecreasingHealth")] public Action ApplyingHealth;
    public Action IsDestroyed;

    [FormerlySerializedAs("UiHealthReference")] [Space(10)] 
    public Transform uiHealthAnchor;

    public float currentTime;
    public int maxHealth;
    private void Start()
    {
        maxHealth = health;
    }

    void Update()
    {
        if (decreaseHealth){
            Timers();
        }
    }
    
    public void Apply(int amount)
    {
        //Can be negative or Positive amount
        health -= amount;
        if (health > maxHealth)
            health = maxHealth;
        ApplyingHealth?.Invoke();
        
        if(IsDeathReady())
            DestroyShip();
    }

    private void Timers()
    {
        currentTime += Time.deltaTime;

        if (!(currentTime > timeToDecrease)) 
            return;
        
        currentTime = 0f;
        Apply(healthDecreaseAmount);
    }


    private bool IsDeathReady()
    {
        return health <= 0;
    }

    public void DestroyShip()
    {
        health = 0;
        IsDestroyed?.Invoke();
        Debug.Log("Ship Destroyed");
    }
}
