using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Scripts.Application;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Items;
using Assets.Scripts.ViewModel;
using Assets.Scripts.Utilities.ExtensionMethods;

namespace Assets.Scripts.View {
    
    /// <summary>
    /// MoveSelector is a View component used for the Player to select a Action for a Unit
    /// </summary>
    public class MoveSelector : MonoBehaviour {

        #region Constants
        /// <summary>
        /// Constant vector set to 'null' (-1,-1) for error checking
        /// </summary>
        public static Vector2Int NULL_VECTOR = new Vector2Int(-1, -1);

        #endregion

        #region public fields

        /// <summary>
        /// The View of the Game System
        /// </summary>
        public GameView gameView;

        /// <summary>
        /// View component to allow Player to enter Select State
        /// </summary>
        public TileSelector tileSelector;

        /// <summary>
        /// Prefab containing a general highlighter for tiles when hovering cursor on the map
        /// </summary>
        public GameObject tileHighlightPrefab;
        
        /// <summary>
        /// Prefab containing a highlighter for a selected square
        /// </summary>
        public GameObject tileSelectedPrefab;

        /// <summary>
        /// Prefab containing a highlighter for tiles with allies on them
        /// </summary>
        public GameObject allyLocationPrefab;

        /// <summary>
        /// Prefab containing a highlighter for locations a Unit can move to
        /// </summary>
        public GameObject moveLocationPrefab;

        /// <summary>
        /// Prefab containing a highlighter for locations a Unit can attack
        /// </summary>
        public GameObject attackLocationPrefab;

        /// <summary>
        /// The menu controlling Unit actions
        /// </summary>
        public UnitMenu unitMenu;

        public CombatForecast combatForecast;

        #endregion

        #region Private Fields

        /// <summary>
        /// The ViewModel of the Game system retrieved from the View
        /// </summary>
        private GameViewModel gameViewModel;

        /// <summary>
        /// The Current GameMove the Player will apply to the Game
        /// </summary>
        private GameMove CurrentGameMove;

        /// <summary>
        /// The Selected Unit that will be moving
        /// </summary>
        private Unit selectedUnit;

        /// <summary>
        /// The current Skill that the Unit will be using
        /// </summary>
        private ActiveSkill selectedSkill;

        /// <summary>
        /// The current Item that the Unit can consume
        /// </summary>
        private ConsumableItem selectedItem;

        /// <summary>
        /// Set of locations Unit can move to
        /// </summary>
        private IEnumerable<Vector2Int> moveLocations;

        /// <summary>
        /// The list of locations a Unit will follow in a path to move
        /// </summary>
        private IEnumerable<Vector2Int> pathLocations;

        /// <summary>
        /// Set of locations a Unit can attack
        /// </summary>
        private IEnumerable<Vector2Int> attackLocations;

        /// <summary>
        /// Set of locations a Unit can use a Skill they are currently equipped with
        /// </summary>
        private IEnumerable<Vector2Int> currentSkillLocations;

        private IEnumerable<Vector2Int> currentItemLocations;

        /// <summary>
        /// Set of Skills where each Skill is mapped to possible locations they can be used at
        /// </summary>
        private Dictionary<ActiveSkill, HashSet<Vector2Int>> skillsToLocations;


        private Dictionary<ConsumableItem, HashSet<Vector2Int>> itemToLocations;
        
        /// <summary>
        /// The current highlighter for hovered tiles
        /// </summary>
        private GameObject tileHighlight;

        /// <summary>
        /// Active highlighters for ally locations
        /// </summary>
        private List<GameObject> allyLocationHighlights;

        /// <summary>
        /// Active highlighters for move locations
        /// </summary>
        private List<GameObject> moveLocationHighlights;

        /// <summary>
        /// Active highlighters for attack locations
        /// </summary>
        private List<GameObject> attackLocationHighlights;

        /// <summary>
        /// Active highlighters for path locations
        /// </summary>
        private List<GameObject> pathLocationHighlights;

        /// <summary>
        /// Flag determining if a Unit is waiting for a Move to be confirmed
        /// </summary>
        private bool waitingForMove;

        /// <summary>
        /// Flag determining if a Unit is waiting for an Attack to be confirmed at a chosen position
        /// </summary>
        private bool waitingForAttack;

        /// <summary>
        /// Flag determining if a Unit is waiting for a Skill to be selected
        /// </summary>
        private bool waitingForSkillChoice;

        /// <summary>
        /// Flag determining if a Unit is waiting for a Skill to be confirmed at a chosen position
        /// </summary>
        private bool waitingForSkillMove;

        private bool waitingForItemChoice;

        private bool waitingForItemMove;

        /// <summary>
        /// The starting position of the Unit
        /// </summary>
        private Vector2Int startPoint;

        /// <summary>
        /// The position the Unit will move to
        /// </summary>
        private Vector2Int movedPoint;

        /// <summary>
        /// The position the Unit will attack
        /// </summary>
        private Vector2Int attackPoint;

        /// <summary>
        /// The position the Unit will use a Skill
        /// </summary>
        private Vector2Int skillPoint;

        private Vector2Int itemPoint;

        private List<Vector2Int> movePath;
 
