using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Assets.Scripts.Application;
using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class GameView : MonoBehaviour
    {
        public GameViewModel gameViewModel;

        public new GameObject camera;

        public TileSelector tileSelector;
        public MoveSelector moveSelector;

        public TextMeshProUGUI turnLabel;

        public UnitInformation unitInformation;

        public CombatForecast combatForecast;

        public void ConstructView() {
            var cameraObject = camera.GetComponent<CameraController>();
            cameraObject.minMaxXPosition.Set(0, GameManager.instance.Columns);
            cameraObject.minMaxYPosition.Set(0, GameManager.instance.Rows);

            tileSelector.ConstructTileSelector();
            moveSelector.ConstructMoveSelector();

            unitInformation.ConstructUnitInformation();

            combatForecast.ConstructCombatForecast();

            turnLabel.text = $"Turn {gameViewModel.CurrentTurn}";

            gameViewModel.PropertyChanged += GameViewModel_PropertyChanged;

        }

        private void GameViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "CurrentTurn") {
                turnLabel.text = $"Turn {gameViewModel.CurrentTurn}";
            }
        }

        public void LockCamera() {
            var cameraObject = camera.GetComponent<CameraController>();
            cameraObject.LockCamera();
        }

        public void UnlockCamera() {
            var cameraObject = camera.GetComponent<CameraController>();
            cameraObject.UnlockCamera();
        }
    }
}
