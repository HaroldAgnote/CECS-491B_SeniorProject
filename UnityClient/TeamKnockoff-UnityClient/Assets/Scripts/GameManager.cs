using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BoardManager boardScript;

    public GameObject sampleUnit;

    public GameObject[,] units;
    public Tile[,] tiles;

    private void Awake() {
        //Check if instance already exists
        if (instance == null) {
            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:
        else if (instance != this) {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        units = new GameObject[boardScript.columns, boardScript.rows];
        tiles = new Tile[boardScript.columns, boardScript.rows];
        InitialSetup();
    }

    void InitialSetup() {
        AddUnit(sampleUnit, 10, 10);
    }

    void AddUnit(GameObject unitPrefab, int col, int row) {
        GameObject newUnit = boardScript.AddUnit(unitPrefab, col, row);
        units[col, row] = newUnit;
    }

    public void AddTile(Tile tile) {
        tiles[tile.XPosition, tile.YPosition] = tile;
    }

    public List<Vector2Int> MovesForUnit(GameObject unitObject) {
        Unit unit = unitObject.GetComponent<Unit>();
        var gridPoint = GridForUnit(unitObject);
        var moveLocations = unit.GetMoveLocations(gridPoint);
        return moveLocations;
    }

    public Vector2Int GridForUnit(GameObject unit) {
        for (int col = 0; col < boardScript.columns; col++) {
            for (int row = 0; row < boardScript.rows; row++) {
                if (units[col, row] == unit) {
                    return new Vector2Int(col, row);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }

    public GameObject UnitAtGrid(Vector3 gridpoint) {
        try {
            return units[(int) gridpoint.x, (int) gridpoint.y];
        } catch {
            return null;
        }
    }

    public void Move(GameObject unit, Vector2Int gridPoint) {
        Unit unitComponent = unit.GetComponent<Unit>();

        Vector2Int startGridPoint = GridForUnit(unit);
        units[startGridPoint.x, startGridPoint.y] = null;
        units[gridPoint.x, gridPoint.y] = unit;
        boardScript.MoveUnit(unit, gridPoint);
    }


    // Update is called once per frame
    void Update() {
    }
}
