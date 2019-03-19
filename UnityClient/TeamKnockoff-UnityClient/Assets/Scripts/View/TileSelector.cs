using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Model.Units;
using Assets.Scripts.ViewModel;
using Assets.Scripts.ExtensionMethods;

namespace Assets.Scripts.View {
    public class TileSelector : MonoBehaviour {
        [SerializeField]
        private GameView gameView;

        [SerializeField]
        private MoveSelector moveSelector;

        private GameViewModel gameViewModel;

        public GameObject tileHighlightPrefab;
        public GameObject allyHighlightPrefab;
        private GameObject tileHighlight;
        private List<GameObject> allyLocationHighlights;

        // Start is called before the first frame update
        void Start() {
            Vector3 point = new Vector3(0, 0, 0);
            tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
            tileHighlight.SetActive(false);

        }

        public void ConstructTileSelector() {
            gameViewModel = gameView.gameViewModel;
            EnterState();
        }

        // Update is called once per frame
        void Update() {
            //Converting Mouse Pos to 2D (vector2) World Pos
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (hit) {
                Vector3 point = hit.collider.gameObject.transform.position;
                // Debug.Log($"Hovering at Point: ({point.x}, {point.y})");

                tileHighlight.SetActive(true);
                tileHighlight.transform.position = point;

                if (Input.GetMouseButtonDown(0)) {
                    gameViewModel.SelectedSquare = gameViewModel.Squares
                        .SingleOrDefault(sq => sq.Position == point.ToVector2Int());

                    var selectedObject = gameViewModel.SelectedSquare.GameObject;
                    if (selectedObject != null) {
                        var selectedUnit = selectedObject as UnitViewModel;
                        Debug.Log($"Unit Information: {selectedUnit.Unit.UnitInformation}");

                        // TODO: Maybe update unit information in UI here?

                        if (gameViewModel.SelectedUnitBelongsToPlayer && 
                            !gameViewModel.UnitHasMoved(selectedUnit) &&
                            gameViewModel.IsControllingPlayersTurn) {

                            ExitState(selectedUnit.Unit);
                        }
                    }
                } else if (Input.GetMouseButtonUp(1)) {
                    Debug.Log($"Clicking at Point: ({point.x}, {point.y})");
                }
            } else {
                tileHighlight.SetActive(false);
            }
        }

        public void EnterState() {
            this.enabled = true;

            allyLocationHighlights = new List<GameObject>();

            foreach (var unit in gameViewModel.ControlllingPlayer.Units) {
                GameObject highlight;
                var allyLoc = gameViewModel.GetPositionOfUnit(unit);
                highlight = Instantiate(allyHighlightPrefab, allyLoc.ToVector3(), Quaternion.identity, gameObject.transform);
                allyLocationHighlights.Add(highlight);
            }
        }

        private void ExitState(Unit selectedUnit) {
            this.enabled = false;
            tileHighlight.SetActive(false);

            foreach (var highlight in allyLocationHighlights) {
                Destroy(highlight);
            }

            moveSelector.EnterState(selectedUnit);
        }
    }
}
