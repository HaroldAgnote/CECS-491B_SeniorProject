﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Assets.Scripts.Application;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;
using Assets.Scripts.ViewModel;

using AttackResult = Assets.Scripts.Model.AttackMoveResult.AttackResult;
using DamageSkillResult = Assets.Scripts.Model.DamageSkillMoveResult.DamageSkillResult;
using SupportSkillResult = Assets.Scripts.Model.SupportSkillMoveResult.SupportSkillResult;

namespace Assets.Scripts.View {
    public class GameView : MonoBehaviour {
        public const int LABEL_DELAY = 500;
        public const float UNIT_LABEL_Y_OFFSET = 0.75f;
        public const float UNIT_LABEL_Z_OFFSET = 5.00f;

        public GameViewModel gameViewModel;

        public CameraController mCamera;

        public TileSelector tileSelector;
        public MoveSelector moveSelector;

        public TextMeshProUGUI turnLabel;
        public TextMeshProUGUI attackerLabel;
        public TextMeshProUGUI defenderLabel;

        public UnitInformation unitInformation;

        public CombatForecast combatForecast;

        public GameOverScreen gameOverScreen;

        public LevelUpScreen levelUpScreen;

        public PauseMenu mPauseMenu;

        public Button mPauseButton;

        public Button mEndTurnButton;

        public GameObject tiles;

        public GameObject units;

        private Dictionary<Vector2Int, ObjectView> mVectorToObjectViews;

        private bool mHasScreenOverlay;

        public bool HasScreenOverlay {
            get {
                return mHasScreenOverlay;
            }

            set {
                if (mHasScreenOverlay != value) {
                    mHasScreenOverlay = value;
                }
            }
        }

        private bool mIsUpdating;

        public bool IsUpdating {
            get {
                return mIsUpdating;
            }
        }

        public void ConstructView(int columns, int rows, Dictionary<Vector2Int, ObjectView> vectorsToObjectViews) {
            mCamera.minMaxXPosition.Set(0, columns);
            mCamera.minMaxYPosition.Set(0, rows);

            mVectorToObjectViews = vectorsToObjectViews;

            tileSelector.ConstructTileSelector();
            moveSelector.ConstructMoveSelector();

            unitInformation.ConstructUnitInformation();

            combatForecast.ConstructCombatForecast();

            gameOverScreen.ConstructGameOverScreen();

            levelUpScreen.ConstructLevelUpScreen();

            mPauseMenu.ConstructPauseMenu();

            turnLabel.text = $"Turn {gameViewModel.CurrentTurn}";

            attackerLabel.gameObject.SetActive(false);
            defenderLabel.gameObject.SetActive(false);

            mPauseButton.onClick.AddListener(PauseGame);
            mEndTurnButton.onClick.AddListener(EndTurn);

            gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;
        }

        public void EndTurn() {
            var units = gameViewModel.ControllingPlayer.Units.Where(unit => unit.IsAlive && !unit.HasMoved);
            foreach(var unit in units) {
                var unitPos = gameViewModel.GetPositionOfUnit(unit);
                var gameMove = new GameMove(unitPos, unitPos, GameMove.GameMoveType.Wait);
                gameViewModel.ApplyMove(gameMove);
            }
        }

        public void PauseGame() {
            mPauseButton.interactable = false;
            mPauseMenu.gameObject.SetActive(true);
            tileSelector.gameObject.SetActive(false);
            moveSelector.gameObject.SetActive(false);
            LockCamera();
            gameViewModel.PauseGame();
        }

        public void UnpauseGame() {
            mPauseButton.interactable = true;
            mPauseMenu.gameObject.SetActive(false);
            tileSelector.gameObject.SetActive(true);
            moveSelector.gameObject.SetActive(true);
            UnlockCamera();
            gameViewModel.UnpauseGame();
        }

        public void FullLockCamera() {
            mCamera.LockMoveCamera();
            mCamera.LockZoomCamera();
        }

        public void FullUnlockCamera() {
            mCamera.UnlockMoveCamera();
            mCamera.UnlockZoomCamera();
        }

        public void LockCamera() {
            mCamera.LockMoveCamera();
        }

        public void UnlockCamera() {
            mCamera.UnlockMoveCamera();
        }

