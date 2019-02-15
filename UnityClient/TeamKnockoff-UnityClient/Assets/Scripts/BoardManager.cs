using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject floor;
    public GameObject wall;
    public GameObject swamp;

    public int rows = 8;
    public int columns = 8;

    [HideInInspector]
    List<Vector3> boardPositions;

    private Transform boardHolder;

    // Start is called before the first frame update
    private void Start() {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = gameObject.transform;
        boardHolder.position.Set(0, 0, 0);
        boardPositions = new List<Vector3>();

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = 0; x < columns; x++) {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = 0; y < rows; y++) {
                GameObject toInstantiate = floor;

                // TODO: Need to instantiate correct Tile
                if (x != 17 && (x >= 15 && x <= 20) && ( y >= 30 && y <= 40)) {
                    var newTile = new Tile(x, y, Tile.BoardTileType.Boundary);

                    GameManager.instance.AddTile(newTile);

                    Vector3 newPos = new Vector3(x, y, 0f);

                    boardPositions.Add(newPos);

                    //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                    GameObject instance = Instantiate(wall, newPos, Quaternion.identity) as GameObject;

                    //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                    instance.transform.SetParent(boardHolder);
                } else if (x > 10 && x < 20 && y > 10 && y < 20) {
                    var newTile = new Tile(x, y, 3, Tile.BoardTileType.Normal);

                    GameManager.instance.AddTile(newTile);

                    Vector3 newPos = new Vector3(x, y, 0f);

                    boardPositions.Add(newPos);

                    //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                    GameObject instance = Instantiate(swamp, newPos, Quaternion.identity) as GameObject;

                    //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                    instance.transform.SetParent(boardHolder);
                } else {
                    var newTile = new Tile(x, y);

                    GameManager.instance.AddTile(newTile);

                    Vector3 newPos = new Vector3(x, y, 0f);

                    boardPositions.Add(newPos);

                    //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                    GameObject instance = Instantiate(floor, newPos, Quaternion.identity) as GameObject;

                    //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                    instance.transform.SetParent(boardHolder);
                }
            }
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
