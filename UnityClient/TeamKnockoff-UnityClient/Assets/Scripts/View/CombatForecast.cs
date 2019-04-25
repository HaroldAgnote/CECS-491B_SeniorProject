using Assets.Scripts.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Scripts.Model;
using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Units;

namespace Assets.Scripts.View
{
    public class CombatForecast : MonoBehaviour {
        public TextMeshProUGUI playerNameLabel;
        public TextMeshProUGUI playerAttackMethodLabel;
        public TextMeshProUGUI playerCurrentHpLabel;
        public TextMeshProUGUI playerPotentialHpLabel;
        public TextMeshProUGUI playerMaxHpLabel;
        public TextMeshProUGUI playerHitLabel;
        public TextMeshProUGUI playerOffensiveLabel;
        public TextMeshProUGUI playerFollowupPossibleLabel;
        public TextMeshProUGUI playerDefensiveLabel;
        public TextMeshProUGUI playerCritLabel;

        public TextMeshProUGUI enemyNameLabel;
        public TextMeshProUGUI enemyAttackMethodLabel;
        public TextMeshProUGUI enemyCurrentHpLabel;
        public TextMeshProUGUI enemyPotentialHpLabel;
        public TextMeshProUGUI enemyMaxHpLabel;
        public TextMeshProUGUI enemyHitLabel;
        public TextMeshProUGUI enemyOffensiveLabel;
        public TextMeshProUGUI enemyFollowupPossibleLabel;
        public TextMeshProUGUI enemyDefensiveLabel;
        public TextMeshProUGUI enemyCritLabel;

        public GameView gameView;
        private GameViewModel gameViewModel;
  
        public void ConstructCombatForecast() {
            this.gameObject.SetActive(false);
            gameViewModel = gameView.gameViewModel;
        }

        public void ResetLabels() {
            var playerUnit = gameViewModel.SelectedUnit;
            playerNameLabel.text = playerUnit.Name;
            playerAttackMethodLabel.text = "";
            playerCurrentHpLabel.text = $"CUR {playerUnit.HealthPoints}";
            playerMaxHpLabel.text = $"MAX {playerUnit.MaxHealthPoints}";
            playerHitLabel.text = $"HIT ";
            playerOffensiveLabel.text = $"OFF ";
            playerDefensiveLabel.text = $"DEF ";
            playerCritLabel.text = $"CRT ";

            playerPotentialHpLabel.gameObject.SetActive(false);
            playerFollowupPossibleLabel.gameObject.SetActive(false);

            enemyNameLabel.text = "";
            enemyAttackMethodLabel.text = "";
            enemyCurrentHpLabel.text = "";
            enemyMaxHpLabel.text = "";
            enemyHitLabel.text = "";
            enemyOffensiveLabel.text = "";
            enemyDefensiveLabel.text = "";
            enemyCritLabel.text = "";

            enemyPotentialHpLabel.gameObject.SetActive(false);
            enemyFollowupPossibleLabel.gameObject.SetActive(false);
        }

        public void UpdateForecast(Vector2Int attackingUnitNewPosition, Unit playerUnit, Unit enemyUnit) {
            if (playerUnit.PlayerNumber != enemyUnit.PlayerNumber) {

                var defenderPosition = gameViewModel.GetPositionOfUnit(enemyUnit);
                var defenderAttackPositions = gameViewModel.GetSurroundingLocationsAtPoint(defenderPosition, enemyUnit.MainWeapon.Range);

                playerNameLabel.text = playerUnit.Name;
                playerAttackMethodLabel.text = playerUnit.MainWeapon.Name;
                playerCurrentHpLabel.text = $"CUR {playerUnit.HealthPoints}";
                playerMaxHpLabel.text = $"MAX {playerUnit.MaxHealthPoints}";

                playerHitLabel.text = $"HIT {DamageCalculator.GetHitChance(playerUnit, enemyUnit)}";
                playerOffensiveLabel.text = $"OFF {DamageCalculator.GetOffensive(playerUnit)}";

                enemyDefensiveLabel.text = $"DEF {DamageCalculator.GetDefensive(playerUnit, enemyUnit)}";

                enemyPotentialHpLabel.gameObject.SetActive(true);
                if (playerUnit.Speed.Value > enemyUnit.Speed.Value) {
                    enemyPotentialHpLabel.text = $"{enemyUnit.HealthPoints - (2 * DamageCalculator.GetDamage(playerUnit, enemyUnit))}";
                    playerFollowupPossibleLabel.gameObject.SetActive(true);
                } else {
                    enemyPotentialHpLabel.text = $"{enemyUnit.HealthPoints - DamageCalculator.GetDamage(playerUnit, enemyUnit)}";
                    playerFollowupPossibleLabel.gameObject.SetActive(false);
                }
                playerDefensiveLabel.text = $"DEF {DamageCalculator.GetDefensive(enemyUnit, playerUnit)}";
                playerCritLabel.text = $"CRT {DamageCalculator.GetCritRate(playerUnit, enemyUnit)}";

                enemyNameLabel.text = enemyUnit.Name;
                enemyCurrentHpLabel.text = $"CUR {enemyUnit.HealthPoints}";
                enemyMaxHpLabel.text = $"MAX {enemyUnit.MaxHealthPoints}";

                playerPotentialHpLabel.gameObject.SetActive(true);
                if (defenderAttackPositions.Contains(attackingUnitNewPosition)) {
                    if (enemyUnit.Speed.Value > playerUnit.Speed.Value) {
                        playerPotentialHpLabel.text = $"{playerUnit.HealthPoints - (2 *DamageCalculator.GetDamage(enemyUnit, playerUnit))}";
                        enemyFollowupPossibleLabel.gameObject.SetActive(true);
                    } else {
                        playerPotentialHpLabel.text = $"{playerUnit.HealthPoints - DamageCalculator.GetDamage(enemyUnit, playerUnit)}";
                        enemyFollowupPossibleLabel.gameObject.SetActive(false);
                    }
                    enemyAttackMethodLabel.text = playerUnit.MainWeapon.Name;

                    enemyHitLabel.text = $"HIT {DamageCalculator.GetHitChance(enemyUnit, playerUnit)}";
                    enemyOffensiveLabel.text = $"OFF {DamageCalculator.GetOffensive(enemyUnit)}";

                    enemyDefensiveLabel.text = $"DEF {DamageCalculator.GetDefensive(playerUnit, enemyUnit)}";
                    enemyCritLabel.text = $"CRT {DamageCalculator.GetCritRate(enemyUnit, playerUnit)}";

                } else {
                    playerPotentialHpLabel.text = $"{playerUnit.HealthPoints}";
                    enemyAttackMethodLabel.text = "";
                    enemyHitLabel.text = $"HIT 0";
                    enemyOffensiveLabel.text = $"OFF 0";
                    enemyCritLabel.text = $"CRT 0";
                }
            }
        }
        
