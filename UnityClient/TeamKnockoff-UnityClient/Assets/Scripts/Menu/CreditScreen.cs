using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Application;

namespace Assets.Scripts.Menu {
    public class CreditScreen : MonoBehaviour {
        public Button backButton;

        private void Start() {
            backButton.onClick.AddListener(SceneLoader.instance.GoToStartMenu);
        }
    }
}