            #region Unit Menu Options

        /// <summary>
        /// Possible Options that can be on the Main Menu
        /// </summary>
        public enum UnitMenuOptions {
            Attack,
            Skills,
            Items,
            Wait,
            Cancel
        }


        /// <summary>
        /// The current menu options that willl be on the Unit Main Menu
        /// </summary>
        private HashSet<UnitMenuOptions> currentMenuOptions;

        /// <summary>
        /// Determines if the Unit Main Menu has already been set up with options
        /// </summary>
        private bool mainMenuCreated;

        #endregion

        #endregion

        #region Methods

        #region Initializers
        
        /// <summary>
        /// Start is called before the first frame update
        /// </summary>
        private void Start() {
            this.enabled = false;
            Vector3 point = new Vector3(0, 0, 0);
            tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
            tileHighlight.SetActive(false);
            unitMenu.SetActive(false);

            pathLocationHighlights = new List<GameObject>();
        }

        /// <summary>
        /// Initializes MoveSelector View Component and properties
        /// </summary>
        public void ConstructMoveSelector() {
            if (gameView == null) {
                gameView = GameManager.instance.view;
            }

            if (tileSelector == null) {
                tileSelector = gameView.tileSelector;
            }

            gameViewModel = gameView.gameViewModel;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Enter Action State
        /// </summary>
        public void EnterState() {

            gameView.mEndTurnButton.interactable = false;

            this.enabled = true;

            // Initialize view model properties in Action State
            gameViewModel.CombatMode = false;
            gameViewModel.TargetSquare = null;

            combatForecast.ResetLabels();
            combatForecast.DisableCombatForecast();

            // Initialize fields in Action State
            waitingForMove = false;
            waitingForAttack = false;
            waitingForSkillChoice = false;
            waitingForSkillMove = false;
            waitingForItemChoice = false;
            waitingForItemMove = false;

            selectedUnit = gameViewModel.SelectedSquare.Unit;
            selectedSkill = null;
            selectedItem = null;

            // All vectors start at NULL
            startPoint = NULL_VECTOR;
            movedPoint = NULL_VECTOR;
            skillPoint = NULL_VECTOR;
            itemPoint = NULL_VECTOR;
            attackPoint = NULL_VECTOR;

            // Menu options initialize
            mainMenuCreated = false;
            unitMenu.CreateMainMenu();
            unitMenu.SwitchtoMainMenu();
            currentMenuOptions = new HashSet<UnitMenuOptions>();

            // Retrieve and show available Move locations
            moveLocations = gameViewModel.MovesForUnit;
            moveLocationHighlights = new List<GameObject>();

            foreach (Vector2Int loc in moveLocations) {
                GameObject highlight;
                var point = new Vector3Int(loc.x, loc.y, 0);
                highlight = Instantiate(moveLocationPrefab, point, Quaternion.identity, gameObject.transform);
                moveLocationHighlights.Add(highlight);
            }

            // Retrieve and show available attack locations (including damage skills)
            attackLocations = gameViewModel.AttacksForUnit;
            skillsToLocations = gameViewModel.SkillsForUnit;
            itemToLocations = gameViewModel.ItemsForUnits;
            var attackSkillLocations = skillsToLocations
                                    .Where(sk => sk.Key is SingleDamageSkill)
                                    .SelectMany(sk => sk.Value);

            attackLocationHighlights = new List<GameObject>();

            var filteredAttackLocations = attackLocations.Where(x => !moveLocations.Contains(x));
            var filteredAttackSkillHighlights = attackSkillLocations.Where(x => !moveLocations.Contains(x) && !filteredAttackLocations.Contains(x));

            foreach (Vector2Int loc in filteredAttackLocations) {
                GameObject highlight;
                var point = new Vector3Int(loc.x, loc.y, 0);
                highlight = Instantiate(attackLocationPrefab, point, Quaternion.identity, gameObject.transform);
                attackLocationHighlights.Add(highlight);
            }

            foreach (Vector2Int loc in filteredAttackSkillHighlights) {
                GameObject highlight;
                var point = new Vector3Int(loc.x, loc.y, 0);
                highlight = Instantiate(attackLocationPrefab, point, Quaternion.identity, gameObject.transform);
                attackLocationHighlights.Add(highlight);
            }

            // Show ally positions
            allyLocationHighlights = new List<GameObject>();

            foreach (var playerUnit in gameViewModel.ControllingPlayer.Units.Where(un => un.IsAlive)) {
                GameObject highlight;
                var allyLoc = gameViewModel.GetPositionOfUnit(playerUnit);
                highlight = Instantiate(allyLocationPrefab, allyLoc.ToVector3(), Quaternion.identity, gameObject.transform);
                allyLocationHighlights.Add(highlight);
            }
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
                highlight = Instantiate(allyLocationPrefab, allyLoc.ToVector3(), Quaternion.identity, gameObject.transform);
                allyLocationHighlights.Add(highlight);
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
                var point = hit.collider.gameObject.transform.position;

                // Debug.Log($"Hovering at Point: ({point.x}, {point.y})");
                tileHighlight.SetActive(true);
                tileHighlight.transform.position = point;

                var cursorPosition = point.ToVector2Int();

                // Set target square to attack location when hovering
                if (waitingForAttack) {
                    if (attackLocations.Contains(cursorPosition)) {
                        gameViewModel.TargetSquare = gameViewModel.Squares
                                .SingleOrDefault(sq => sq.Position == cursorPosition);

                        var enemyUnit = gameViewModel.TargetSquare.Unit;
                        if (enemyUnit != null) {
                            combatForecast.EnableCombatForecast();
                            combatForecast.UpdateForecast(movedPoint, selectedUnit, enemyUnit);
                        }
                    }
                }

                // Set target square to skill location when hovering
                if (waitingForSkillMove) {
                    if (currentSkillLocations.Contains(cursorPosition)) {
                        gameViewModel.TargetSquare = gameViewModel.Squares
                                .SingleOrDefault(sq => sq.Position == cursorPosition);

                        var targetUnit = gameViewModel.TargetSquare.Unit;
                        if (targetUnit != null && selectedSkill != null && selectedSkill is SingleTargetSkill) {
                            combatForecast.EnableCombatForecast();
                            combatForecast.UpdateForecastWithSkill(movedPoint, selectedUnit, targetUnit, selectedSkill as SingleTargetSkill);
                        }

                    }
                }

                // Render shortest path when hovering
                if (!waitingForMove) {
                    if (moveLocations.Contains(cursorPosition)) {
                        pathLocations = gameViewModel.GetShortestPath(cursorPosition);
                    } else if (attackLocations.Contains(cursorPosition)) {
                        pathLocations = gameViewModel.GetShortestPathToAttack(cursorPosition);
                    } else if (skillsToLocations.Any(sk => sk.Value.Contains(cursorPosition))) {
                        pathLocations = gameViewModel.GetShortestPathToSkill(cursorPosition);
                    }
                    RefreshPathHighlighters();
                }

                if (Input.GetMouseButtonDown(0)) {
                    if (!EventSystem.current.IsPointerOverGameObject()) {
                        CheckCursorClick(cursorPosition);
                    }
                }
            } else {
                tileHighlight.SetActive(false);
            }
        }

