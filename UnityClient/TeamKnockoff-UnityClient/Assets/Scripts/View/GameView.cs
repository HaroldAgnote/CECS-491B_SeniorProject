using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine;
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

            turnLabel.text = $"Turn {gameViewModel.CurrentTurn}";

            gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;
        }

        public void LockCamera() {
            mCamera.LockCamera();
        }

        public void UnlockCamera() {
            mCamera.UnlockCamera();
        }

        private async void GameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "CurrentTurn") {
                turnLabel.text = $"Turn {gameViewModel.CurrentTurn}";
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
