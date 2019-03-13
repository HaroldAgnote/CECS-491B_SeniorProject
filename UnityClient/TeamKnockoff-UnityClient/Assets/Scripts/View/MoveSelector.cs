using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.ExtensionMethods;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;
using Assets.Scripts.ViewModel;

namespace Assets.Scripts.View {
    public class MoveSelector : MonoBehaviour
    {
        [SerializeField]
        private GameView gameView;

        [SerializeField]
        private TileSelector tileSelector;


        public GameObject moveLocationPrefab;
        public GameObject tileHighlightPrefab;
        public GameObject attackLocationPrefab;

        private GameViewModel gameViewModel;

        private GameObject tileHighlight;
        private Unit movingUnit;
        private List<Vector2Int> moveLocations;
        private List<Vector2Int> attackLocations;
        private List<GameObject> moveLocationHighlights;
        private List<GameObject> attackLocationHighlights;

        // Start is called before the first frame update
        void Start() {
            this.enabled = false;
            gameViewModel = gameView.gameViewModel;
            Vector3 point = new Vector3(0, 0, 0);
            tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
            tileHighlight.SetActive(false);
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
                        Vector2Int movedPoint = point.ToVector2Int();

                        Vector2Int startPoint = gameViewModel.SelectedSquare.Position;
                        GameMove move = new GameMove(startPoint, movedPoint);

                        gameViewModel.ApplyMove(move);

                        // TODO: Need to manage moving versus attacking
                        // GameManagerOrig.instance.Move(movingUnit, movedPoint);
                        //maybe make GameManagerOrig.instance.Attack();
                        //what should I pass in? That's my take on that.
                        //make Dmg Calc class that returns number
                        //within Attack() call in the Dmg Calc. Pass in Atkr and Defr
                        //reason for Dmg Calc class, this allows a preview (in FE).
                        ExitState();
                    } else if (attackLocations.Contains(point.ToVector2Int())) {
                        // Vector2Int attackPoint = Vector2Int.FloorToInt(point.ToVector2());
                        // Vector2Int movePoint = FindClosestAttackPoint(movingUnit, attackPoint);
                        // GameManagerOrig.instance.Move(movingUnit, movePoint);
                        // GameManagerOrig.instance.Attack(movingUnit, attackPoint);

                        ExitState();
                    }
                }
            } else {
                tileHighlight.SetActive(false);
            }
        }

        private void CancelMove() {
            this.enabled = false;
            tileHighlight.SetActive(false);

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

        public void EnterState(Unit unit) {
            movingUnit = unit;
            this.enabled = true;
            moveLocations = gameViewModel.MovesForUnit;

            moveLocationHighlights = new List<GameObject>();

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