        /// <summary>
        /// Check where the cursor was clicked to select/perform action
        /// </summary>
        /// <param name="cursorPosition">The position that was clicked</param>
        private void CheckCursorClick(Vector2Int cursorPosition) {

            if ((!moveLocations.Contains(cursorPosition)
                && !attackLocations.Contains(cursorPosition)
                && !skillsToLocations.Any(sk => sk.Value.Contains(cursorPosition))
                && !itemToLocations.Any(it => it.Value.Contains(cursorPosition)))
                || (waitingForAttack && !attackLocations.Contains(cursorPosition))
                || (waitingForSkillMove && !currentSkillLocations.Contains(cursorPosition))
                || (waitingForItemMove && !currentItemLocations.Contains(cursorPosition))) {

                // Cancel Move since player clicked outside all usable locations
                CancelMove();

            } else if (moveLocations.Contains(cursorPosition)) {

                // Move Action
                CheckMovePositionClick(cursorPosition);
            } else if (attackLocations.Contains(cursorPosition)) { 

                // Attack Action
                CheckAttackPositionClick(cursorPosition);
            } else if (skillsToLocations.Values.Any(locs => locs.Contains(cursorPosition))) {

                // Skill Action
                CheckSkillPositionClick(cursorPosition);
            } else if (itemToLocations.Values.Any(locs => locs.Contains(cursorPosition)))
            {
                // Item Action
                CheckItemPositionClick(cursorPosition);
            }
        }

        /// <summary>
        /// Invoke Move Actions when clicking in Move location
        /// </summary>
        /// <param name="cursorPosition">The position that was clicked</param>
        private void CheckMovePositionClick(Vector2Int cursorPosition) {

            // Clicking for the first time
            if (!waitingForMove) {

                movedPoint = cursorPosition;
                startPoint = gameViewModel.SelectedSquare.Position;

                // TODO: Phantom Movement!
                pathLocations = gameViewModel.GetShortestPath(cursorPosition);

                RefreshPathHighlighters();

                // If an enemy is within range to attack, add Attack option
                if (gameViewModel.EnemyWithinRange(movedPoint, selectedUnit.MainWeapon.Range)) {

                    currentMenuOptions.Add(UnitMenuOptions.Attack);
                }

                // If a Unit has at least one Skill that is within use range of a Unit,
                // add Skill option
                if (selectedUnit.Skills.Count > 0
                        && selectedUnit.Skills
                        .Where(sk => sk is SingleTargetSkill)
                        .Select(sk => sk as SingleTargetSkill)
                        .Any(sk => 
                        (sk.CanTargetSelf)
                        || (sk is SingleSupportSkill && gameViewModel.AllyWithinRange(movedPoint, sk.Range))
                        || (sk is SingleDamageSkill && gameViewModel.EnemyWithinRange(movedPoint, sk.Range)))) {

                    currentMenuOptions.Add(UnitMenuOptions.Skills);

                }

                // If a Unit has at least one Item, add Item option
                if (selectedUnit.Items.Count > 0) {
                    currentMenuOptions.Add(UnitMenuOptions.Items);
                }

                SetupMainUnitMenu();
                WaitForChoice();
            } else {
                // Player double clicked position, so just Move and Wait
                if (cursorPosition == movedPoint) {
                    ApplyWaitMove();
                }
            }
        }

