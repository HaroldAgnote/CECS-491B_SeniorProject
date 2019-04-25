using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Assets.Scripts.Application;
using Assets.Scripts.Utilities.ExtensionMethods;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;
using Assets.Scripts.ViewModel;

using AttackResult = Assets.Scripts.Model.AttackMoveResult.AttackResult;
using DamageSkillResult = Assets.Scripts.Model.DamageSkillMoveResult.DamageSkillResult;
using SupportSkillResult = Assets.Scripts.Model.SupportSkillMoveResult.SupportSkillResult;

namespace Assets.Scripts.View {
    public class GameView : MonoBehaviour {
        public const int LABEL_DELAY = 1000;
        public const float UNIT_LABEL_Y_OFFSET = 0.75f;
        public const float UNIT_LABEL_Z_OFFSET = 5.00f;

        public static Color FADE_COLOR = Color.gray;
        public static Color UNFADE_COLOR = Color.white;

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

        public Toggle mDangerZoneToggle;

        public Button mPauseButton;

        public Button mEndTurnButton;

        public GameObject tiles;

        public GameObject units;

        public GameObject topPanel;

        public GameObject bottomPanel;

        public GameObject availableUnitsContent;

        public GameObject unitButtonPrefab;

        private Dictionary<Vector2Int, ObjectView> mVectorToObjectViews;

        private Dictionary<Unit, Button> mUnitToButtons;

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

        private bool mHasMoveText;

        public bool HasMoveText => mHasMoveText;

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

            var units = gameViewModel.ControllingPlayer.Units;
            mUnitToButtons = new Dictionary<Unit, Button>();

            foreach (var unit in units) {
                var button = CreateUnitButton(unit);
                mUnitToButtons.Add(unit, button);
                button.onClick.AddListener(() => {
                    // TODO: Camera move to Unit Position
                    var position = gameViewModel.GetPositionOfUnit(unit);

                    mCamera.MoveToPosition(position.ToVector3());

                    unitInformation.UpdateUnitInformation(unit);
                });
            }

            foreach (var unitViewModel in gameViewModel.UnitViewModels) {
                if (unitViewModel.Unit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber) {
                    unitViewModel.PropertyChanged += UnitViewModel_PropertyChanged;
                }
            }

            mDangerZoneToggle.onValueChanged.AddListener(delegate {
                DangerZoneToggleValueChanged(mDangerZoneToggle.isOn);
            });

            gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;
        }

        private void DangerZoneToggleValueChanged(bool dangerZoneToggled) {
            if (dangerZoneToggled) {
                tileSelector.RefreshDangerZone();
            } else {
                tileSelector.DestroyDangerZones();
            }
        }

        public void StartGame() {
            tileSelector.RefreshAllyHighlighters();
        }

