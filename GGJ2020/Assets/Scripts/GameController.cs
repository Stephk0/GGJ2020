using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Image healthBar;
    public Image timeBar;
    public Text wrenchCount;
    public GameObject winScreen;
    public Text winText;
    public GameObject loseScreen;
    public Text loseText;
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
        Debug.Log("Level Start: Difficulty" + DifficultyController.difficulty);
        Init();
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
        shipManager.health.ApplyingHealth += UpdateHealthBar;
        UpdateHealthBar();

        if(healthUiFollow)
            healthBar.rectTransform.localScale = Vector3.one * 0.2f;
        
        resourcePlacer.SetupMaterialResources(_activeLevelProfile);
    }

    void Update()
    {
        if(healthUiFollow)
            AlignHealthBarToShip();

        UpdateTimeBar();
        UpdateWrenchCount();

        if (DifficultyController.collectedMaterials >= DifficultyController.winValue){
            if (_haveWon == false)
            {
                _haveWon = true;
                LevelWin();
            }
        }
    }

    private void UpdateHealthBar()
    {
        var amount = shipManager.health.health * _healthBarIncrement;
        healthBar.fillAmount = amount;
        
        if (amount <= 0.25f){
            healthBar.color = Color.red;
        }
    }

    private void UpdateTimeBar()
    {
        var amount = 1f - shipManager.health.currentTime;
        timeBar.fillAmount = amount;
    }
    
    private void UpdateWrenchCount()
    {
        wrenchCount.text = DifficultyController.collectedMaterials.ToString() + "/" +
                           DifficultyController.winValue.ToString();
    }

    private void LevelWin()
    {
        Debug.Log("LevelWin(Difficulty:" + DifficultyController.difficulty);
        DifficultyController.difficulty += 1;
        winScreen.SetActive(true);
        winText.text = "-- sector " + (DifficultyController.difficulty) + " clear --";
    }

    private void LevelLoss()
    {
        Debug.Log("LevelLoss()");
        loseScreen.SetActive(true);
        loseText.text = "-- se.ctor " + (DifficultyController.difficulty) + " E/RRO;R --";
    }
    
    private void AlignHealthBarToShip()
    {
        var healthPos = Camera.main.WorldToScreenPoint(shipManager.health.uiHealthAnchor.position);
        healthBar.rectTransform.position = healthPos;
    }
}