        /// <summary>
        /// Invoke attack actions when attack location is clicked
        /// </summary>
        /// <param name="cursorPosition">The position that was clicked</param>
        private void CheckAttackPositionClick(Vector2Int cursorPosition) {

            // Don't do anything unless there is an enemy at this position
            if (gameViewModel.EnemyAtPoint(cursorPosition)) {

                // Clicking for the first time
                // or clicking another valid attack location (switching targets)
                if ((!waitingForMove 
                    && !waitingForAttack
                    && !waitingForSkillChoice
                    && !waitingForSkillMove)
                    || (waitingForMove && attackPoint != NULL_VECTOR && attackPoint != cursorPosition)) {

                    attackPoint = cursorPosition;

                    // If clicking for the first time, enable combat mode
                    // and update target square
                    gameViewModel.CombatMode = true;
                    gameViewModel.TargetSquare = gameViewModel.Squares
                            .SingleOrDefault(sq => sq.Position == cursorPosition);

                    startPoint = gameViewModel.SelectedSquare.Position;
                    pathLocations = gameViewModel.GetShortestPathToAttack(cursorPosition);
                    movedPoint = pathLocations.Last();

                    var enemyUnit = gameViewModel.TargetSquare.Unit;

                    combatForecast.EnableCombatForecast();
                    combatForecast.UpdateForecast(movedPoint, selectedUnit, enemyUnit);

                    // TODO: Phantom movement to tile of MINIMUM valid
                    // Attack Range to attack point

                    RefreshPathHighlighters();

                    // Attack option added automatically when clicking on valid attack location
                    currentMenuOptions.Add(UnitMenuOptions.Attack);


                    // If a Unit has at least one Skill that is within use range of a Unit,
                    // add Skill option
                    if (selectedUnit.Skills.Count > 0
                            && selectedUnit.Skills
                            .Where(sk => sk is SingleTargetSkill)
                            .Select(sk => sk as SingleTargetSkill)
                            .Any(sk => 
                            (sk.CanTargetSelf)
                            || (sk is SingleSupportSkill && gameViewModel.AllyWithinRange(movedPoint, sk.Range))
                            || (sk is SingleDamageSkill && gameViewModel.EnemyWithinRange(movedPoint, sk.Range)))) {

                        // Set Skill Point to the Cursor Position
                        skillPoint = cursorPosition;
                        currentMenuOptions.Add(UnitMenuOptions.Skills);

                    }


                    // If a Unit has at least one Item, add Item option
                    if (selectedUnit.Items.Count > 0) {
                        itemPoint = cursorPosition;
                        currentMenuOptions.Add(UnitMenuOptions.Items);
                    }

                    SetupMainUnitMenu();
                    WaitForChoice();
                } else {

                    // If not waiting for Skill Move
                    // and if waiting for Attack location to click
                    // or if double clicking on same attack location
                    if (!waitingForSkillMove && (waitingForAttack || cursorPosition == attackPoint)) {
                        attackPoint = cursorPosition;
                        ApplyAttackMove();
                    } else if (waitingForSkillMove) {

                        // Use skill on attack location within attack range
                        // if waiting for Skill Move
                        skillPoint = cursorPosition;
                        ApplySkillMove();
                    }
                }
            }
        }

        /// <summary>
        /// Invoke skill actions when clicking on a skill possible location
        /// </summary>
        /// <param name="cursorPosition">The position that was clicked</param>
        private void CheckSkillPositionClick(Vector2Int cursorPosition) {

            // Don't do anything unless there is an enemy or ally at this position 
            if (gameViewModel.EnemyAtPoint(cursorPosition) || gameViewModel.AllyAtPoint(cursorPosition)) {

                // Clicking for the first time
                // or clicking another valid skill location (switching targets)
                if ((!waitingForMove 
                    && !waitingForAttack
                    && !waitingForSkillChoice
                    && !waitingForSkillMove)
                    || (waitingForMove && skillPoint != NULL_VECTOR && skillPoint != cursorPosition)) {

                    skillPoint = cursorPosition;

                    startPoint = gameViewModel.SelectedSquare.Position;
                    // Use best skill to get shortest path
                    pathLocations = gameViewModel.GetShortestPathToSkill(cursorPosition);
                    movedPoint = pathLocations.Last();

                    // TODO: Phantom movement to tile of MINIMUM valid
                    // Skill Range to skill point

                    RefreshPathHighlighters();

                    currentMenuOptions.Add(UnitMenuOptions.Skills);

                    if (selectedUnit.Items.Count > 0) {
                        itemPoint = cursorPosition;
                        currentMenuOptions.Add(UnitMenuOptions.Items);
                    }
                    SetupMainUnitMenu();
                    WaitForChoice();
                } else {
                    if (waitingForSkillChoice) {
                        SkillMenu();
                    }
                    if (currentSkillLocations != null && currentSkillLocations.Contains(cursorPosition)) {
                        if (waitingForSkillMove) {
                            skillPoint = cursorPosition;
                            ApplySkillMove();
                        }
                    }
                }
            } else if (selectedSkill != null && (selectedSkill as SingleTargetSkill).CanTargetSelf) {
                if (currentSkillLocations != null && currentSkillLocations.Contains(cursorPosition)) {
                    if (waitingForSkillMove) {
                        skillPoint = cursorPosition;
                        ApplySkillMove();
                    }
                }
            }
        }

