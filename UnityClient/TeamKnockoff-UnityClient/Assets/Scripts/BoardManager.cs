using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject floor;
    public int rows = 8;
    public int columns = 8;

    [HideInInspector]
    List<Vector3> boardPositions;

    private Transform boardHolder;

    private void Awake() {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = gameObject.transform;
        boardHolder.position.Set(0, 0, 0);
        boardPositions = new List<Vector3>();

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = 0; x < columns; x++) {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = 0; y < rows; y++) {
                GameObject toInstantiate = floor;

                Vector3 newPos = new Vector3(x, y, 0f);
                boardPositions.Add(newPos);

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance = Instantiate(floor, newPos, Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(boardHolder);

            }
        }
    }

    public GameObject AddUnit(GameObject unitPrefab, int col, int row) {
        Vector3 gridPoint = new Vector3(col, row, 0f);
        GameObject newUnit = Instantiate(unitPrefab, gridPoint, Quaternion.identity) as GameObject;
        return newUnit;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
