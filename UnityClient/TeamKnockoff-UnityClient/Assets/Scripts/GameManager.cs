using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BoardManager boardScript;

    public GameObject sampleUnit;

    public GameObject[,] units;

    private void Awake() {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        units = new GameObject[boardScript.rows, boardScript.columns];
        InitialSetup();
    }

    void InitialSetup() {
        AddUnit(sampleUnit, 0, 0);
    }

    void AddUnit(GameObject unitPrefab, int col, int row) {
        GameObject newUnit = boardScript.AddUnit(unitPrefab, col, row);
        units[col, row] = newUnit;
    }

    // Update is called once per frame
    void Update() {
    }
}