        /// <summary>
        /// Set up and show Unit Main Menu 
        /// </summary>
        private void SetupMainUnitMenu() {
            unitMenu.SetActive(true);

            // Prevent duplicate menu options from being added
            if (!mainMenuCreated) {
                if (currentMenuOptions.Contains(UnitMenuOptions.Attack)) {
                    var attackButton = unitMenu.CreateButton("ATTACK");
                    attackButton.onClick.AddListener(CheckAttackOptions);
                }

                if (currentMenuOptions.Contains(UnitMenuOptions.Skills)) {
                    var skills = unitMenu.CreateButton("SKILLS");
                    skills.onClick.AddListener(SkillMenu);
                }

                if (currentMenuOptions.Contains(UnitMenuOptions.Items)) {
                    var skills = unitMenu.CreateButton("ITEMS");
                    skills.onClick.AddListener(ItemMenu);
                }

                var waitButton = unitMenu.CreateButton("WAIT");
                waitButton.onClick.AddListener(ApplyWaitMove);

                var cancelButton = unitMenu.CreateButton("CANCEL");
                cancelButton.onClick.AddListener(CancelMove);

                mainMenuCreated = true;
            }

        }

        /// <summary>
        /// Lock Camera and enable flag for waiting for move
        /// </summary>
        private void WaitForChoice() {
            // gameView.LockCamera();
            waitingForMove = true;
        } 

        /// <summary>
        /// Apply Position Move to Game
        /// </summary>
        private void ApplyMoveUnit() {
            Debug.Log("Move");
            CurrentGameMove = new GameMove(startPoint, movedPoint, pathLocations.ToList());
            gameViewModel.ApplyMove(CurrentGameMove);
        }

        /// <summary>
        /// Set up attack options
        /// </summary>
        private void CheckAttackOptions() {

            // If Player hasn't chosen an attack position yet, 
            // highlight all possible positions that can be attacked
            if (attackPoint == NULL_VECTOR) {

                waitingForAttack = true;
                waitingForSkillChoice = false;
                waitingForSkillMove = false;
                gameViewModel.CombatMode = true;

                unitMenu.SetActive(false);

                // Destroy move and attack highlighters only
                foreach (GameObject highlight in moveLocationHighlights) {
                    Destroy(highlight);
                }

                foreach (GameObject highlight in attackLocationHighlights) {
                    Destroy(highlight);
                }

                moveLocations = new List<Vector2Int>();

                skillsToLocations = new Dictionary<ActiveSkill, HashSet<Vector2Int>>();

                itemToLocations = new Dictionary<ConsumableItem, HashSet<Vector2Int>>();

                // Create new attack highlighters on surrounding attack positions
                attackLocationHighlights = new List<GameObject>();

                attackLocations = gameViewModel.GetSurroundingLocationsAtPoint(movedPoint, selectedUnit.MainWeapon.Range);

                foreach (Vector2Int loc in attackLocations) {
                    GameObject highlight;
                    var point = new Vector3Int(loc.x, loc.y, 0);
                    highlight = Instantiate(attackLocationPrefab, point, Quaternion.identity, gameObject.transform);
                    attackLocationHighlights.Add(highlight);
                }
            } else {
                // Player has already chosen attack position, so attack the Unit
                ApplyAttackMove();
            }
        }

        /// <summary>
        /// Apply Attack Move to Game
        /// </summary>
        private async void ApplyAttackMove() {

            // Move Unit first to move position before attacking
            if (startPoint != movedPoint) {
                ApplyMoveUnit();
                await Task.Run(() => {
                    while (gameView.IsUpdating) { }
                });
            }

            Debug.Log("Attack");
            CurrentGameMove = new GameMove(movedPoint, attackPoint, GameMove.GameMoveType.Attack);
            gameViewModel.ApplyMove(CurrentGameMove);
            ExitState();
        }

