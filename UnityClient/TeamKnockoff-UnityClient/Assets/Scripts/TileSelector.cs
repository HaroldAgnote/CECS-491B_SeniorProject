using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public GameObject tileHighlightPrefab;

    private GameObject tileHighlight;

    void Start() {
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
                GameObject selectedPiece = GameManager.instance.UnitAtGrid(point);
                // Reference Point 1: add ExitState call here later
                ExitState(selectedPiece);
            }
        } else {
            tileHighlight.SetActive(false);
        }
    }

    public void EnterState() {
        enabled = true;
    }

    private void ExitState(GameObject movingUnit) {
        this.enabled = false;
        tileHighlight.SetActive(false);
        MoveSelector move = GetComponent<MoveSelector>();
        move.EnterState(movingUnit);
    }
}
