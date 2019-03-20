using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Skills;
using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class MoveSelector : MonoBehaviour {
        public static Vector2Int NULL_VECTOR = new Vector2Int(-1, -1);

        [SerializeField]
        private GameView gameView;

        [SerializeField]
        private TileSelector tileSelector;

        public Button attackButton;
        public Button skillsButton;
        public Button itemsButton;
        public Button waitButton;
        public Button cancelButton;

        public GameObject tileHighlightPrefab;
        public GameObject tileSelectedPrefab;
        public GameObject allyLocationPrefab;
        public GameObject moveLocationPrefab;
        public GameObject attackLocationPrefab;

        public GameObject unitMenu;

        private GameViewModel gameViewModel;

        private GameMove CurrentGameMove;

        private GameObject tileHighlight;
        private Unit movingUnit;

        private IEnumerable<Vector2Int> moveLocations;
        private IEnumerable<Vector2Int> pathLocations;
        private IEnumerable<Vector2Int> attackLocations;
        private IEnumerable<Vector2Int> skillLocations;
        private IEnumerable<Vector2Int> supportSkillLocations;
        private IEnumerable<Vector2Int> attackSkillLocations;

        private List<GameObject> allyLocationHighlights;
        private List<GameObject> moveLocationHighlights;
        private List<GameObject> attackLocationHighlights;
        private List<GameObject> pathLocationHighlights;

        private bool waitingForMove;
        private bool waitingForAttack;
        private bool waitingForSkill;

        private Vector2Int startPoint;
        private Vector2Int movedPoint;
        private Vector2Int skillPoint;
        private Vector2Int attackPoint;
        private Vector2Int supportPoint;

        public enum UnitMenuType {
            Attack_Skills_Items_Wait_Cancel,
            Attack_Skills_Wait_Cancel,
            Attack_Items_Wait_Cancel,
            Attack_Wait_Cancel,
            Skills_Items_Wait_Cancel,
            Skills_Wait_Cancel,
            Items_Wait_Cancel,
            Wait_Cancel
        }

        // Start is called before the first frame update
        void Start() {
            this.enabled = false;
            Vector3 point = new Vector3(0, 0, 0);
            tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
            tileHighlight.SetActive(false);
            unitMenu.SetActive(false);

            pathLocationHighlights = new List<GameObject>();
        }

        public void ConstructMoveSelector() {
            gameViewModel = gameView.gameViewModel;
            attackButton.onClick.AddListener(AttackMove);
            skillsButton.onClick.AddListener(SkillMove);
            itemsButton.onClick.AddListener(ItemMove);
            waitButton.onClick.AddListener(WaitUnit);
            cancelButton.onClick.AddListener(CancelMove);
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

                if (waitingForAttack) {
                    gameViewModel.HoveredSquare = gameViewModel.Squares
                            .SingleOrDefault(sq => sq.Position == point.ToVector2Int());
                }

                if (!waitingForMove) {
                    foreach (var highlight in pathLocationHighlights) {
                        Destroy(highlight);
                    }
                    if (moveLocations.Contains(point.ToVector2Int())) {
                        pathLocations = gameViewModel.GetShortestPath(point.ToVector2Int());
                        pathLocationHighlights = new List<GameObject>();
                        foreach (var loc in pathLocations) {
                            GameObject highlight;
                            highlight = Instantiate(tileSelectedPrefab, loc.ToVector3(), Quaternion.identity, gameObject.transform);
                            pathLocationHighlights.Add(highlight);
                        }
                    } else if (attackLocations.Contains(point.ToVector2Int())) {
                        pathLocations = gameViewModel.GetShortestPathToAttack(point.ToVector2Int());
                        pathLocationHighlights = new List<GameObject>();
                        foreach (var loc in pathLocations) {
                            GameObject highlight;
                            highlight = Instantiate(tileSelectedPrefab, loc.ToVector3(), Quaternion.identity, gameObject.transform);
                            pathLocationHighlights.Add(highlight);
                        }
                    }
                }

                if (Input.GetMouseButtonDown(0)) {
                    if (!EventSystem.current.IsPointerOverGameObject()) {
                        // TODO: Implement movement here
                        if (!moveLocations.Contains(point.ToVector2Int())
                            && !attackLocations.Contains(point.ToVector2Int())
                            && !attackSkillLocations.Contains(point.ToVector2Int())
                            && !supportSkillLocations.Contains(point.ToVector2Int())) {

                            CancelMove();
                            //ExitState();
                        } else if (moveLocations.Contains(point.ToVector2Int())) {
                            // TODO: Need to manage moving versus attacking
                            // GameManagerOrig.instance.Move(movingUnit, movedPoint);
                            //maybe make GameManagerOrig.instance.Attack();
                            //what should I pass in? That's my take on that.
                            //make Dmg Calc class that returns number
                            //within Attack() call in the Dmg Calc. Pass in Atkr and Defr
                            //reason for Dmg Calc class, this allows a preview (in FE).
                            if (!waitingForMove) {
                                movedPoint = point.ToVector2Int();
                                startPoint = gameViewModel.SelectedSquare.Position;

                                // TODO: Phantom Movement!

                                // TODO: Need to generate the correct menu based on unit and unit's position
                                if (gameViewModel.EnemyWithinRange(movedPoint, movingUnit.MainWeapon.Range)) {
                                    if (movingUnit.Skills.Count > 0
                                        && movingUnit.Skills
                                        .Select(sk => sk as SingleTargetSkill)
                                        .Any(sk => (sk is SingleSupportSkill && gameViewModel.AllyWithinRange(movedPoint, sk.Range))
                                        || (sk is SingleDamageSkill && gameViewModel.EnemyWithinRange(movedPoint, sk.Range)))) {

                                        if (movingUnit.Items.Count > 0) {
                                            SetupUnitMenu(UnitMenuType.Attack_Skills_Items_Wait_Cancel);
                                        } else {

                                            SetupUnitMenu(UnitMenuType.Attack_Skills_Wait_Cancel);
                                        }
                                    } else {
                                        if (movingUnit.Items.Count > 0) {

                                            SetupUnitMenu(UnitMenuType.Attack_Items_Wait_Cancel);
                                        } else {

                                            SetupUnitMenu(UnitMenuType.Attack_Wait_Cancel);
                                        }
                                    }

                                } else {
                                    if (movingUnit.Skills.Count > 0
                                        && movingUnit.Skills
                                        .Select(sk => sk as SingleTargetSkill)
                                        .Any(sk => (sk is SingleSupportSkill && gameViewModel.AllyWithinRange(movedPoint, sk.Range))
                                        || (sk is SingleDamageSkill && gameViewModel.EnemyWithinRange(movedPoint, sk.Range)))) {

                                        if (movingUnit.Items.Count > 0) {

                                            SetupUnitMenu(UnitMenuType.Skills_Items_Wait_Cancel);
                                        } else {

                                            SetupUnitMenu(UnitMenuType.Skills_Wait_Cancel);
                                        }
                                    } else {
                                        if (movingUnit.Items.Count > 0) {

                                            SetupUnitMenu(UnitMenuType.Items_Wait_Cancel);
                                        } else {

                                            SetupUnitMenu(UnitMenuType.Wait_Cancel);
                                        }
                                    }
                                }
                                foreach (GameObject highlight in pathLocationHighlights) {
                                    Destroy(highlight);
                                }
                                pathLocations = gameViewModel.GetShortestPath(point.ToVector2Int());
                                pathLocationHighlights = new List<GameObject>();
                                foreach (var loc in pathLocations) {
                                    GameObject highlight;
                                    highlight = Instantiate(tileSelectedPrefab, loc.ToVector3(), Quaternion.identity, gameObject.transform);
                                    pathLocationHighlights.Add(highlight);
                                }
                                WaitForChoice();
                            } else {
                                if (point.ToVector2Int() == movedPoint) {
                                    WaitUnit();
                                }
                            }

                        } else if (attackLocations.Contains(point.ToVector2Int())) { // Vector2Int attackPoint = Vector2Int.FloorToInt(point.ToVector2());
                            // Vector2Int movePoint = FindClosestAttackPoint(movingUnit, attackPoint);
                            // GameManagerOrig.instance.Move(movingUnit, movePoint);
                            // GameManagerOrig.instance.Attack(movingUnit, attackPoint);

                            if (gameViewModel.EnemyAtPoint(point.ToVector2Int())) {
                                attackPoint = point.ToVector2Int();
                                if (!waitingForMove) {
                                    // TODO: Calculate Move point here
                                    gameViewModel.CombatMode = true;

                                    startPoint = gameViewModel.SelectedSquare.Position;
                                    pathLocations = gameViewModel.GetShortestPathToAttack(point.ToVector2Int());
                                    movedPoint = pathLocations.Last();

                                    // TODO: Phantom movement to tile of MINIMUM valid
                                    // Attack Range to attack point

                                    if (movingUnit.Items.Count > 0) {

                                        SetupUnitMenu(UnitMenuType.Attack_Skills_Items_Wait_Cancel);
                                    } else {

                                        SetupUnitMenu(UnitMenuType.Attack_Skills_Wait_Cancel);
                                    }
                                    foreach (GameObject highlight in pathLocationHighlights) {
                                        Destroy(highlight);
                                    }
                                    pathLocationHighlights = new List<GameObject>();
                                    foreach (var loc in pathLocations) {
                                        GameObject highlight;
                                        highlight = Instantiate(tileSelectedPrefab, loc.ToVector3(), Quaternion.identity, gameObject.transform);
                                        pathLocationHighlights.Add(highlight);
                                    }
                                    WaitForChoice();
                                } else {
                                    if (!waitingForSkill && (waitingForAttack || point.ToVector2Int() == attackPoint)) {
                                        AttackUnit();
                                    } else if (waitingForSkill) {
                                        skillPoint = attackPoint;
                                        SkillUnit();
                                    }
                                }
                            }
                        } else if (attackSkillLocations.Contains(point.ToVector2Int())) {
                            if (gameViewModel.EnemyAtPoint(point.ToVector2Int())) {
                                attackPoint = point.ToVector2Int();

                                if (!waitingForMove) {
                                    // TODO: Calculate Move point here
                                    startPoint = gameViewModel.SelectedSquare.Position;
                                    pathLocations = gameViewModel.GetShortestPathToSkill(point.ToVector2Int());
                                    movedPoint = pathLocations.Last();

                                    // TODO: Phantom movement to tile of MINIMUM valid
                                    // Attack Range to attack point

                                    if (movingUnit.Items.Count > 0) {

                                        SetupUnitMenu(UnitMenuType.Attack_Skills_Items_Wait_Cancel);
                                    } else {

                                        SetupUnitMenu(UnitMenuType.Attack_Skills_Wait_Cancel);
                                    }
                                    foreach (GameObject highlight in pathLocationHighlights) {
                                        Destroy(highlight);
                                    }
                                    pathLocationHighlights = new List<GameObject>();
                                    foreach (var loc in pathLocations) {
                                        GameObject highlight;
                                        highlight = Instantiate(tileSelectedPrefab, loc.ToVector3(), Quaternion.identity, gameObject.transform);
                                        pathLocationHighlights.Add(highlight);
                                    }
                                    WaitForChoice();
                                } else {
                                    if (waitingForSkill) {
                                        skillPoint = attackPoint;
                                        SkillUnit();
                                    }
                                }
                            }
                        } else if (supportSkillLocations.Contains(point.ToVector2Int())) {
                            if (gameViewModel.AllyAPoint(point.ToVector2Int())) {
                                supportPoint = point.ToVector2Int();

                                if (!waitingForMove) {
                                    // TODO: Calculate Move point here
                                    startPoint = gameViewModel.SelectedSquare.Position;
                                    pathLocations = gameViewModel.GetShortestPathToSkill(point.ToVector2Int());
                                    movedPoint = pathLocations.Last();

                                    // TODO: Phantom movement to tile of MINIMUM valid
                                    // Attack Range to attack point

                                    if (movingUnit.Items.Count > 0) {

                                        SetupUnitMenu(UnitMenuType.Attack_Skills_Items_Wait_Cancel);
                                    } else {

                                        SetupUnitMenu(UnitMenuType.Attack_Skills_Wait_Cancel);
                                    }

                                    foreach (GameObject highlight in pathLocationHighlights) {
                                        Destroy(highlight);
                                    }
                                    pathLocationHighlights = new List<GameObject>();
                                    foreach (var loc in pathLocations) {
                                        GameObject highlight;
                                        highlight = Instantiate(tileSelectedPrefab, loc.ToVector3(), Quaternion.identity, gameObject.transform);
                                        pathLocationHighlights.Add(highlight);
                                    }
                                    WaitForChoice();
                                } else {
                                    if (waitingForSkill || point.ToVector2Int() == supportPoint) {
                                        skillPoint = supportPoint;
                                        SkillUnit();
                                    }
                                }
                            }
                        } else {
                            tileHighlight.SetActive(false);
                        }
                    }
                }
            }
        }

        private void SetupUnitMenu(UnitMenuType unitMenuType) {
            unitMenu.SetActive(true);
            
            attackButton.interactable = true;
            itemsButton.interactable = true;
            skillsButton.interactable = true;
            
            switch (unitMenuType) {
                case UnitMenuType.Attack_Items_Wait_Cancel:
                    skillsButton.interactable = false;
                    break;
                case UnitMenuType.Attack_Skills_Wait_Cancel:
                    itemsButton.interactable = false;
                    break;
                case UnitMenuType.Attack_Wait_Cancel:
                    skillsButton.interactable = false;
                    itemsButton.interactable = false;
                    break;
                case UnitMenuType.Items_Wait_Cancel:
                    attackButton.interactable = false;
                    skillsButton.interactable = false;
                    break;
                case UnitMenuType.Skills_Items_Wait_Cancel:
                    attackButton.interactable = false;
                    break;
                case UnitMenuType.Skills_Wait_Cancel:
                    attackButton.interactable = false;
                    itemsButton.interactable = false;
                    break;
                case UnitMenuType.Wait_Cancel:
                    attackButton.interactable = false;
                    itemsButton.interactable = false;
                    skillsButton.interactable = false;
                    break;
            }

        }

        private void MoveUnit() {
            Debug.Log("Move");
            CurrentGameMove = new GameMove(startPoint, movedPoint);
            gameViewModel.ApplyMove(CurrentGameMove);
        }

        private void ItemMove() {

        }

        private void SkillMove() {
            if (attackSkillLocations.Contains(movedPoint)) {
                DamageSkillMove();
            } else if (supportSkillLocations.Contains(movedPoint)) {
                SupportSkillMove();
            }
        }

        private void DamageSkillMove() {
            if (attackPoint == NULL_VECTOR) {
                //waitingForAttack = true;
                waitingForSkill = true;

                foreach (GameObject highlight in moveLocationHighlights)
                {
                    Destroy(highlight);
                }

                foreach (GameObject highlight in attackLocationHighlights)
                {
                    Destroy(highlight);
                }

                attackLocationHighlights = new List<GameObject>();

                var newAttackLocations = gameViewModel.GetSurroundingAttackLocationsAtPoint(movedPoint, (movingUnit.Skills.First() as SingleTargetSkill).Range);

                foreach (Vector2Int loc in newAttackLocations)
                {
                    GameObject highlight;
                    var point = new Vector3Int(loc.x, loc.y, 0);
                    highlight = Instantiate(attackLocationPrefab, point, Quaternion.identity, gameObject.transform);
                    attackLocationHighlights.Add(highlight);
                }
            }
            else {
                skillPoint = attackPoint;
                SkillUnit();
            }
        }

        private void SupportSkillMove() {
            if (supportPoint == NULL_VECTOR) {
                //waitingForAttack = true;
                waitingForSkill = true;
                foreach (GameObject highlight in moveLocationHighlights) {
                    Destroy(highlight);
                }

                foreach (GameObject highlight in allyLocationHighlights) {
                    Destroy(highlight);
                }

                allyLocationHighlights = new List<GameObject>();

                var newSupportLocations = gameViewModel.GetSurroundingAttackLocationsAtPoint(movedPoint, (movingUnit.Skills.First() as SingleTargetSkill).Range);

                foreach (Vector2Int loc in newSupportLocations) {
                    GameObject highlight;
                    var point = new Vector3Int(loc.x, loc.y, 0);
                    highlight = Instantiate(allyLocationPrefab, point, Quaternion.identity, gameObject.transform);
                    attackLocationHighlights.Add(highlight);
                }
            } else {
                skillPoint = supportPoint;
                SkillUnit();
            }
        }

        private void SkillUnit()
        {
            if (startPoint != movedPoint) {
                MoveUnit();
            }

            Debug.Log("Use Skill");
            CurrentGameMove = new GameMove(movedPoint, skillPoint, movingUnit.Skills.First());
            gameViewModel.ApplyMove(CurrentGameMove);
            ExitState();
        }

        private void WaitUnit() {
            if (startPoint != movedPoint) {
                MoveUnit();
            }

            Debug.Log("Wait");
            CurrentGameMove = new GameMove(movedPoint, movedPoint, GameMove.GameMoveType.Wait);
            gameViewModel.ApplyMove(CurrentGameMove);
            ExitState();
        }

        private void AttackMove() {
            if (attackPoint == NULL_VECTOR) {
                waitingForAttack = true;

                gameViewModel.CombatMode = true;

                foreach (GameObject highlight in moveLocationHighlights) {
                    Destroy(highlight);
                }

                foreach (GameObject highlight in attackLocationHighlights) {
                    Destroy(highlight);
                }

                attackLocationHighlights = new List<GameObject>();

                var newAttackLocations = gameViewModel.GetSurroundingAttackLocationsAtPoint(movedPoint, movingUnit.MainWeapon.Range);

                foreach (Vector2Int loc in newAttackLocations) {
                    GameObject highlight;
                    var point = new Vector3Int(loc.x, loc.y, 0);
                    highlight = Instantiate(attackLocationPrefab, point, Quaternion.identity, gameObject.transform);
                    attackLocationHighlights.Add(highlight);
                }
            } else {
                AttackUnit();
            }
        }

        private void AttackUnit() {
            if (startPoint != movedPoint) {
                MoveUnit();
            }

            Debug.Log("Attack");
            CurrentGameMove = new GameMove(movedPoint, attackPoint, GameMove.GameMoveType.Attack);
            gameViewModel.ApplyMove(CurrentGameMove);
            ExitState();
        }

        private void CancelMove() {
            this.enabled = false;
            tileHighlight.SetActive(false);

            unitMenu.SetActive(false);

            waitingForMove = false;
            waitingForAttack = false;
            waitingForSkill = false;
            startPoint = NULL_VECTOR;
            movedPoint = NULL_VECTOR;
            attackPoint = NULL_VECTOR;

            gameView.UnlockCamera();

            DestroyHighlighters();

            tileSelector.EnterState();
        }

        private void WaitForChoice() {
            gameView.LockCamera();
            waitingForMove = true;
        } 

        public void EnterState(Unit unit) {
            movingUnit = unit;
            this.enabled = true;
            moveLocations = gameViewModel.MovesForUnit;

            moveLocationHighlights = new List<GameObject>();

            waitingForMove = false;
            waitingForAttack = false;
            startPoint = NULL_VECTOR;
            movedPoint = NULL_VECTOR;
            skillPoint = NULL_VECTOR;
            attackPoint = NULL_VECTOR;
            supportPoint = NULL_VECTOR;

            foreach (Vector2Int loc in moveLocations) {
                GameObject highlight;
                var point = new Vector3Int(loc.x, loc.y, 0);
                highlight = Instantiate(moveLocationPrefab, point, Quaternion.identity, gameObject.transform);
                moveLocationHighlights.Add(highlight);
            }

            // TODO: Need to get attackLocations and highlight them
            attackLocations = gameViewModel.AttacksForUnit;
            skillLocations = gameViewModel.SkillsForUnit;
            supportSkillLocations = gameViewModel.SupportSkillsForUnit;
            attackSkillLocations = gameViewModel.DamageSkillsForUnit;

            attackLocationHighlights = new List<GameObject>();

            // TODO: Don't filter out locations where there is a unit
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

            allyLocationHighlights = new List<GameObject>();

            foreach (var playerUnit in gameViewModel.ControllingPlayer.Units.Where(un => un.IsAlive)) {
                GameObject highlight;
                var allyLoc = gameViewModel.GetPositionOfUnit(playerUnit);
                highlight = Instantiate(allyLocationPrefab, allyLoc.ToVector3(), Quaternion.identity, gameObject.transform);
                allyLocationHighlights.Add(highlight);
            }
        }

        public void ExitState() {
            this.enabled = false;
            tileHighlight.SetActive(false);
            movingUnit = null;

            unitMenu.SetActive(false);

            waitingForMove = false;
            waitingForAttack = false;
            waitingForSkill = false;

            gameViewModel.CombatMode = false;

            startPoint = NULL_VECTOR;
            movedPoint = NULL_VECTOR;
            skillPoint = NULL_VECTOR;
            attackPoint = NULL_VECTOR;
            supportPoint = NULL_VECTOR;

            DestroyHighlighters();

            gameView.UnlockCamera();

            tileSelector.EnterState();
        }

        private void DestroyHighlighters() {
            // Destroy all highlighters
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
    }
}