        /// <summary>
        /// Show Skill Sub Menu
        /// </summary>
        private void SkillMenu() {
            unitMenu.CreateSubMenu();
            unitMenu.SwitchtoSubMenu();

            unitMenu.mBackButton.onClick.AddListener(() => {
                waitingForSkillChoice = false;
            });

            waitingForMove = true;
            waitingForSkillChoice = true;

            // Get all available active skills
            var availableSkills = selectedUnit.Skills
                                    .Where(sk => sk is ActiveSkill)
                                    .Select(sk => sk as ActiveSkill);

            foreach (var skill in availableSkills) {
                // Create button for skill
                var skillButton = unitMenu.CreateButton(skill.SkillName.ToUpper());
                skillButton.onClick.AddListener(() => {
                    selectedSkill = skill;
                    CheckSkillOptions();
                });

                if (skill is SingleTargetSkill) {
                    var targetSkill = skill as SingleTargetSkill;
                    var trigger = skillButton.gameObject.AddComponent<EventTrigger>();
                    var pointerEnter = new EventTrigger.Entry();
                    pointerEnter.eventID = EventTriggerType.PointerEnter;
                    pointerEnter.callback.AddListener((e) => {
                        if (skillPoint != NULL_VECTOR) {
                            var enemyUnit = gameViewModel.Squares.SingleOrDefault(sq => sq.Position == skillPoint).Unit;
                            if (enemyUnit != null) {
                                combatForecast.UpdateForecastWithSkill(movedPoint, selectedUnit, enemyUnit, targetSkill);
                            }
                        }
                    });
                }


                // Disable button if Unit does not have enough HP to use Skill
                if (!skill.CanUse(selectedUnit)) {
                    skillButton.interactable = false;
                }

                // Disable button if Skill cannot be used on target position
                if (skill is SingleTargetSkill && skillPoint != NULL_VECTOR) {
                    var singleTargetSkill = skill as SingleTargetSkill;
                    if (!gameViewModel.SkillUsableOnTarget(singleTargetSkill, skillPoint)) {
                        skillButton.interactable = false;
                    }
                }

                // Disable button if Skill has no usable targets
                if (skill is SingleTargetSkill && skillPoint == NULL_VECTOR && skillsToLocations.ContainsKey(skill)) {
                    var singleTargetSkill = skill as SingleTargetSkill;
                    var locs = skillsToLocations[skill];
                    if (locs.All(pos => !gameViewModel.SkillUsableOnTarget(singleTargetSkill, pos))) {
                        skillButton.interactable = false;
                    }
                }
            }

        }

        /// <summary>
        /// Set up Skill options
        /// </summary>
        private void CheckSkillOptions() {

            // Show Skill Submenu if Skill hasn't been selected
            if (selectedSkill == null) {
                SkillMenu();
            }

            // Show Damage Skill options if Skill is Damage Skill
            if (selectedSkill is SingleDamageSkill) {
                CheckDamageSkillOptions();
            }

            // Show Support Skill options if Skill is Support Skill
            if (selectedSkill is SingleSupportSkill) {
                CheckSupportSkillOptions();
            }
        }

        /// <summary>
        /// Set up damage skill options
        /// </summary>
        private void CheckDamageSkillOptions() {

            // If Player hasn't chosen a skill position yet, 
            // highlight all possible positions that can a skill can be used
            if (skillPoint == NULL_VECTOR) {

                waitingForSkillMove = true;
                waitingForAttack = false;
                gameViewModel.CombatMode = true;
                unitMenu.SetActive(false);
                // TODO: Enable flag that using damage skill for forecasting

                // Destroy move and attack highlighters only
                foreach (GameObject highlight in moveLocationHighlights) {
                    Destroy(highlight);
                }

                foreach (GameObject highlight in attackLocationHighlights) {
                    Destroy(highlight);
                }

                moveLocations = new List<Vector2Int>();

                // Create new attack highlighters on surrounding attack positions based on skill range
                attackLocationHighlights = new List<GameObject>();

                currentSkillLocations = gameViewModel.GetSurroundingLocationsAtPoint(movedPoint, (selectedSkill as SingleTargetSkill).Range);

                foreach (Vector2Int loc in currentSkillLocations) {
                    GameObject highlight;
                    var point = new Vector3Int(loc.x, loc.y, 0);
                    highlight = Instantiate(attackLocationPrefab, point, Quaternion.identity, gameObject.transform);
                    attackLocationHighlights.Add(highlight);
                }

                // Use only positions where a Skill is usable on a target
                currentSkillLocations = currentSkillLocations
                                            .Where(pos => 
                                                gameViewModel.SkillUsableOnTarget((selectedSkill as SingleTargetSkill), pos));
            }
            else {
                // Player has already chosen skill position, so use the Skill
                ApplySkillMove();
            }
        }

