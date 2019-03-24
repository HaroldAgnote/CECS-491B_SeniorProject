using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Scripts.Application;
using Assets.Scripts.Model.Units;
using Assets.Scripts.ViewModel;
using Assets.Scripts.Utilities.ExtensionMethods;

namespace Assets.Scripts.View {
    /// <summary>
    /// The TileSelector is a View component used for the player to select a Unit to move.
    /// </summary>
    public class TileSelector : MonoBehaviour {

        /// <summary>
        /// The View of the Game system
        /// </summary>
        public GameView gameView;

        /// <summary>
        /// The MoveSelector component of the View
        /// </summary>
        public MoveSelector moveSelector;

        /// <summary>
        /// The ViewModel of the Game system retrieved from the View
        /// </summary>
        private GameViewModel gameViewModel;

        /// <summary>
        /// Prefab containing a general highlighter for tiles when hovering cursor on the map
        /// </summary>
        public GameObject tileHighlightPrefab;

        /// <summary>
        /// Prefab containing a highlighter for tiles with allies on them
        /// </summary>
        public GameObject allyHighlightPrefab;

        /// <summary>
        /// The highlighter that will be used when hovering the cursor on the map
        /// </summary>
        private GameObject tileHighlight;

        /// <summary>
        /// The highlighters used for all ally locations
        /// </summary>
        private List<GameObject> allyLocationHighlights;

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        void Start() {
            Vector3 point = new Vector3(0, 0, 0);
            tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
            tileHighlight.SetActive(false);

        }

        /// <summary>
        /// Constructs the Tile Selector and its components
        /// </summary>
        public void ConstructTileSelector() {
            // If gameView is null, get it from GameManager
            if (gameView == null) {
                gameView = GameManager.instance.view;
            }

            // If moveSelector is null, get it from gameView
            if (moveSelector == null) {
                moveSelector = gameView.moveSelector;
            }

            gameViewModel = gameView.gameViewModel;
            EnterState();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update() {
            //Converting Mouse Pos to 2D (vector2) World Pos
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (hit) {
                // Don't do anything if hovering over UI element
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    Vector3 point = hit.collider.gameObject.transform.position;
                    // Debug.Log($"Hovering at Point: ({point.x}, {point.y})");

                    tileHighlight.SetActive(true);
                    tileHighlight.transform.position = point;

                    var cursorPosition = point.ToVector2Int();

                    gameViewModel.HoveredSquare = gameViewModel.Squares
                        .SingleOrDefault(sq => sq.Position == cursorPosition);

                    if (Input.GetMouseButtonDown(0))
                    {
                        gameViewModel.SelectedSquare = gameViewModel.Squares
                            .SingleOrDefault(sq => sq.Position == cursorPosition);

                        var selectedSquare = gameViewModel.SelectedSquare;
                        if (selectedSquare.ObjectAtSquare)
                        {
                            var selectedUnit = selectedSquare.Unit;
                            Debug.Log($"Unit Information: {selectedUnit.UnitInformation}");

                            if (gameViewModel.SelectedUnitBelongsToPlayer &&
                                !gameViewModel.UnitHasMoved(selectedUnit) &&
                                gameViewModel.IsControllingPlayersTurn)
                            {

                                ExitState(selectedUnit);
                            }
                        }
                    }
                    else if (Input.GetMouseButtonUp(1))
                    {
                        Debug.Log($"Clicking at Point: ({cursorPosition.x}, {cursorPosition.y})");
                    }
                }
            } else {
                tileHighlight.SetActive(false);
            }
        }

        /// <summary>
        /// Entering the Select State for the Player to select a Unit to move
        /// </summary>
        public void EnterState() {
            this.enabled = true;

            allyLocationHighlights = new List<GameObject>();

            foreach (var unit in gameViewModel.ControllingPlayer.Units.Where(u => u.IsAlive)) {
                GameObject highlight;
                var allyLoc = gameViewModel.GetPositionOfUnit(unit);
                highlight = Instantiate(allyHighlightPrefab, allyLoc.ToVector3(), Quaternion.identity, gameObject.transform);
                allyLocationHighlights.Add(highlight);
            }
        }

        /// <summary>
        /// Exiting the Select State when the Player has selected a Unit to move
        /// </summary>
        private void ExitState(Unit selectedUnit) {
            this.enabled = false;
            tileHighlight.SetActive(false);

            // Remove Highlight locations
            foreach (var highlight in allyLocationHighlights) {
                Destroy(highlight);
            }

            moveSelector.EnterState(selectedUnit);
        }
    }
}
