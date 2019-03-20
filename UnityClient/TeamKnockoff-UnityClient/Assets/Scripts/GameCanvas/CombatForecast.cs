using Assets.Scripts.View;
using Assets.Scripts.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class CombatForecast : MonoBehaviour
{
    public TextMeshProUGUI playerNameLabel;
    public TextMeshProUGUI playerCurrentHpLabel;
    public TextMeshProUGUI playerMaxHpLabel;
    public TextMeshProUGUI playerHitLabel;
    public TextMeshProUGUI playerOffensiveLabel;
    public TextMeshProUGUI playerDefensiveLabel;
    public TextMeshProUGUI playerCritLabel;

    public TextMeshProUGUI enemyUnitNameLabel;
    public TextMeshProUGUI enemyCurrentHpLabel;
    public TextMeshProUGUI enemyMaxHpLabel;
    public TextMeshProUGUI enemyHitLabel;
    public TextMeshProUGUI enemyOffensiveLabel;
    public TextMeshProUGUI enemyDefensiveLabel;
    public TextMeshProUGUI enemyCritLabel;

    public GameView gameView;
    private GameViewModel gameViewModel;
  
    public void ConstructCombatForecast()
    {
        gameViewModel = gameView.gameViewModel;
        gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;
    }

    private void GameViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "CombatMode")
        {
            if (gameViewModel.CombatMode == true)
            {
                this.enabled = true;
            }
            else
            {
                this.enabled = false;
            }
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