        private void UnitViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) {
            var unitViewModel = sender as UnitViewModel;
            if (e.PropertyName == "IsAlive") {
                var unit = unitViewModel.Unit;
                if (unit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber) {
                    if (!unit.IsAlive) {
                        mUnitToButtons[unit].interactable = false;
                        var unitSprite = mUnitToButtons[unit].GetComponentInChildren<Image>();
                        unitSprite.color = FADE_COLOR;
                    }
                }
            }
        }

        private Button CreateUnitButton(Unit unit) {
            var buttonObject = Instantiate(unitButtonPrefab, availableUnitsContent.transform);
            var buttonLabel = buttonObject.GetComponentInChildren<TextMeshProUGUI>();
            buttonLabel.text = unit.Name.ToUpper();


            var buttonComponent = buttonObject.GetComponent<Button>();

            var buttonSprites = buttonComponent.gameObject.GetComponentsInChildren<Image>();

            var unitSprite = UnitFactory.instance.GetUnitSprite(unit.Name);
            buttonSprites[1].sprite = unitSprite;

            return buttonComponent;
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
                    await Task.Run(() => {
                        while (mHasMoveText || mIsUpdating) { }
                    });

                    bottomPanel.gameObject.SetActive(false);

                    mCamera.LockMoveCamera();
                    mCamera.LockZoomCamera();

                    mPauseButton.interactable = false;
                    mEndTurnButton.interactable = false;
                    tileSelector.gameObject.SetActive(false);
                    moveSelector.gameObject.SetActive(false);
                    UnfadeUnits();
                } else {
                    await Task.Run(() => {
                        while (mHasMoveText || mIsUpdating) { }
                    });

                    bottomPanel.gameObject.SetActive(true);

                    mCamera.UnlockMoveCamera();
                    mCamera.UnlockZoomCamera();

                    mPauseButton.interactable = true;
                    mEndTurnButton.interactable = true;
                    tileSelector.gameObject.SetActive(true);
                    moveSelector.gameObject.SetActive(true);
                }
                turnLabel.text = $"{gameViewModel.CurrentPlayer.Name} - Turn {gameViewModel.CurrentTurn}";
            }

            if (e.PropertyName == "CurrentTurn") {
                await Task.Run(() => {
                    while (mHasMoveText || mIsUpdating) { }
                });
                turnLabel.text = $"{gameViewModel.CurrentPlayer.Name} - Turn {gameViewModel.CurrentTurn}";
            }

            if (e.PropertyName == "Squares") {
                tileSelector.RefreshAllyHighlighters();
                if (mDangerZoneToggle.isOn) {
                    tileSelector.RefreshDangerZone();
                }
                tileSelector.RefreshToggledEnemySquares();
            }

            if (e.PropertyName == "MoveResult") {
                var moveResult = gameViewModel.MoveResult;
                bottomPanel.gameObject.SetActive(false);

                if (moveResult is PositionMoveResult) {
                    var positionMoveResult = moveResult as PositionMoveResult;
                    if (positionMoveResult.StartPosition != positionMoveResult.EndPosition) {
                        var startPosition = positionMoveResult.StartPosition;
                        var endPosition = positionMoveResult.EndPosition;
                        var path = positionMoveResult.Path;

                        var objectView = mVectorToObjectViews[startPosition];

                        mCamera.FollowGameObject(objectView.GameObject.transform);

                        tileSelector.gameObject.SetActive(false);
                        (objectView as UnitView).UpdatePosition(path);
                        var unitMover = objectView.GameObject.GetComponent<UnitMover>();

                        mVectorToObjectViews.Remove(startPosition);
                        mVectorToObjectViews.Add(endPosition, objectView);

                        await Task.Run(() => {
                            mIsUpdating = true;
                            while (unitMover.IsMoving) { }
                            mIsUpdating = false;
                        });

                        mCamera.StopFollowingGameObject();

                    }
                } else if (moveResult is AttackMoveResult) {
                    var attackMoveResult = moveResult as AttackMoveResult;
                    var attackResults = attackMoveResult.AttackResults;

                    var playerAttackPosition = attackResults.First().AttackerPosition;

                    var playerUnit = gameViewModel.Squares
                                .SingleOrDefault(sq => sq.Position == playerAttackPosition)
                                .Unit;

                    foreach (var attackResult in attackResults) {
                        var attackerPosition = attackResult.AttackerPosition;
                        var defenderPosition = attackResult.DefenderPosition;

                        var attackerUnitView = mVectorToObjectViews[attackerPosition] as UnitView;
                        var defenderObjectView = mVectorToObjectViews[defenderPosition];

                        var attackerUnitMover = attackerUnitView.GameObject.GetComponent<UnitMover>();

                        var unit = gameViewModel.Squares
                                    .SingleOrDefault(sq => sq.Position == attackerPosition)
                                    .Unit;

                        mCamera.MoveToPosition(attackerUnitView.GameObject.transform.position);

                        // TODO: Animate Attack Animation
                        attackerUnitView.NudgeTowardsPosition(attackerPosition, defenderPosition);

                        await Task.Run(() => {
                            mIsUpdating = true;
                            while (attackerUnitMover.IsMoving) { }
                        });

                        UpdateAttackLabels(attackResult, attackerPosition, defenderPosition);

                        await Task.Run(() => {
                            while (mHasMoveText) { }
                        });

                    }

                    if (playerUnit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber && mVectorToObjectViews.Keys.Contains(playerAttackPosition)) {
                        FadeUnit(playerAttackPosition);
                    }

                    CheckObjectStatus(attackResults.First().AttackerPosition);
                    CheckObjectStatus(attackResults.First().DefenderPosition);

                    mIsUpdating = false;
                } else if (moveResult is SkillMoveResult) {
                    var skillMoveResult = moveResult as SkillMoveResult;
                    if (skillMoveResult is DamageSkillMoveResult) {
                        var damageSkillMoveResult = skillMoveResult as DamageSkillMoveResult;
                        var attackerResult = damageSkillMoveResult.AttackerResult;
                        var defenderResults = damageSkillMoveResult.DefenderResults;

                        var attackerPosition = attackerResult.AttackerPosition;
                        var defenderPosition = attackerResult.DefenderPosition;

                        var unit = gameViewModel.Squares
                                    .SingleOrDefault(sq => sq.Position == attackerPosition)
                                    .Unit;

                        var attackerUnitView = mVectorToObjectViews[attackerPosition] as UnitView;
                        var defenderObjectView = mVectorToObjectViews[defenderPosition];

                        var attackerUnitMover = attackerUnitView.GameObject.GetComponent<UnitMover>();

                        mCamera.MoveToPosition(attackerUnitView.GameObject.transform.position);

                        // TODO: Animate Attack Animation
                        attackerUnitView.NudgeTowardsPosition(attackerPosition, defenderPosition);

                        await Task.Run(() => {
                            mIsUpdating = true;
                            while (attackerUnitMover.IsMoving) { }
                        });

                        UpdateDamageSkillLabels(attackerResult, attackerPosition, defenderPosition);

                        await Task.Run(() => {
                            while (mHasMoveText) { }
                        });

                        foreach (var defenderResult in defenderResults) {
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

                                await Task.Run(() => {
                                    while (mHasMoveText) { }
                                });

                            }
                        }
                        if (unit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber && mVectorToObjectViews.Keys.Contains(attackerPosition)) {
                            FadeUnit(attackerPosition);
                        }

                        CheckObjectStatus(attackerPosition);
                        CheckObjectStatus(defenderPosition);

                        mIsUpdating = false;
                    } else if (skillMoveResult is SupportSkillMoveResult) {
                        var supportSkillMoveResult = skillMoveResult as SupportSkillMoveResult;
                        var supporterResult = supportSkillMoveResult.SupporterResult;

                        var supporterPosition = supporterResult.SupporterPosition;
                        var supportedPosition = supporterResult.SupportedPosition;

                        var unit = gameViewModel.Squares
                                    .SingleOrDefault(sq => sq.Position == supporterPosition)
                                    .Unit;

                        var supporterUnitView = mVectorToObjectViews[supporterPosition] as UnitView;

                        var supporterUnitMover = supporterUnitView.GameObject.GetComponent<UnitMover>();

                        mCamera.MoveToPosition(supporterUnitView.GameObject.transform.position);
                        // TODO: Animate Attack Animation
                        supporterUnitView.NudgeTowardsPosition(supporterPosition, supportedPosition);

                        await Task.Run(() => {
                            mIsUpdating = true;
                            while (supporterUnitMover.IsMoving) { }
                        });

                        UpdateSupportSkillLabels(supporterResult, supporterPosition, supportedPosition);

                        await Task.Run(() => {
                            while (mHasMoveText) { }
                        });

                        CheckObjectStatus(supportedPosition);

                        if (unit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber && mVectorToObjectViews.Keys.Contains(supporterPosition)) {
                            FadeUnit(supporterPosition);
                        }
                    } else {
                        throw new Exception("Error: Bad Skill Result");
                    }
                    mIsUpdating = false;
                } else if (moveResult is ItemMoveResult) {
                    var itemMoveResult = moveResult as ItemMoveResult;
                    if (itemMoveResult is DamageItemMoveResult) {
                        var damageItemMoveResult = itemMoveResult as DamageItemMoveResult;

                        var attackerPosition = damageItemMoveResult.AttackerPosition;
                        var defenderPosition = damageItemMoveResult.DefenderPosition;

                        var attackerUnitView = mVectorToObjectViews[attackerPosition] as UnitView;
                        var defenderObjectView = mVectorToObjectViews[defenderPosition];

                        var attackerUnitMover = attackerUnitView.GameObject.GetComponent<UnitMover>();

                        mCamera.MoveToPosition(attackerUnitView.GameObject.transform.position);
                        // TODO: Animate Attack Animation
                        attackerUnitView.NudgeTowardsPosition(attackerPosition, defenderPosition);

                        await Task.Run(() => {
                            mIsUpdating = true;
                            while (attackerUnitMover.IsMoving) { }
                        });

                        UpdateDamageItemLabels(damageItemMoveResult);

                        await Task.Run(() => {
                            while (mHasMoveText) { }
                        });

                        CheckObjectStatus(defenderPosition);

                        if (gameViewModel.IsControllingPlayersTurn && mVectorToObjectViews.Keys.Contains(attackerPosition)) {
                            FadeUnit(attackerPosition);
                        }
                    } else if (itemMoveResult is SupportItemMoveResult) {
                        var supportItemMoveResult = itemMoveResult as SupportItemMoveResult;

                        var supporterPosition = supportItemMoveResult.SupporterPosition;
                        var supportedPosition = supportItemMoveResult.SupportedPosition;

                        var unit = gameViewModel.Squares
                                    .SingleOrDefault(sq => sq.Position == supporterPosition)
                                    .Unit;

                        var supporterUnitView = mVectorToObjectViews[supporterPosition] as UnitView;

                        var supporterUnitMover = supporterUnitView.GameObject.GetComponent<UnitMover>();

                        mCamera.MoveToPosition(supporterUnitView.GameObject.transform.position);
                        // TODO: Animate Attack Animation
                        supporterUnitView.NudgeTowardsPosition(supporterPosition, supportedPosition);

                        await Task.Run(() => {
                            mIsUpdating = true;
                            while (supporterUnitMover.IsMoving) { }
                        });

                        UpdateSupportItemLabels(supportItemMoveResult);

                        await Task.Run(() => {
                            while (mHasMoveText) { }
                        });

                        CheckObjectStatus(supportedPosition);

                        if (unit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber && mVectorToObjectViews.Keys.Contains(supporterPosition)) {
                            FadeUnit(supporterPosition);
                        }
                    } else {
                        throw new Exception("Error: Bad Skill Result");
                    }
                    mIsUpdating = false;
                } else if (moveResult is WaitMoveResult) {
                    var waitMoveResult = moveResult as WaitMoveResult;
                    var unit = gameViewModel.Squares
                                .SingleOrDefault(sq => sq.Position == waitMoveResult.UnitPosition)
                                .Unit;

                    if (unit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber && mVectorToObjectViews.Keys.Contains(waitMoveResult.UnitPosition)) {
                        FadeUnit(waitMoveResult.UnitPosition);
                    }
                    mIsUpdating = false;
                } else {
                    throw new Exception("Bad Move Result");
                }

                if (!gameViewModel.IsPaused && gameViewModel.CurrentPlayer == gameViewModel.ControllingPlayer) {
                    tileSelector.gameObject.SetActive(true);
                    bottomPanel.gameObject.SetActive(true);
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
            mHasMoveText = true;
            attackerLabel.gameObject.SetActive(true);
            defenderLabel.gameObject.SetActive(true);

            await Task.Run(() => {
                Thread.Sleep(LABEL_DELAY);
            });

            attackerLabel.gameObject.SetActive(false);
            defenderLabel.gameObject.SetActive(false);
            mHasMoveText = false;
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
            mHasMoveText = true;
            attackerLabel.gameObject.SetActive(true);
            defenderLabel.gameObject.SetActive(true);

            await Task.Run(() => {
                Thread.Sleep(LABEL_DELAY);
            });

            attackerLabel.gameObject.SetActive(false);
            defenderLabel.gameObject.SetActive(false);
            mHasMoveText = false;
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

            mHasMoveText = true;
            attackerLabel.gameObject.SetActive(true);
            defenderLabel.gameObject.SetActive(true);

            await Task.Run(() => {
                Thread.Sleep(LABEL_DELAY);
            });

            attackerLabel.gameObject.SetActive(false);
            defenderLabel.gameObject.SetActive(false);
            mHasMoveText = false;
        }

        private async void UpdateDamageItemLabels(DamageItemMoveResult damageItemMoveResult) {
            var attackerPosition = damageItemMoveResult.AttackerPosition;
            var defenderPosition = damageItemMoveResult.DefenderPosition;

            var attackerLabelPos = new Vector3(attackerPosition.x, attackerPosition.y + UNIT_LABEL_Y_OFFSET, UNIT_LABEL_Z_OFFSET);
            attackerLabel.transform.position = attackerLabelPos;

            var defenderLabelPos = new Vector3(defenderPosition.x, defenderPosition.y + UNIT_LABEL_Y_OFFSET, UNIT_LABEL_Z_OFFSET);
            defenderLabel.transform.position = defenderLabelPos;

            attackerLabel.text = $"";
            defenderLabel.text = $"- {damageItemMoveResult.DamageDealt}";

            mHasMoveText = true;
            attackerLabel.gameObject.SetActive(true);
            defenderLabel.gameObject.SetActive(true);

            await Task.Run(() => {
                Thread.Sleep(LABEL_DELAY);
            });

            attackerLabel.gameObject.SetActive(false);
            defenderLabel.gameObject.SetActive(false);
            mHasMoveText = false;
        }

        private async void UpdateSupportItemLabels(SupportItemMoveResult supportItemMoveResult) {
            var supporterPosition = supportItemMoveResult.SupporterPosition;
            var supportedPosition = supportItemMoveResult.SupportedPosition;

            var attackerLabelPos = new Vector3(supporterPosition.x, supporterPosition.y + UNIT_LABEL_Y_OFFSET, UNIT_LABEL_Z_OFFSET);
            attackerLabel.transform.position = attackerLabelPos;

            var defenderLabelPos = new Vector3(supportedPosition.x, supportedPosition.y + UNIT_LABEL_Y_OFFSET, UNIT_LABEL_Z_OFFSET);
            defenderLabel.transform.position = defenderLabelPos;

            if (supportItemMoveResult.Result == SupportItemMoveResult.SupportItemStatus.Heal) {
                if (attackerLabelPos == defenderLabelPos) {
                    attackerLabelPos.y += UNIT_LABEL_Y_OFFSET;
                    attackerLabel.transform.position = attackerLabelPos;
                }
                attackerLabel.text = $"{supportItemMoveResult.UsedItem.ItemName}";
                defenderLabel.text = $"+ {supportItemMoveResult.DamageHealed}";
            } else if (supportItemMoveResult.Result == SupportItemMoveResult.SupportItemStatus.Buff) {
                attackerLabel.text = $"{supportItemMoveResult.UsedItem.ItemName}";
                defenderLabel.text = "";
            }

            mHasMoveText = true;
            attackerLabel.gameObject.SetActive(true);
            defenderLabel.gameObject.SetActive(true);

            await Task.Run(() => {
                Thread.Sleep(LABEL_DELAY);
            });

            attackerLabel.gameObject.SetActive(false);
            defenderLabel.gameObject.SetActive(false);
            mHasMoveText = false;
        }

        private void FadeUnit(Vector2Int unitPosition) {
            var unitView = mVectorToObjectViews[unitPosition];
            var sprite = unitView.GameObject.GetComponent<SpriteRenderer>();
            sprite.color = FADE_COLOR;
        }

        private void UnfadeUnits() {
            var units = gameViewModel.ControllingPlayer.Units.Where(unit => unit.IsAlive);
            foreach (var unit in units) {
                var unitPos = gameViewModel.GetPositionOfUnit(unit);
                var unitView = mVectorToObjectViews[unitPos] as UnitView;
                var sprite = unitView.GameObject.GetComponent<SpriteRenderer>();
                sprite.color = UNFADE_COLOR;
            }
        }
    }
}
