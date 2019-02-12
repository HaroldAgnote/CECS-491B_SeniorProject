using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSelector : MonoBehaviour {
    public GameObject moveLocationPrefab;
    public GameObject tileHighlightPrefab;
    public GameObject attackLocationPrefab;

    private GameObject tileHighlight;
    private GameObject movingUnit;
    private List<Vector2Int> moveLocations;
    private List<GameObject> locationHighlights;

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
                
                ExitState();
            }
        } else {
            tileHighlight.SetActive(false);
        }
    }

    private void CancelMove() {
        this.enabled = false;

        foreach (GameObject highlight in locationHighlights) {
            Destroy(highlight);
        }

        TileSelector selector = GetComponent<TileSelector>();
        selector.EnterState();
    }

    public void EnterState(GameObject unit) {
        movingUnit = unit;
        this.enabled = true;

        moveLocations = GameManager.instance.MovesForUnit(movingUnit);
        locationHighlights = new List<GameObject>();

        if (moveLocations.Count == 0) {
            CancelMove();
        }

        foreach (Vector2Int loc in moveLocations) {
            GameObject highlight;
            var point = new Vector3Int(loc.x, loc.y, 0);
            highlight = Instantiate(moveLocationPrefab, point, Quaternion.identity, gameObject.transform);
            locationHighlights.Add(highlight);
        }
    }

    private void ExitState() {
        this.enabled = false;
        TileSelector selector = GetComponent<TileSelector>();
        tileHighlight.SetActive(false);
        movingUnit = null;
        selector.EnterState();
        foreach (GameObject highlight in locationHighlights) {
            Destroy(highlight);
        }
    }
}
