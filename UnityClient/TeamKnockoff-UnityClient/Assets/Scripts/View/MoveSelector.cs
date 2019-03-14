using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;
using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class MoveSelector : MonoBehaviour {
        public static Vector2Int NULL_VECTOR = new Vector2Int(-1, -1);

        [SerializeField]
        private GameView gameView;

        [SerializeField]
        private TileSelector tileSelector;

        public GameObject unitMenu_Attack_Skills_Items_Wait_Cancel_prefab;

        public GameObject moveLocationPrefab;
        public GameObject tileHighlightPrefab;
        public GameObject attackLocationPrefab;

        private GameViewModel gameViewModel;

        private GameMove CurrentGameMove;

        private GameObject unitMenu;
        private GameObject tileHighlight;
        private Unit movingUnit;
        private List<Vector2Int> moveLocations;
        private List<Vector2Int> attackLocations;
        private List<GameObject> moveLocationHighlights;
        private List<GameObject> attackLocationHighlights;

        private bool waitingForMove;
        private bool waitingForAttack;

        private Vector2Int startPoint;
        private Vector2Int movedPoint;
        private Vector2Int attackPoint;

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

        public static Dictionary<UnitMenuType, GameObject> unitMenus;

        // Start is called before the first frame update
        void Start() {
            this.enabled = false;
            gameViewModel = gameView.gameViewModel;
            Vector3 point = new Vector3(0, 0, 0);
            tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
            tileHighlight.SetActive(false);

            unitMenus = new Dictionary<UnitMenuType, GameObject>() {
                { UnitMenuType.Attack_Skills_Items_Wait_Cancel, unitMenu_Attack_Skills_Items_Wait_Cancel_prefab },
            };
        }

        // Update is called once per frame
        void Update()
        {
            //Converting Mouse Pos to 2D (vector2) World Pos
            Vector2 rayPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0f);

            if (hit) {
                Vector3 point = hit.collider.gameObject.transform.position;
                // Debug.Log($"Hovering at Point: ({point.x}, {point.y})");


                tileHighlight.SetActive(true);
                tileHighlight.transform.position = point;
                if (Input.GetMouseButtonDown(0)) {
                    // TODO: Implement movement here
                    if (!moveLocations.Contains(point.ToVector2Int()) && 
                        !attackLocations.Contains(point.ToVector2Int())) {
                        CancelMove();
                        //ExitState();
                    }

                    else if (moveLocations.Contains(point.ToVector2Int())) {
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
                            SetupUnitMenu(point, UnitMenuType.Attack_Skills_Items_Wait_Cancel);
                            WaitForChoice();
                        }
                        
                    } else if (attackLocations.Contains(point.ToVector2Int())) {
                        // Vector2Int attackPoint = Vector2Int.FloorToInt(point.ToVector2());
                        // Vector2Int movePoint = FindClosestAttackPoint(movingUnit, attackPoint);
                        // GameManagerOrig.instance.Move(movingUnit, movePoint);
                        // GameManagerOrig.instance.Attack(movingUnit, attackPoint);

                        attackPoint = point.ToVector2Int();

                        if (!waitingForMove) {
                            // TODO: Calculate Move point here

                            // TODO: Phantom movement to tile of MINIMUM valid
                            // Attack Range to attack point

                            SetupUnitMenu(point, UnitMenuType.Attack_Skills_Items_Wait_Cancel);
                            WaitForChoice();
                        }

                        if (waitingForAttack) {
                            AttackUnit();
                        }
                    }
                }
            } else {
                tileHighlight.SetActive(false);
            }
        }

        private void SetupUnitMenu(Vector3 point, UnitMenuType unitMenuType) {
            unitMenu = Instantiate(unitMenus[unitMenuType], point, Quaternion.identity, gameObject.transform);

            // Dirty solution in finding the canvas
            var canvas = GameObject.Find("UnitMenuCanvas");

            var buttons = canvas.GetComponentsInChildren<Button>();
            
            // Dirty solution in hooking button names to listeners
            foreach (var button in buttons) {
                switch (button.name) {
                    case "AttackButton":
                        button.onClick.AddListener(AttackMove);
                        break;
                    case "SkillsButton":
                        button.onClick.AddListener(SkillMove);
                        break;
                    case "ItemsButton":
                        button.onClick.AddListener(ItemMove);
                        break;
                    case "WaitButton":
                        button.onClick.AddListener(WaitUnit);
                        break;
                    case "CancelButton":
                        button.onClick.AddListener(CancelMove);
                        break;
                }
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

                foreach (GameObject highlight in moveLocationHighlights) {
                    Destroy(highlight);
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

            Destroy(unitMenu);

            waitingForMove = false;
            waitingForAttack = false;
            startPoint = NULL_VECTOR;
            movedPoint = NULL_VECTOR;
            attackPoint = NULL_VECTOR;

            gameView.UnlockCamera();

            foreach (GameObject highlight in moveLocationHighlights)
            {
                Destroy(highlight);
            }

            foreach (GameObject highlight in attackLocationHighlights)
            {
                Destroy(highlight);
            }
            tileSelector.EnterState();
        }

        private void WaitForChoice() {
            gameView.LockCamera();
            tileHighlight.SetActive(false);
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
            attackPoint = NULL_VECTOR;

            foreach (Vector2Int loc in moveLocations) {
                GameObject highlight;
                var point = new Vector3Int(loc.x, loc.y, 0);
                highlight = Instantiate(moveLocationPrefab, point, Quaternion.identity, gameObject.transform);
                moveLocationHighlights.Add(highlight);
            }

            // TODO: Need to get attackLocations and highlight them
            attackLocations = gameViewModel.AttacksForUnit;
            attackLocationHighlights = new List<GameObject>();

            // TODO: Don't filter out locations where there is a unit
            var filteredAttackLocations = attackLocations.Where(x => !moveLocations.Contains(x));

            foreach (Vector2Int loc in filteredAttackLocations) {
                GameObject highlight;
                var point = new Vector3Int(loc.x, loc.y, 0);
                highlight = Instantiate(attackLocationPrefab, point, Quaternion.identity, gameObject.transform);
                attackLocationHighlights.Add(highlight);
            }
        }

        public void ExitState() {
            this.enabled = false;
            tileHighlight.SetActive(false);
            movingUnit = null;

            Destroy(unitMenu);

            waitingForMove = false;
            waitingForAttack = false;
            startPoint = NULL_VECTOR;
            movedPoint = NULL_VECTOR;
            attackPoint = NULL_VECTOR;

            gameView.UnlockCamera();

            // Destroy all highlighters
            foreach (GameObject highlight in moveLocationHighlights) {
                Destroy(highlight);
            }

            foreach (GameObject highlight in attackLocationHighlights) {
                Destroy(highlight);
            }

            tileSelector.EnterState();
        }
    }
}
