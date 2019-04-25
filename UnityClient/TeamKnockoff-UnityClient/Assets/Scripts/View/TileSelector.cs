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
        #region Public fields
        /// <summary>
        /// The View of the Game system
        /// </summary>
        public GameView gameView;

        /// <summary>
        /// The MoveSelector component of the View
        /// </summary>
        public MoveSelector moveSelector;

        /// <summary>
        /// Prefab containing a general highlighter for tiles when hovering cursor on the map
        /// </summary>
        public GameObject tileHighlightPrefab;

        public GameObject moveHighlightPrefab;

        public GameObject enemyHighlightPrefab;

        public GameObject dangerZoneHighlightPrefab;

        /// <summary>
        /// Prefab containing a highlighter for tiles with allies on them
        /// </summary>
        public GameObject allyHighlightPrefab;

        #endregion

        #region Private Fields

        /// <summary>
        /// The ViewModel of the Game system retrieved from the View
        /// </summary>
        private GameViewModel gameViewModel;

        /// <summary>
        /// The highlighter that will be used when hovering the cursor on the map
        /// </summary>
        private GameObject tileHighlight;

        private List<GameObject> moveLocationHighlights;

        private List<GameObject> attackLocationHighlights;

        private List<GameObject> dangerZoneHighlights;

        /// <summary>
        /// The highlighters used for all ally locations
        /// </summary>
        private List<GameObject> allyLocationHighlights;

        private Dictionary<Unit, HashSet<Vector2Int>> toggledEnemyToLocations;

        private Dictionary<Unit, List<GameObject>> toggledEnemyToLocationHighlights;

        #endregion

        #region Methods

        #region Initializers

        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start() {
            Vector3 point = new Vector3(0, 0, 0);
            tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
            tileHighlight.SetActive(false);

            moveLocationHighlights = new List<GameObject>();
            attackLocationHighlights = new List<GameObject>();
            dangerZoneHighlights = new List<GameObject>();
            allyLocationHighlights = new List<GameObject>();
            toggledEnemyToLocations = new Dictionary<Unit, HashSet<Vector2Int>>();
            toggledEnemyToLocationHighlights = new Dictionary<Unit, List<GameObject>>();
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

            foreach (var unitViewModel in gameViewModel.UnitViewModels) {
                if (unitViewModel.Unit.PlayerNumber != gameViewModel.ControllingPlayer.PlayerNumber) {
                    unitViewModel.PropertyChanged += UnitViewModel_PropertyChanged;

                }
            }

            EnterState();
        }

        private void UnitViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) {
            if (e.PropertyName == "IsAlive") {
                var unitViewModel = sender as UnitViewModel;
                var unit = unitViewModel.Unit;
                if (!unit.IsAlive) {
                    if (toggledEnemyToLocationHighlights.Keys.Contains(unit)) {
                        var enemyHighlights = toggledEnemyToLocationHighlights[unit];
                        foreach (var highlight in enemyHighlights) {
                            Destroy(highlight);
                        }
                        toggledEnemyToLocationHighlights.Remove(unit);
                        // toggledEnemyToLocationHighlights[unit] = new List<GameObject>();
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Entering the Select State for the Player to select a Unit to move
        /// </summary>
        public void EnterState() {
            this.enabled = true;
        }

        /// <summary>
        /// Used to refresh highlighters on ally positions
        /// </summary>
        public void RefreshAllyHighlighters() {
            foreach (var highlight in allyLocationHighlights) {
                Destroy(highlight);

            }
            allyLocationHighlights = new List<GameObject>();

            foreach (var playerUnit in gameViewModel.ControllingPlayer.Units.Where(un => un.IsAlive)) {
                GameObject highlight;
                var allyLoc = gameViewModel.GetPositionOfUnit(playerUnit);
                highlight = Instantiate(allyHighlightPrefab, allyLoc.ToVector3(), Quaternion.identity, gameObject.transform);
                allyLocationHighlights.Add(highlight);
            }
        }

        public void DestroyDangerZones() {
            foreach (var highlight in dangerZoneHighlights) {
                Destroy(highlight);
            }
            dangerZoneHighlights = new List<GameObject>();
        }

        public void RefreshDangerZone() {
            foreach (var highlight in dangerZoneHighlights) {
                Destroy(highlight);
            }
            dangerZoneHighlights = new List<GameObject>();


            var dangerZones = gameViewModel.GetDangerZoneLocations();
            foreach (var dangerZone in dangerZones) {
                GameObject highlight;
                highlight = Instantiate(dangerZoneHighlightPrefab, dangerZone.ToVector3(), Quaternion.identity, gameObject.transform);
                dangerZoneHighlights.Add(highlight);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update() {
            //Converting Mouse Pos to 2D (vector2) World Pos
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (!gameView.IsUpdating && hit) {
                // Don't do anything if hovering over UI element
                if (!EventSystem.current.IsPointerOverGameObject()) {
                    Vector3 point = hit.collider.gameObject.transform.position;
                    // Debug.Log($"Hovering at Point: ({point.x}, {point.y})");

                    tileHighlight.SetActive(true);
                    tileHighlight.transform.position = point;

                    var cursorPosition = point.ToVector2Int();

                    gameViewModel.HoveredSquare = gameViewModel.Squares
                        .SingleOrDefault(sq => sq.Position == cursorPosition);

                    if (gameViewModel.HoveredSquare != null) {
                        RefreshUnitSquares(gameViewModel.HoveredSquare.Unit);
                    }

                    if (Input.GetMouseButtonDown(0)) {
                        gameViewModel.SelectedSquare = gameViewModel.Squares
                            .SingleOrDefault(sq => sq.Position == cursorPosition);

                        var selectedSquare = gameViewModel.SelectedSquare;
                        if (selectedSquare.ObjectAtSquare)
                        {
                            var selectedUnit = selectedSquare.Unit;
                            Debug.Log($"Unit Information: {selectedUnit.UnitInformation}");

                            if (gameViewModel.SelectedUnitBelongsToPlayer 
                                && !selectedUnit.HasMoved 
                                && gameViewModel.IsControllingPlayersTurn) {

                                gameViewModel.SelectedUnit = selectedUnit;

                                ExitState();
                            } else if (!gameViewModel.SelectedUnitBelongsToPlayer) {
                                var enemyDangerLocations = gameViewModel.GetAllAttackLocations(selectedUnit).ToHashSet();

                                if (toggledEnemyToLocationHighlights.Keys.Contains(selectedUnit)) {
                                    var unitEnemyHighlights = toggledEnemyToLocationHighlights[selectedUnit];
                                    foreach (var highlight in unitEnemyHighlights) {
                                        Destroy(highlight);
                                    }
                                    // toggledEnemyToLocationHighlights[selectedUnit] = new List<GameObject>();
                                    toggledEnemyToLocations.Remove(selectedUnit);
                                    toggledEnemyToLocationHighlights.Remove(selectedUnit);
                                } else {
                                    var unitEnemyHighlights = new List<GameObject>();
                                    foreach (Vector2Int loc in enemyDangerLocations) {
                                        var highlight = Instantiate(enemyHighlightPrefab, loc.ToVector3(), Quaternion.identity, gameObject.transform);
                                        unitEnemyHighlights.Add(highlight);
                                    }
                                    toggledEnemyToLocationHighlights.Add(selectedUnit, unitEnemyHighlights);
                                    toggledEnemyToLocations.Add(selectedUnit, enemyDangerLocations);
                                }
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

        public void RefreshToggledEnemySquares() {
            var tempToggledEnemyToHighlights = new Dictionary<Unit, List<GameObject>>();
            foreach (var toggledEnemyToLocHighlights in toggledEnemyToLocationHighlights) {
                var enemyUnit = toggledEnemyToLocHighlights.Key;
                foreach (var highlight in this.toggledEnemyToLocationHighlights[enemyUnit]) {
                    Destroy(highlight);
                }
                if (enemyUnit.IsAlive) {
                    var unitEnemyHighlights = new List<GameObject>();
                    var enemyDangerLocations = gameViewModel.GetAllAttackLocations(enemyUnit);
                    foreach (var loc in enemyDangerLocations) {
                        var highlight = Instantiate(enemyHighlightPrefab, loc.ToVector3(), Quaternion.identity, gameObject.transform);
                        unitEnemyHighlights.Add(highlight);
                    }
                    toggledEnemyToLocations[enemyUnit] = enemyDangerLocations.ToHashSet();
                    tempToggledEnemyToHighlights.Add(enemyUnit, unitEnemyHighlights);
                } else {
                    toggledEnemyToLocations.Remove(enemyUnit);
                    toggledEnemyToLocationHighlights.Remove(enemyUnit);
                }

            }

            foreach (var tempToggledEnemy in tempToggledEnemyToHighlights.Keys) {
                toggledEnemyToLocationHighlights[tempToggledEnemy] = tempToggledEnemyToHighlights[tempToggledEnemy];
            }
        }

        private void RefreshUnitSquares(Unit unit) {
            foreach (var highlight in moveLocationHighlights) {
                Destroy(highlight);
            }

            foreach (var highlight in attackLocationHighlights) {
                Destroy(highlight);
            }

            if (unit != null) {
                if (unit.PlayerNumber == gameViewModel.ControllingPlayer.PlayerNumber && !unit.HasMoved) {
                    var moveLocations = gameViewModel.GetMoveLocations(unit);
                    foreach (Vector2Int loc in moveLocations) {
                        GameObject highlight;
                        var point = new Vector3Int(loc.x, loc.y, 0);
                        highlight = Instantiate(moveHighlightPrefab, point, Quaternion.identity, gameObject.transform);
                        moveLocationHighlights.Add(highlight);
                    }

                    var attackLocations = gameViewModel.GetAllAttackLocations(unit);
                    var filteredAttackLocations = attackLocations.Where(loc => !moveLocations.Contains(loc));

                    foreach (Vector2Int loc in filteredAttackLocations) {
                        GameObject highlight;
                        var point = new Vector3Int(loc.x, loc.y, 0);
                        highlight = Instantiate(enemyHighlightPrefab, point, Quaternion.identity, gameObject.transform);
                        attackLocationHighlights.Add(highlight);
                    }
                } else if (unit.PlayerNumber != gameViewModel.ControllingPlayer.PlayerNumber) {
                    if (!toggledEnemyToLocationHighlights.Keys.Contains(unit)) {
                        var enemyDangerLocations = gameViewModel.GetAllAttackLocations(unit)
                                                                .Where(loc => !toggledEnemyToLocations.Any(enemyToLocation => enemyToLocation.Value.Contains(loc)));
                        foreach (Vector2Int loc in enemyDangerLocations) {
                            GameObject highlight;
                            var point = new Vector3Int(loc.x, loc.y, 0);
                            highlight = Instantiate(enemyHighlightPrefab, point, Quaternion.identity, gameObject.transform);
                            attackLocationHighlights.Add(highlight);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Exiting the Select State when the Player has selected a Unit to move
        /// </summary>
        private void ExitState() {
            this.enabled = false;
            tileHighlight.SetActive(false);

            // Remove Highlight locations
            foreach (var highlight in allyLocationHighlights) {
                Destroy(highlight);
            }

            foreach (var highlight in moveLocationHighlights) {
                Destroy(highlight);
            }

            foreach (var highlight in attackLocationHighlights) {
                Destroy(highlight);
            }

            moveSelector.EnterState();
        }


        #endregion

        #endregion
    }
}
