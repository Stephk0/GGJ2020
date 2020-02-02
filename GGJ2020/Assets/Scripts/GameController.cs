﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Image healthBar;
    public GameObject winScreen;
    public GameObject loseScreen;
    public ShipManager shipManager;
    public MaterialResourcePlacer resourcePlacer;
    public AsteroidLevelProfile[] levelProfiles;

    [Space(10)] 
    public bool healthUiFollow = true;
    [Space(10)]
    
    private AsteroidLevelProfile _activeLevelProfile;
    private bool _haveWon;
    private float _healthBarIncrement;
    

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Debug.Log("Game started");
    }

    private void Init()
    {
        var difficulty = DifficultyController.difficulty;
        if (difficulty >= levelProfiles.Length)
            difficulty = levelProfiles.Length - 1;
        
        _activeLevelProfile = levelProfiles[difficulty];
        
        resourcePlacer = FindObjectOfType<MaterialResourcePlacer>();
        shipManager = FindObjectOfType<ShipManager>();

        _healthBarIncrement = 1f / _activeLevelProfile.initialHealth;
        
        shipManager.health.health = _activeLevelProfile.initialHealth;
        shipManager.health.healthDecreaseAmount = _activeLevelProfile.healthDecreasePerTick;
        shipManager.health.timeToDecrease = _activeLevelProfile.healthDecreaseTickTime;
        
        shipManager.health.IsDestroyed += LevelLoss;
        shipManager.health.DecreasingHealth += UpdateHealthBar;
        UpdateHealthBar();

        if(healthUiFollow)
            healthBar.rectTransform.localScale = Vector3.one * 0.2f;
        
        resourcePlacer.SetupMaterialResources(_activeLevelProfile);
    }

    void Update()
    {
        if(healthUiFollow)
            AlignHealthBarToShip();

        if (DifficultyController.collectedMaterials < DifficultyController.winValue)
            return;
        
        DifficultyController.difficulty += 1;
        LevelWin();
    }

    private void AlignHealthBarToShip()
    {
        var healthPos = Camera.main.WorldToScreenPoint(shipManager.health.uiHealthAnchor.position);
        healthBar.rectTransform.position = healthPos;
    }

    private void UpdateHealthBar()
    {
        var amount = shipManager.health.health * _healthBarIncrement;

        if (amount <= 0.25f){
            healthBar.color = Color.red;
        }
        
        healthBar.fillAmount = amount;
    }

    private void LevelWin()
    {
        winScreen.SetActive(true);
    }

    private void LevelLoss()
    {
        loseScreen.SetActive(true);
    }

}
