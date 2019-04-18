using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Assets.Scripts.Application;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;
using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class GameView : MonoBehaviour {

        public GameViewModel gameViewModel;

        public CameraController mCamera;

        public TileSelector tileSelector;
        public MoveSelector moveSelector;

        public TextMeshProUGUI turnLabel;

        public UnitInformation unitInformation;

        public CombatForecast combatForecast;

        public GameOverScreen gameOverScreen;

        public PauseMenu mPauseMenu;

        public Button mPauseButton;

        public Button mEndTurnButton;

        private Dictionary<Vector2Int, ObjectView> mVectorToObjectViews;

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

            mPauseMenu.ConstructPauseMenu();

            turnLabel.text = $"Turn {gameViewModel.CurrentTurn}";

            mPauseButton.onClick.AddListener(PauseGame);
            mEndTurnButton.onClick.AddListener(EndTurn);

            gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;
        }

        public void EndTurn() {
            var units = gameViewModel.ControllingPlayer.Units;
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
                } else {
                    mPauseButton.interactable = true;
                    mEndTurnButton.interactable = true;
                }
                turnLabel.text = $"{gameViewModel.CurrentPlayer.Name} - Turn {gameViewModel.CurrentTurn}";
            }

            if (e.PropertyName == "CurrentTurn") {
                turnLabel.text = $"{gameViewModel.CurrentPlayer.Name} - Turn {gameViewModel.CurrentTurn}";
            }

            if (e.PropertyName == "Squares") {
                tileSelector.RefreshAllyHighlighters();
            }

            if (e.PropertyName == "LastMove") {
                var gameMove = gameViewModel.LastMove;
                if (gameMove.MoveType == GameMove.GameMoveType.Move) {
                    if (gameMove.StartPosition != gameMove.EndPosition) {
                        var objectView = mVectorToObjectViews[gameMove.StartPosition];

                        tileSelector.gameObject.SetActive(false);
                        (objectView as UnitView).UpdatePosition(gameMove.Path);
                        var unitMover = objectView.GameObject.GetComponent<UnitMover>();

                        mVectorToObjectViews.Remove(gameMove.StartPosition);
                        mVectorToObjectViews.Add(gameMove.EndPosition, objectView);

                        await Task.Run(() => {
                            mIsUpdating = true;
                            while (unitMover.IsMoving) { }
                            mIsUpdating = false;
                        });

                        tileSelector.gameObject.SetActive(true);
                    }
                } else if (gameMove.MoveType == GameMove.GameMoveType.Attack || 
                        gameMove.MoveType == GameMove.GameMoveType.Skill) {
                    var endPosition = gameMove.EndPosition;
                    var endObjectView= mVectorToObjectViews[endPosition];

                    if (endObjectView is UnitView) {
                        var unitView= endObjectView as UnitView;

                        var unit = gameViewModel.Squares
                                    .SingleOrDefault(sq => sq.Position == endPosition)
                                    .Unit;

                        if (unit == null || !unit.IsAlive) {
                            unitView.GameObject.SetActive(false);
                            mVectorToObjectViews.Remove(gameMove.EndPosition);
                        }
                    }

                    var startPosition = gameMove.StartPosition;
                    var startObjectView= mVectorToObjectViews[startPosition];

                    if (startObjectView is UnitView) {
                        var unitView= startObjectView as UnitView;

                        var unit = gameViewModel.Squares
                                    .SingleOrDefault(sq => sq.Position == startPosition)
                                    .Unit;

                        if (unit == null || !unit.IsAlive) {
                            unitView.GameObject.SetActive(false);
                            mVectorToObjectViews.Remove(gameMove.StartPosition);
                        }
                    }
                }
            }
        }
    }
}
