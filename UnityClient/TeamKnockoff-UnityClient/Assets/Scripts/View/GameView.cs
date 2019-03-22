using System.Collections;
using System.Collections.Generic;
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

        public new GameObject camera;

        public TileSelector tileSelector;
        public MoveSelector moveSelector;

        public TextMeshProUGUI turnLabel;

        public UnitInformation unitInformation;

        public CombatForecast combatForecast;

        private Dictionary<Vector2Int, ObjectView> mVectorToObjectViews;

        public void ConstructView(int columns, int rows, Dictionary<Vector2Int, ObjectView> vectorsToObjectViews) {
            var cameraObject = camera.GetComponent<CameraController>();
            cameraObject.minMaxXPosition.Set(0, columns);
            cameraObject.minMaxYPosition.Set(0, rows);

            mVectorToObjectViews = vectorsToObjectViews;

            tileSelector.ConstructTileSelector();
            moveSelector.ConstructMoveSelector();

            unitInformation.ConstructUnitInformation();

            combatForecast.ConstructCombatForecast();

            turnLabel.text = $"Turn {gameViewModel.CurrentTurn}";

            gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;
        }

        public void LockCamera() {
            var cameraObject = camera.GetComponent<CameraController>();
            cameraObject.LockCamera();
        }

        public void UnlockCamera() {
            var cameraObject = camera.GetComponent<CameraController>();
            cameraObject.UnlockCamera();
        }

        private void GameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "CurrentTurn") {
                turnLabel.text = $"Turn {gameViewModel.CurrentTurn}";
            }

            if (e.PropertyName == "LastMove") {
                var gameMove = gameViewModel.LastMove;
                if (gameMove.MoveType == GameMove.GameMoveType.Move) {
                    if (gameMove.StartPosition != gameMove.EndPosition) {
                        var objectView = mVectorToObjectViews[gameMove.StartPosition];

                        // TODO: Update this to use animated movement
                        objectView.UpdatePosition(gameMove.EndPosition);

                        mVectorToObjectViews.Remove(gameMove.StartPosition);
                        mVectorToObjectViews.Add(gameMove.EndPosition, objectView);
                    }
                }

                else if (gameMove.MoveType == GameMove.GameMoveType.Attack || 
                        gameMove.MoveType == GameMove.GameMoveType.Skill) {
                    var endPosition = gameMove.EndPosition;
                    var endObjectView= mVectorToObjectViews[endPosition];

                    if (endObjectView is UnitView) {
                        var unitView= endObjectView as UnitView;

                        var unit = gameViewModel.Squares
                                    .SingleOrDefault(sq => sq.Position == endPosition)
                                    .Unit;

                        if (!unit.IsAlive) {
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

                        if (!unit.IsAlive) {
                            unitView.GameObject.SetActive(false);
                            mVectorToObjectViews.Remove(gameMove.StartPosition);
                        }
                    }
                }
            }
        }
    }
}