        public void UpdateForecastWithSkill(Vector2Int attackingUnitNewPosition, Unit playerUnit, Unit targetUnit, SingleTargetSkill skill) {
            if (playerUnit.PlayerNumber != targetUnit.PlayerNumber) {

                var defenderPosition = gameViewModel.GetPositionOfUnit(targetUnit);
                var defenderAttackPositions = gameViewModel.GetSurroundingLocationsAtPoint(defenderPosition, targetUnit.MainWeapon.Range);
                var damageSkill = skill as SingleDamageSkill;

                playerNameLabel.text = playerUnit.Name;
                playerAttackMethodLabel.text = $"{damageSkill.SkillName}";
                playerCurrentHpLabel.text = $"CUR {playerUnit.HealthPoints}";
                playerMaxHpLabel.text = $"MAX {playerUnit.MaxHealthPoints}";

                playerHitLabel.text = $"HIT {damageSkill.GetHitChance(playerUnit, targetUnit)}";
                playerOffensiveLabel.text = $"OFF {damageSkill.GetOffensive(playerUnit)}";
                playerDefensiveLabel.text = $"DEF {DamageCalculator.GetDefensive(targetUnit, playerUnit)}";
                playerCritLabel.text = $"CRT {damageSkill.GetCritRate(playerUnit, targetUnit)}";

                playerPotentialHpLabel.gameObject.SetActive(true);
                if (defenderAttackPositions.Contains(attackingUnitNewPosition)) {
                    if (targetUnit.Speed.Value > playerUnit.Speed.Value) {
                        playerPotentialHpLabel.text = $"{playerUnit.HealthPoints - skill.SkillCost - (2 *DamageCalculator.GetDamage(targetUnit, playerUnit))}";
                        enemyFollowupPossibleLabel.gameObject.SetActive(true);
                    } else {
                        playerPotentialHpLabel.text = $"{playerUnit.HealthPoints - skill.SkillCost - DamageCalculator.GetDamage(targetUnit, playerUnit)}";
                        enemyFollowupPossibleLabel.gameObject.SetActive(false);
                    }
                    enemyAttackMethodLabel.text = playerUnit.MainWeapon.Name;

                    enemyHitLabel.text = $"HIT {DamageCalculator.GetHitChance(targetUnit, playerUnit)}";
                    enemyOffensiveLabel.text = $"OFF {DamageCalculator.GetOffensive(targetUnit)}";
                    enemyCritLabel.text = $"CRT {DamageCalculator.GetCritRate(targetUnit, playerUnit)}";

                } else {
                    playerPotentialHpLabel.text = $"{playerUnit.HealthPoints - skill.SkillCost}";
                    enemyAttackMethodLabel.text = "";
                    enemyHitLabel.text = $"HIT 0";
                    enemyOffensiveLabel.text = $"OFF 0";
                    enemyCritLabel.text = $"CRT 0";
                }

                enemyNameLabel.text = targetUnit.Name;
                enemyAttackMethodLabel.text = targetUnit.MainWeapon.Name;
                enemyCurrentHpLabel.text = $"CUR {targetUnit.HealthPoints}";

                enemyPotentialHpLabel.gameObject.SetActive(true);
                enemyPotentialHpLabel.text = $"{targetUnit.HealthPoints - damageSkill.GetDamage(playerUnit, targetUnit)}";

                enemyMaxHpLabel.text = $"MAX {targetUnit.MaxHealthPoints}";
                enemyDefensiveLabel.text = $"DEF {damageSkill.GetDefensive(playerUnit, targetUnit)}";
            }
        }

        public void EnableCombatForecast() {
            gameObject.SetActive(true);
        }

        public void DisableCombatForecast() {
            gameObject.SetActive(false);
        }
    }
}