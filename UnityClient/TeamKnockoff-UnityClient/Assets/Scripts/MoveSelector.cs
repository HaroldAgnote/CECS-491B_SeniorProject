using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Units;
using Assets.Scripts.ExtensionMethods;

public class MoveSelector : MonoBehaviour {
    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingUnit;
    private List<Vector2Int> moveLocations;
    private List<Vector2Int> attackLocations;
    private List<GameObject> moveLocationHighlights;
    private List<GameObject> attackLocationHighlights;

    void Start() {
        this.enabled = false;
        Vector3 point = new Vector3(0, 0, 0);
        tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);
        tileHighlight.SetActive(false);
    }

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
                // TODO: Implement movement here
                if (!moveLocations.Contains(point.ToVector2Int()) && 
                    !attackLocations.Contains(point.ToVector2Int())) {
                    CancelMove();
                    //ExitState();
                }

                else if (moveLocations.Contains(point.ToVector2Int())) {
                    Vector2Int movedPoint = point.ToVector2Int();

                    // TODO: Need to manage moving versus attacking
                    GameManager.instance.Move(movingUnit, movedPoint);
                    //maybe make GameManager.instance.Attack();
                    //what should I pass in? That's my take on that.
                    //make Dmg Calc class that returns number
                    //within Attack() call in the Dmg Calc. Pass in Atkr and Defr
                    //reason for Dmg Calc class, this allows a preview (in FE).
                    ExitState();
                }

                else if (attackLocations.Contains(point.ToVector2Int())) {
                    Vector2Int attackPoint = Vector2Int.FloorToInt(point.ToVector2());
                    Vector2Int movePoint = FindClosestAttackPoint(movingUnit, attackPoint);
                    GameManager.instance.Move(movingUnit, movePoint);
                    GameManager.instance.Attack(movingUnit, attackPoint);
                    ExitState();
                }

            }
        } else {
            tileHighlight.SetActive(false);
        }
    }

    private Vector2Int FindClosestAttackPoint(GameObject attackingUnit, Vector2Int attackPoint)
    {
        var unit = attackingUnit.GetComponent<Unit>();
        var unitPosition = GameManager.instance.GridForUnit(attackingUnit);

        Debug.Log($"Attack Point: ({attackPoint.x}, {attackPoint.y}");
        var closestPoints = new List<Vector2Int>();
        
        closestPoints.Add(new Vector2Int(attackPoint.x - unit.MainWeapon.Range, attackPoint.y));
        closestPoints.Add(new Vector2Int(attackPoint.x, attackPoint.y - unit.MainWeapon.Range));

        closestPoints.Add(new Vector2Int(attackPoint.x + unit.MainWeapon.Range, attackPoint.y));
        closestPoints.Add(new Vector2Int(attackPoint.x, attackPoint.y + unit.MainWeapon.Range));

        Debug.Log("Closest Points");
        foreach (var pos in closestPoints) {
            Debug.Log($"({pos.x}, {pos.y})");
        }

        var possiblePoints = closestPoints.Where(point =>
            ((point == unitPosition) ||
            (GameManager.instance.UnitAtGrid(new Vector3(point.x, point.y, 0f)) == null)) &&
            (moveLocations.Contains(point))).ToList();

        Debug.Log("Possible attack positions: ");
        foreach (var pos in possiblePoints) {
            Debug.Log($"({pos.x}, {pos.y})");
        }


        if (possiblePoints.Count > 1) {
            return possiblePoints.First();
        } else {
            return possiblePoints.SingleOrDefault();
        }

    } 

    private void CancelMove() {
        this.enabled = false;

        foreach (GameObject highlight in moveLocationHighlights)
        {
            Destroy(highlight);
        }

        foreach (GameObject highlight in attackLocationHighlights)
        {
            Destroy(highlight);
        }

        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
    }

    public void EnterState(GameObject unit) {
        movingUnit = unit;
        this.enabled = true;

        moveLocations = GameManager.instance.MovesForUnit(movingUnit);
        moveLocationHighlights = new List<GameObject>();

        foreach (Vector2Int loc in moveLocations) {
            GameObject highlight;
            var point = new Vector3Int(loc.x, loc.y, 0);
            highlight = Instantiate(moveLocationPrefab, point, Quaternion.identity, gameObject.transform);
            moveLocationHighlights.Add(highlight);
        }

        // TODO: Need to get attackLocations and highlight them
        attackLocations = GameManager.instance.AttacksForUnit(movingUnit);
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

    private void ExitState() {
        this.enabled = false;
        TileSelector selector = GetComponent<TileSelector>();
        tileHighlight.SetActive(false);
        movingUnit = null;

        // Check if Players have active units
        GameManager.instance.CheckGameState();

        // If Player has no more moves, change turns
        if (GameManager.instance.CheckIfCurrentPlayerHasNoMoves()) {
            GameManager.instance.NextPlayer();
        }
        selector.EnterState();

        // Destroy all highlighters
        foreach (GameObject highlight in moveLocationHighlights) {
            Destroy(highlight);
        }

        foreach (GameObject highlight in attackLocationHighlights) {
            Destroy(highlight);
        }
    }
}
