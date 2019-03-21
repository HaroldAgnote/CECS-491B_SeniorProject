using Assets.Scripts.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.View
{
    public class CombatForecast : MonoBehaviour
    {
        public TextMeshProUGUI playerNameLabel;
        public TextMeshProUGUI playerCurrentHpLabel;
        public TextMeshProUGUI playerMaxHpLabel;
        public TextMeshProUGUI playerHitLabel;
        public TextMeshProUGUI playerOffensiveLabel;
        public TextMeshProUGUI playerDefensiveLabel;
        public TextMeshProUGUI playerCritLabel;

        public TextMeshProUGUI enemyNameLabel;
        public TextMeshProUGUI enemyCurrentHpLabel;
        public TextMeshProUGUI enemyMaxHpLabel;
        public TextMeshProUGUI enemyHitLabel;
        public TextMeshProUGUI enemyOffensiveLabel;
        public TextMeshProUGUI enemyDefensiveLabel;
        public TextMeshProUGUI enemyCritLabel;

        public GameView gameView;
        public DamageCalculator damageCalculator;
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
                    gameObject.SetActive(true);
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }

            //change label depending on info on unit
            if (e.PropertyName == "TargetSquare")
            {
                var playerUnit = gameViewModel.SelectedSquare.Unit;
                var targetSquare = gameViewModel.TargetSquare;

                if (gameViewModel.CombatMode == true)
                {
                    if (targetSquare == null) {
                        playerNameLabel.text = playerUnit.Name;
                        playerCurrentHpLabel.text = $"CUR {playerUnit.HealthPoints}";
                        playerMaxHpLabel.text = $"MAX {playerUnit.MaxHealthPoints}";
                        playerHitLabel.text = $"HIT ";
                        playerOffensiveLabel.text = $"OFF ";
                        playerDefensiveLabel.text = $"DEF ";
                        playerCritLabel.text = $"CRT ";

                        enemyNameLabel.text = "";
                        enemyCurrentHpLabel.text = "";
                        enemyMaxHpLabel.text = "";
                        enemyHitLabel.text = "";
                        enemyOffensiveLabel.text = "";
                        enemyDefensiveLabel.text = "";
                        enemyCritLabel.text = "";
                    } else {
                        if (gameViewModel.EnemyAtPoint(targetSquare.Position) == true) {
                            var enemyUnit = targetSquare.Unit;

                            if (enemyUnit != null) {
                                playerNameLabel.text = playerUnit.Name;
                                playerCurrentHpLabel.text = $"CUR {playerUnit.HealthPoints}";
                                playerMaxHpLabel.text = $"MAX {playerUnit.MaxHealthPoints}";
                                playerHitLabel.text = $"HIT {DamageCalculator.GetHitChance(playerUnit, enemyUnit)}";
                                playerOffensiveLabel.text = $"OFF {DamageCalculator.GetOffensive(playerUnit)}";
                                playerDefensiveLabel.text = $"DEF {DamageCalculator.GetDefensive(enemyUnit, playerUnit)}";
                                playerCritLabel.text = $"CRT {DamageCalculator.GetCritRate(playerUnit, enemyUnit)}";

                                enemyNameLabel.text = enemyUnit.Name;
                                enemyCurrentHpLabel.text = $"CUR {enemyUnit.HealthPoints}";
                                enemyMaxHpLabel.text = $"MAX {enemyUnit.MaxHealthPoints}";
                                enemyHitLabel.text = $"HIT {DamageCalculator.GetHitChance(enemyUnit, playerUnit)}";
                                enemyOffensiveLabel.text = $"OFF {DamageCalculator.GetOffensive(enemyUnit)}";
                                enemyDefensiveLabel.text = $"DEF {DamageCalculator.GetDefensive(playerUnit, enemyUnit)}";
                                enemyCritLabel.text = $"CRT {DamageCalculator.GetCritRate(enemyUnit, playerUnit)}";
                            }
                        }
                    }
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
}