        /// <summary>
        /// Set up support skill options
        /// </summary>
        private void CheckSupportSkillOptions() {
            if (skillPoint == NULL_VECTOR) {

                waitingForSkillMove = true;

                waitingForSkillChoice = false;
                waitingForAttack = false;
                unitMenu.SetActive(false);
                // TODO: Enable flag that using support skill for forecasting

                // Destroy move, attack, and ally highlighters only
                foreach (GameObject highlight in moveLocationHighlights) {
                    Destroy(highlight);
                }

                foreach (GameObject highlight in attackLocationHighlights) {
                    Destroy(highlight);
                }

                foreach (var highlight in allyLocationHighlights) {
                    Destroy(highlight);
                }

                moveLocations = new List<Vector2Int>();

                attackLocations = new List<Vector2Int>();

                itemToLocations = new Dictionary<ConsumableItem, HashSet<Vector2Int>>();

                // Create new support highlighters on surrounding attack positions based on skill range
                allyLocationHighlights = new List<GameObject>();

                currentSkillLocations = new List<Vector2Int>();

                var targetLocations = gameViewModel.GetSurroundingLocationsAtPoint(movedPoint, (selectedSkill as SingleTargetSkill).Range);

                currentSkillLocations = currentSkillLocations.Union(targetLocations);

                if ((selectedSkill as SingleTargetSkill).CanTargetSelf) {
                    var selfPosition = new List<Vector2Int>() {
                        movedPoint,
                    };
                    currentSkillLocations = currentSkillLocations.Union(selfPosition);
                }

                foreach (Vector2Int loc in currentSkillLocations) {
                    GameObject highlight;
                    var point = new Vector3Int(loc.x, loc.y, 0);
                    highlight = Instantiate(allyLocationPrefab, point, Quaternion.identity, gameObject.transform);
                    allyLocationHighlights.Add(highlight);
                }

                // Use only positions where a Skill is usable on a target
                currentSkillLocations = currentSkillLocations
                                            .Where(pos => 
                                                gameViewModel.SkillUsableOnTarget((selectedSkill as SingleTargetSkill), pos));

                if ((selectedSkill as SingleTargetSkill).CanTargetSelf) {
                    var selfPosition = new List<Vector2Int>() {
                        movedPoint,
                    };
                    currentSkillLocations = currentSkillLocations.Union(selfPosition);
                }

            } else {
                // Player has already chosen skill position, so use the Skill
                ApplySkillMove();
            }
        }

        /// <summary>
        /// Apply Skill Move to Game
        /// </summary>
        private async void ApplySkillMove() {
            // Move Unit first to move position before using Skill
            if (startPoint != movedPoint) {
                ApplyMoveUnit();
                await Task.Run(() => {
                    while (gameView.IsUpdating) { }
                });
            }

            Debug.Log("Use Skill");
            CurrentGameMove = new GameMove(movedPoint, skillPoint, selectedSkill);
            gameViewModel.ApplyMove(CurrentGameMove);
            ExitState();
        }

        /// <summary>
        /// Show Item Sub Menu
        /// </summary>
        private void ItemMenu() {

            unitMenu.CreateSubMenu();
            unitMenu.SwitchtoSubMenu();

            unitMenu.mBackButton.onClick.AddListener(() => {
                waitingForItemChoice = false;
            });

            waitingForMove = true;
            waitingForItemChoice = true;

            var availableItems = selectedUnit.Items
                                    .Where(it => it is ConsumableItem && (it is ISelfConsumable || it is ITargetConsumable))
                                    .Select(it => it as ConsumableItem);

            foreach (var item in availableItems)
            {
                var itemButton = unitMenu.CreateButton(item.ItemName.ToUpper());
                itemButton.onClick.AddListener(() =>
                {
                    selectedItem = item;
                    CheckItemOptions();
                });

                //If the current unit will not be able to consume the item (Full HP)...
                //Then it must be disabled to prevent misuse

                itemButton.interactable = false;

                if (item is ISelfConsumable)
                {
                    var consumableItem = item as ISelfConsumable;
                    if (consumableItem.CanUse(selectedUnit))
                    {
                        itemButton.interactable = true;
                    }
                }
                if(item is ITargetConsumable)
                {
                    var consumableItem = item as ITargetConsumable;
                    if (gameViewModel.ItemUsableOnTarget(item, itemPoint))
                    {
                        itemButton.interactable = true;
                    }
                }
                
                if (item is ITargetConsumable && itemPoint == NULL_VECTOR && itemToLocations.ContainsKey(item))
                {
                    var locs = itemToLocations[item];
                    if (locs.Any(pos => gameViewModel.ItemUsableOnTarget(item, pos)))
                    {
                        itemButton.interactable = true;
                    }
                }
            }
        }

        /// <summary>
        /// Apply Item Move to Game
        /// </summary>
        private async void ApplyItemMove() {
            if (startPoint != movedPoint)
            {
                ApplyMoveUnit();
                await Task.Run(() => {
                    while (gameView.IsUpdating) { }
                });
            }

            Debug.Log("Item");
            CurrentGameMove = new GameMove(movedPoint, itemPoint, selectedItem);
            gameViewModel.ApplyMove(CurrentGameMove);
            ExitState();
        }

