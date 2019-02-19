using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public int columns;
    public int rows;

    public TileFactory tileFactory;

    public TextAsset mapData;

    private Transform boardHolder;

    // Start is called before the first frame update
    private void Start() {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = gameObject.transform;
        boardHolder.position.Set(0, 0, 0);

        var tileData = JsonUtility.FromJson<TileDataWrapper>(mapData.text);

        columns = tileData.Columns;
        rows = tileData.Rows;

        foreach(var tile in tileData.tileData) {
            var newTile = tileFactory.CreateTile(tile, boardHolder);
            GameManager.instance.AddTile(newTile);
        }

        // After all tiles have been created, go through entire board to set neighbors
        for (int x = 0; x < columns; x++) {
            for (int y = 0; y < rows; y++) {
                var currentTile = GameManager.instance.tiles[x, y];
                AddNeighbors(currentTile, x, y);
            }
        }
    }

    public void AddNeighbors(Tile newTile, int x, int y) {
        try {
            GameManager.instance.tiles[x - 1, y].Neighbors.Add(newTile);
        } catch { }
        try {
            GameManager.instance.tiles[x + 1, y].Neighbors.Add(newTile);
        } catch { }

        try {
            GameManager.instance.tiles[x, y - 1].Neighbors.Add(newTile);
        } catch { }
        try {
            GameManager.instance.tiles[x, y + 1].Neighbors.Add(newTile);
        } catch { }
    }

    public GameObject AddUnit(GameObject unitPrefab, int col, int row) {
        Vector3 gridPoint = new Vector3(col, row, 0f);
        GameObject newUnit = Instantiate(unitPrefab, gridPoint, Quaternion.identity) as GameObject;
        return newUnit;
    }

    public void MoveUnit(GameObject unit, Vector2Int gridPoint) {
        var newPos = new Vector3(gridPoint.x, gridPoint.y, 0f);

        // TODO: Animate smoother movement
        unit.transform.position = newPos;
    }
}
