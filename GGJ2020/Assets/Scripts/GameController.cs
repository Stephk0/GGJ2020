using System;
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
        _activeLevelProfile = levelProfiles[DifficultyController.difficulty];
        
        resourcePlacer = FindObjectOfType<MaterialResourcePlacer>();
        shipManager = FindObjectOfType<ShipManager>();

        _healthBarIncrement = 1f / _activeLevelProfile.initialHealth;
        
        shipManager.health.health = _activeLevelProfile.initialHealth;
        shipManager.health.healthDecreaseAmount = _activeLevelProfile.healthDecreasePerTick;
        shipManager.health.timeToDecrease = _activeLevelProfile.healthDecreaseTickTime;
        
        shipManager.health.IsDestroyed += LevelLoss;
        shipManager.health.DecreasingHealth += UpdateHealthBar;
        UpdateHealthBar();
        
        resourcePlacer.SetupMaterialResources(_activeLevelProfile);
    }

    void Update()
    {
        if (DifficultyController.collectedMaterials < DifficultyController.winValue)
            return;
        
        DifficultyController.difficulty += 1;
        LevelWin();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = shipManager.health.health * _healthBarIncrement;
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