        private void CheckItemOptions()
        {
            if (itemPoint == NULL_VECTOR)
            {
                waitingForAttack = false;
                waitingForItemMove = true;
                waitingForItemChoice = false;
                unitMenu.SetActive(false);

                foreach (GameObject highlight in moveLocationHighlights)
                {
                    Destroy(highlight);
                }

                foreach (GameObject highlight in attackLocationHighlights)
                {
                    Destroy(highlight);
                }

                foreach (var highlight in allyLocationHighlights)
                {
                    Destroy(highlight);
                }

                allyLocationHighlights = new List<GameObject>();

                moveLocations = new List<Vector2Int>();

                attackLocations = new List<Vector2Int>();

                skillsToLocations = new Dictionary<ActiveSkill, HashSet<Vector2Int>>();
                
                currentItemLocations = new List<Vector2Int>();

                if (selectedItem is ISelfConsumable) {
                    var selfPosition = new List<Vector2Int>() {
                        movedPoint,
                    };
                    currentItemLocations = currentItemLocations.Union(selfPosition);
                }

                if (selectedItem is ITargetConsumable) {
                    var targetPositions = gameViewModel
                        .GetSurroundingLocationsAtPoint(movedPoint, (selectedItem as ITargetConsumable).GetRange())
                        .ToList();
                    currentItemLocations = currentItemLocations.Union(targetPositions);
                }

                foreach (Vector2Int loc in currentItemLocations) {
                    GameObject highlight;
                    var point = new Vector3Int(loc.x, loc.y, 0);
                    highlight = Instantiate(allyLocationPrefab, point, Quaternion.identity, gameObject.transform);
                    allyLocationHighlights.Add(highlight);
                }

                if (selectedItem is ITargetConsumable) {
                    currentItemLocations = currentItemLocations
                                                .Where(pos => gameViewModel.ItemUsableOnTarget(selectedItem, pos));
                }

                if (selectedItem is ISelfConsumable) {
                    var selfPosition = new List<Vector2Int>() {
                        movedPoint,
                    };
                    currentItemLocations = currentItemLocations.Union(selfPosition);
                }

                var copy = currentItemLocations.ToList();

            } else {
                ApplyItemMove();
            }
        }

        private void CheckItemPositionClick(Vector2Int cursorPosition)
        {
            if (gameViewModel.AllyAtPoint(cursorPosition)) {
                if((!waitingForMove && !waitingForItemChoice && !waitingForItemMove) ||
                    (waitingForMove && itemPoint != NULL_VECTOR && itemPoint != cursorPosition))
                {
                    itemPoint = cursorPosition;

                    startPoint = gameViewModel.SelectedSquare.Position;
                    pathLocations = gameViewModel.GetShortestPathToItem(cursorPosition);
                    movedPoint = pathLocations.Last();

                    RefreshPathHighlighters();

                    if (selectedUnit.Items.Count > 0)
                    {
                        currentMenuOptions.Add(UnitMenuOptions.Items);
                    }
                    SetupMainUnitMenu();
                    WaitForChoice();
                } else {
                    if (waitingForItemChoice) {
                        ItemMenu();
                    }

                    if (currentItemLocations != null && currentItemLocations.Contains(cursorPosition)) {
                        if (waitingForItemMove) {
                            itemPoint = cursorPosition;
                            ApplyItemMove();
                        }
                    }
                }
            } else if (selectedItem != null && selectedItem is ISelfConsumable) {
                if (currentItemLocations != null && currentItemLocations.Contains(cursorPosition)) {
                    if (waitingForItemMove) {
                        itemPoint = cursorPosition;
                        ApplyItemMove();
                    }
                }
            }
        }
        /// <summary>
        /// Apply Wait Move to Game
        /// </summary>
        private async void ApplyWaitMove() {
            
            // Move Unit first to move position before Wait
            if (startPoint != movedPoint) {
                ApplyMoveUnit();
                await Task.Run(() => {
                    while (gameView.IsUpdating) { }
                });
            }

            Debug.Log("Wait");
            CurrentGameMove = new GameMove(movedPoint, movedPoint, GameMove.GameMoveType.Wait);
            gameViewModel.ApplyMove(CurrentGameMove);
            ExitState();
        }

        /// <summary>
        /// Exit Action State and return to Select State
        /// </summary>
        private void CancelMove() {
            // Maybe don't fully exit?
            ExitState();
            tileSelector.RefreshAllyHighlighters();
        }

        /// <summary>
        /// Exit Action State and Enter Select State
        /// </summary>
        private void ExitState() {
            this.enabled = false;

            combatForecast.ResetLabels();
            combatForecast.DisableCombatForecast();

            // Disable UI elements for Action State
            tileHighlight.SetActive(false);
            unitMenu.SetActive(false);

            DestroyHighlighters();

            // Reset viewModel Properties
            gameViewModel.CombatMode = false;
            gameViewModel.TargetSquare = null;

            // gameView.UnlockCamera();

            // Return to Select State
            tileSelector.EnterState();
            gameView.mEndTurnButton.interactable = true;
        }

        /// <summary>
        /// Refresh Path location highlighters according to path locations
        /// </summary>
        private void RefreshPathHighlighters() {
            foreach (var highlight in pathLocationHighlights) {
                Destroy(highlight);
            }

            pathLocationHighlights = new List<GameObject>();
            foreach (var loc in pathLocations) {
                GameObject highlight;
                highlight = Instantiate(tileSelectedPrefab, loc.ToVector3(), Quaternion.identity, gameObject.transform);
                pathLocationHighlights.Add(highlight);
            }
        }

        /// <summary>
        /// Destroy all highlighters
        /// </summary>
        private void DestroyHighlighters() {
            foreach (GameObject highlight in pathLocationHighlights) {
                Destroy(highlight);
            }

            foreach (GameObject highlight in moveLocationHighlights) {
                Destroy(highlight);
            }

            foreach (GameObject highlight in attackLocationHighlights) {
                Destroy(highlight);
            }

            foreach (var highlight in allyLocationHighlights) {
                Destroy(highlight);
            }
        }

        #endregion

        #endregion
    }
}
