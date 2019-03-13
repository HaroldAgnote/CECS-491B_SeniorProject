using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Application;
using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class GameView : MonoBehaviour
    {
        public GameViewModel gameViewModel;

        public new GameObject camera;

        public void ConstructView() {
            var cameraObject = camera.GetComponent<CameraController>();
            cameraObject.minMaxXPosition.Set(0, GameManager.instance.Columns);
            cameraObject.minMaxYPosition.Set(0, GameManager.instance.Rows);
        }
    }
}
