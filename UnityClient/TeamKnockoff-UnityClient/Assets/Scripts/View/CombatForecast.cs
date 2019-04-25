using Assets.Scripts.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Scripts.Model;
using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Units;

namespace Assets.Scripts.View
{
    public class CombatForecast : MonoBehaviour {
        private static Color ALLY_COLOR = Color.green;
        private static Color ENEMY_COLOR = Color.red;
        
        public Image targetUnitInformationBackground;

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

            targetUnitInformationBackground.color = ENEMY_COLOR;

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

                targetUnitInformationBackground.color = ENEMY_COLOR;

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

                int enemyHealthLost = 0;
                if (playerUnit.Speed.Value > enemyUnit.Speed.Value) {
                    enemyHealthLost = 2 * DamageCalculator.GetDamage(playerUnit, enemyUnit);
                    playerFollowupPossibleLabel.gameObject.SetActive(true);
                } else {
                    enemyHealthLost = DamageCalculator.GetDamage(playerUnit, enemyUnit);
                    playerFollowupPossibleLabel.gameObject.SetActive(false);
                }

                if (enemyUnit.HealthPoints - enemyHealthLost <= 0) {
                    enemyPotentialHpLabel.text = $"0";
                } else {
                    enemyPotentialHpLabel.text = $"{enemyUnit.HealthPoints - enemyHealthLost}";
                }

                playerDefensiveLabel.text = $"DEF {DamageCalculator.GetDefensive(enemyUnit, playerUnit)}";
                playerCritLabel.text = $"CRT {DamageCalculator.GetCritRate(playerUnit, enemyUnit)}";

                enemyNameLabel.text = enemyUnit.Name;
                enemyCurrentHpLabel.text = $"CUR {enemyUnit.HealthPoints}";
                enemyMaxHpLabel.text = $"MAX {enemyUnit.MaxHealthPoints}";

                playerPotentialHpLabel.gameObject.SetActive(true);
                if (defenderAttackPositions.Contains(attackingUnitNewPosition)) {
                    var playerHealthLost = 0;
                    if (enemyUnit.Speed.Value > playerUnit.Speed.Value) {
                        playerHealthLost = 2 * DamageCalculator.GetDamage(enemyUnit, playerUnit);
                        enemyFollowupPossibleLabel.gameObject.SetActive(true);
                    } else {
                        playerHealthLost = DamageCalculator.GetDamage(enemyUnit, playerUnit);
                        enemyFollowupPossibleLabel.gameObject.SetActive(false);
                    }

                    if (playerUnit.HealthPoints - playerHealthLost <= 0) {
                        playerPotentialHpLabel.text = "0";
                    } else {
                        playerPotentialHpLabel.text = $"{playerUnit.HealthPoints - playerHealthLost}";
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

                targetUnitInformationBackground.color = ENEMY_COLOR;

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
                    var playerHealthLost = 0;
                    if (targetUnit.Speed.Value > playerUnit.Speed.Value) {
                        playerHealthLost = damageSkill.SkillCost + (2 * DamageCalculator.GetDamage(targetUnit, playerUnit));
                        enemyFollowupPossibleLabel.gameObject.SetActive(true);
                    } else {
                        playerHealthLost = damageSkill.SkillCost + DamageCalculator.GetDamage(targetUnit, playerUnit);
                        enemyFollowupPossibleLabel.gameObject.SetActive(false);
                    }

                    if (playerUnit.HealthPoints - playerHealthLost <= 0) {
                        playerPotentialHpLabel.text = "0";
                    } else {
                        playerPotentialHpLabel.text = $"{playerUnit.HealthPoints - playerHealthLost}";
                    }

                    enemyAttackMethodLabel.text = targetUnit.MainWeapon.Name;

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
                enemyCurrentHpLabel.text = $"CUR {targetUnit.HealthPoints}";

                enemyPotentialHpLabel.gameObject.SetActive(true);
                var enemyHealthLost = damageSkill.GetDamage(playerUnit, targetUnit);
                if (targetUnit.HealthPoints - enemyHealthLost <= 0) {
                    enemyPotentialHpLabel.text = $"0";
                } else {
                    enemyPotentialHpLabel.text = $"{targetUnit.HealthPoints - damageSkill.GetDamage(playerUnit, targetUnit)}";
                }

                enemyMaxHpLabel.text = $"MAX {targetUnit.MaxHealthPoints}";
                enemyDefensiveLabel.text = $"DEF {damageSkill.GetDefensive(playerUnit, targetUnit)}";
            } else {
                targetUnitInformationBackground.color = ALLY_COLOR;

                var defenderPosition = gameViewModel.GetPositionOfUnit(targetUnit);
                var defenderAttackPositions = gameViewModel.GetSurroundingLocationsAtPoint(defenderPosition, targetUnit.MainWeapon.Range);
                var supportSkill = skill as SingleSupportSkill;

                playerNameLabel.text = playerUnit.Name;
                playerAttackMethodLabel.text = $"{supportSkill.SkillName}";
                playerCurrentHpLabel.text = $"CUR {playerUnit.HealthPoints}";
                playerMaxHpLabel.text = $"MAX {playerUnit.MaxHealthPoints}";

                playerHitLabel.text = $"HIT ";
                playerOffensiveLabel.text = $"OFF ";
                playerDefensiveLabel.text = $"DEF ";
                playerCritLabel.text = $"CRT ";

                playerPotentialHpLabel.gameObject.SetActive(true);
                playerPotentialHpLabel.text = $"{playerUnit.HealthPoints - skill.SkillCost}";

                enemyNameLabel.text = targetUnit.Name;
                enemyAttackMethodLabel.text = "";
                enemyCurrentHpLabel.text = $"CUR {targetUnit.HealthPoints}";

                enemyPotentialHpLabel.gameObject.SetActive(true);

                var targetUnitHealthGain = supportSkill.GetHealAmount(playerUnit, targetUnit);
                if (targetUnitHealthGain == 0) {
                    enemyPotentialHpLabel.gameObject.SetActive(false);
                    enemyPotentialHpLabel.text = $"0";
                } else {
                    enemyPotentialHpLabel.gameObject.SetActive(true);
                    if (targetUnit.HealthPoints + targetUnitHealthGain > targetUnit.MaxHealthPoints.Value) {
                        enemyPotentialHpLabel.text = $"{targetUnit.MaxHealthPoints.Value}";
                    } else {
                        enemyPotentialHpLabel.text = $"{targetUnit.HealthPoints + targetUnitHealthGain}";
                    }
                }

                enemyMaxHpLabel.text = $"MAX {targetUnit.MaxHealthPoints}";
                enemyDefensiveLabel.text = $"DEF ";
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