        private async void GameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "CurrentPlayer") {
                if (gameViewModel.CurrentPlayer != gameViewModel.ControllingPlayer) {
                    mPauseButton.interactable = false;
                    mEndTurnButton.interactable = false;
                    tileSelector.gameObject.SetActive(false);
                    moveSelector.gameObject.SetActive(false);
                } else {
                    mPauseButton.interactable = true;
                    mEndTurnButton.interactable = true;
                    tileSelector.gameObject.SetActive(true);
                    moveSelector.gameObject.SetActive(true);
                }
                turnLabel.text = $"{gameViewModel.CurrentPlayer.Name} - Turn {gameViewModel.CurrentTurn}";
            }

            if (e.PropertyName == "CurrentTurn") {
                turnLabel.text = $"{gameViewModel.CurrentPlayer.Name} - Turn {gameViewModel.CurrentTurn}";
            }

            if (e.PropertyName == "Squares") {
                tileSelector.RefreshAllyHighlighters();
            }

            if (e.PropertyName == "MoveResult") {
                var moveResult = gameViewModel.MoveResult;

                if (moveResult is PositionMoveResult) {
                    var positionMoveResult = moveResult as PositionMoveResult;
                    if (positionMoveResult.StartPosition != positionMoveResult.EndPosition) {
                        var startPosition = positionMoveResult.StartPosition;
                        var endPosition = positionMoveResult.EndPosition;
                        var path = positionMoveResult.Path;

                        var objectView = mVectorToObjectViews[startPosition];

                        tileSelector.gameObject.SetActive(false);
                        (objectView as UnitView).UpdatePosition(path);
                        var unitMover = objectView.GameObject.GetComponent<UnitMover>();

                        mVectorToObjectViews.Remove(startPosition);
                        mVectorToObjectViews.Add(endPosition, objectView);

                        await Task.Run(() => {
                            mIsUpdating = true;
                            while (unitMover.IsMoving) { }
                            mIsUpdating = false;
                            Thread.Sleep(LABEL_DELAY);
                        });

                        if (!gameViewModel.IsPaused && gameViewModel.CurrentPlayer == gameViewModel.ControllingPlayer) {
                            tileSelector.gameObject.SetActive(true);
                        }
                    }
                } else if (moveResult is AttackMoveResult) {
                    var attackMoveResult = moveResult as AttackMoveResult;
                    var attackerResult = attackMoveResult.AttackerResult;
                    var defenderResult = attackMoveResult.DefenderResult;

                    var attackerPosition = attackerResult.AttackerPosition;
                    var defenderPosition = attackerResult.DefenderPosition;

                    var attackerUnitView = mVectorToObjectViews[attackerPosition] as UnitView;
                    var defenderObjectView = mVectorToObjectViews[defenderPosition];

                    var attackerUnitMover = attackerUnitView.GameObject.GetComponent<UnitMover>();

                    // TODO: Animate Attack Animation
                    attackerUnitView.NudgeTowardsPosition(attackerPosition, defenderPosition);

                    await Task.Run(() => {
                        mIsUpdating = true;
                        while (attackerUnitMover.IsMoving) { }
                    });

                    UpdateAttackLabels(attackerResult, attackerPosition, defenderPosition);

                    CheckObjectStatus(defenderPosition);

                    if (defenderResult != null) {
                        if (defenderObjectView is UnitView) {
                            var defenderUnitView = defenderObjectView as UnitView;
                            var defenderUnitMover = defenderUnitView.GameObject.GetComponent<UnitMover>();

                            defenderUnitView.NudgeTowardsPosition(defenderPosition, attackerPosition);

                            // TODO: Animate Attack Animation
                            await Task.Run(() => {
                                mIsUpdating = true;
                                while (attackerUnitMover.IsMoving) { }
                            });

                            UpdateAttackLabels(defenderResult, defenderPosition, attackerPosition);

                            CheckObjectStatus(attackerPosition);
                        }
                    }
                    mIsUpdating = false;

                } else if (moveResult is SkillMoveResult) {
                    var skillMoveResult = moveResult as SkillMoveResult;
                    if (skillMoveResult is DamageSkillMoveResult) {
                        var damageSkillMoveResult = skillMoveResult as DamageSkillMoveResult;
                        var attackerResult = damageSkillMoveResult.AttackerResult;
                        var defenderResult = damageSkillMoveResult.DefenderResult;

                        var attackerPosition = attackerResult.AttackerPosition;
                        var defenderPosition = attackerResult.DefenderPosition;

                        var attackerUnitView = mVectorToObjectViews[attackerPosition] as UnitView;
                        var defenderObjectView = mVectorToObjectViews[defenderPosition];

                        var attackerUnitMover = attackerUnitView.GameObject.GetComponent<UnitMover>();

                        // TODO: Animate Attack Animation
                        attackerUnitView.NudgeTowardsPosition(attackerPosition, defenderPosition);

                        await Task.Run(() => {
                            mIsUpdating = true;
                            while (attackerUnitMover.IsMoving) { }
                        });

                        UpdateDamageSkillLabels(attackerResult, attackerPosition, defenderPosition);

                        CheckObjectStatus(defenderPosition);

                        if (defenderResult != null) {
                            if (defenderObjectView is UnitView) {
                                var defenderUnitView = defenderObjectView as UnitView;
                                var defenderUnitMover = defenderUnitView.GameObject.GetComponent<UnitMover>();

                                defenderUnitView.NudgeTowardsPosition(defenderPosition, attackerPosition);

                                // TODO: Animate Attack Animation
                                await Task.Run(() => {
                                    mIsUpdating = true;
                                    while (attackerUnitMover.IsMoving) { }
                                });

                                UpdateAttackLabels(defenderResult, defenderPosition, attackerPosition);

                                CheckObjectStatus(attackerPosition);
                            }
                        }
                    } else if (skillMoveResult is SupportSkillMoveResult) {
                        var supportSkillMoveResult = skillMoveResult as SupportSkillMoveResult;
                        var supporterResult = supportSkillMoveResult.SupporterResult;

                        var supporterPosition = supporterResult.SupporterPosition;
                        var supportedPosition = supporterResult.SupportedPosition;

                        var supporterUnitView = mVectorToObjectViews[supporterPosition] as UnitView;

                        var supporterUnitMover = supporterUnitView.GameObject.GetComponent<UnitMover>();

                        // TODO: Animate Attack Animation
                        supporterUnitView.NudgeTowardsPosition(supporterPosition, supportedPosition);

                        await Task.Run(() => {
                            mIsUpdating = true;
                            while (supporterUnitMover.IsMoving) { }
                        });

                        UpdateSupportSkillLabels(supporterResult, supporterPosition, supportedPosition);

                        CheckObjectStatus(supportedPosition);
                    } else {
                        throw new Exception("Error: Bad Skill Result");
                    }

                    mIsUpdating = false;
                } else if (moveResult is ItemMoveResult) {

                } else if (moveResult is WaitMoveResult) {

                } else {
                    throw new Exception("Bad Move Result");
                }
            }
        }

        private void CheckObjectStatus(Vector2Int objectPosition) {
            var objectView= mVectorToObjectViews[objectPosition];

            if (objectView is UnitView) {
                var unitView= objectView as UnitView;

                var unit = gameViewModel.Squares
                            .SingleOrDefault(sq => sq.Position == objectPosition)
                            .Unit;

                if (unit == null || !unit.IsAlive) {
                    unitView.GameObject.SetActive(false);
                    mVectorToObjectViews.Remove(objectPosition);
                }
            }
        }

        private async void UpdateAttackLabels(AttackResult attackerResult, Vector2Int attackerPosition, Vector2Int defenderPosition) {
            var attackerLabelPos = new Vector3(attackerPosition.x, attackerPosition.y + UNIT_LABEL_Y_OFFSET, UNIT_LABEL_Z_OFFSET);
            attackerLabel.transform.position = attackerLabelPos;

            var defenderLabelPos = new Vector3(defenderPosition.x, defenderPosition.y + UNIT_LABEL_Y_OFFSET, UNIT_LABEL_Z_OFFSET);
            defenderLabel.transform.position = defenderLabelPos;

            if (attackerResult.Result != AttackResult.AttackStatus.Miss) {
                if (attackerResult.Result == AttackResult.AttackStatus.Regular) {
                    attackerLabel.text = "";
                    defenderLabel.text = $"- {attackerResult.DamageDealt}";
                } else if (attackerResult.Result == AttackResult.AttackStatus.Critical) {
                    attackerLabel.text = "CRITICAL!";
                    defenderLabel.text = $"- {attackerResult.DamageDealt}";
                } else if (attackerResult.Result == AttackResult.AttackStatus.Lethal) {
                    attackerLabel.text = "LETHAL HIT!";
                    defenderLabel.text = $"- {attackerResult.DamageDealt}";
                }
            } else {
                attackerLabel.text = "";
                defenderLabel.text = "Missed!";
            }
            attackerLabel.gameObject.SetActive(true);
            defenderLabel.gameObject.SetActive(true);

            await Task.Run(() => {
                Thread.Sleep(LABEL_DELAY);
            });

            attackerLabel.gameObject.SetActive(false);
            defenderLabel.gameObject.SetActive(false);
        }

        private async void UpdateDamageSkillLabels(DamageSkillResult attackerResult, Vector2Int attackerPosition, Vector2Int defenderPosition) {
            var attackerLabelPos = new Vector3(attackerPosition.x, attackerPosition.y + UNIT_LABEL_Y_OFFSET, UNIT_LABEL_Z_OFFSET);
            attackerLabel.transform.position = attackerLabelPos;

            var defenderLabelPos = new Vector3(defenderPosition.x, defenderPosition.y + UNIT_LABEL_Y_OFFSET, UNIT_LABEL_Z_OFFSET);
            defenderLabel.transform.position = defenderLabelPos;

            if (attackerResult.Result != DamageSkillResult.DamageSkillStatus.Miss) {
                if (attackerResult.Result == DamageSkillResult.DamageSkillStatus.Regular) {
                    attackerLabel.text = $"{attackerResult.SkillUsed.SkillName}!";
                    defenderLabel.text = $"- {attackerResult.DamageDealt}";
                } else if (attackerResult.Result == DamageSkillResult.DamageSkillStatus.Critical) {
                    attackerLabel.text = $"{attackerResult.SkillUsed.SkillName} - CRITICAL!";
                    defenderLabel.text = $"- {attackerResult.DamageDealt}";
                } else if (attackerResult.Result == DamageSkillResult.DamageSkillStatus.Lethal) {
                    attackerLabel.text = $"{attackerResult.SkillUsed.SkillName} - LETHAL HIT!";
                    defenderLabel.text = $"- {attackerResult.DamageDealt}";
                }
            } else {
                attackerLabel.text = "";
                defenderLabel.text = "Missed!";
            }
            attackerLabel.gameObject.SetActive(true);
            defenderLabel.gameObject.SetActive(true);

            await Task.Run(() => {
                Thread.Sleep(LABEL_DELAY);
            });

            attackerLabel.gameObject.SetActive(false);
            defenderLabel.gameObject.SetActive(false);
        }
        
        private async void UpdateSupportSkillLabels(SupportSkillResult supporterResult, Vector2Int supporterPosition, Vector2Int supportedPosition) {
            var supporterLabelPos = new Vector3(supporterPosition.x, supporterPosition.y + UNIT_LABEL_Y_OFFSET, UNIT_LABEL_Z_OFFSET);
            attackerLabel.transform.position = supporterLabelPos;

            var supportedLabelPos = new Vector3(supportedPosition.x, supportedPosition.y + UNIT_LABEL_Y_OFFSET, UNIT_LABEL_Z_OFFSET);
            defenderLabel.transform.position = supportedLabelPos;

            if (supporterResult.Result == SupportSkillMoveResult.SupportSkillResult.SupportSkillStatus.Heal) {
                attackerLabel.text = $"{supporterResult.SkillUsed.SkillName}!";
                defenderLabel.text = $"+ {supporterResult.DamageHealed}";
            } else if (supporterResult.Result == SupportSkillMoveResult.SupportSkillResult.SupportSkillStatus.Buff) {
                attackerLabel.text = $"{supporterResult.SkillUsed.SkillName}!";
                defenderLabel.text = "";
            } else {
                throw new Exception("Bad Support Skill");
            }

            attackerLabel.gameObject.SetActive(true);
            defenderLabel.gameObject.SetActive(true);

            await Task.Run(() => {
                Thread.Sleep(LABEL_DELAY);
            });

            attackerLabel.gameObject.SetActive(false);
            defenderLabel.gameObject.SetActive(false);
        }
    }
}
