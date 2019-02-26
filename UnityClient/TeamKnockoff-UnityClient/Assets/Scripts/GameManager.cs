using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Units;
using UnityEngine;
using Assets.Scripts;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public BoardManager boardScript;

    public GameObject[,] units;
    public Tile[,] tiles;

    public Player playerOne;
    public Player playerTwo;

    Player currentPlayer;
    Player otherPlayer;

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
        // Initialize unit and tile 2D Arrays
        units = new GameObject[boardScript.columns, boardScript.rows];
        tiles = new Tile[boardScript.columns, boardScript.rows];
        
        // Initialize Players
        playerOne = new Player("Player One");
        playerTwo = new Player("Player Two");

        // Set Current Player to Player One
        currentPlayer = playerOne;
        otherPlayer = playerTwo;

        InitialSetup();
    }

    void InitialSetup() {
        // TODO: Figure out how to import Units and add them using the inspector
        //       rather than hard coding them in

        // Start Current Player's Turn
        currentPlayer.StartTurn();
    }

    public void AddUnit(GameObject newUnit, Player player, int col, int row) {
        player.AddUnit(newUnit);
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

    public List<Vector2Int> AttacksForUnit(GameObject unitObject) {
        Unit unit = unitObject.GetComponent<Unit>();
        var gridPoint = GridForUnit(unitObject);
        var attackLocations = unit.GetAttackLocations(gridPoint);
        return attackLocations;
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
        currentPlayer.MarkUnitAsMoved(unit);
    }

    public bool UnitHasMoved(GameObject unit) {
        return currentPlayer.CheckUnitHasMoved(unit);
    }

    public bool DoesUnitBelongToCurrentPlayer(GameObject unit) {
        return currentPlayer.units.Contains(unit);
    }

    public bool CheckIfCurrentPlayerHasNoMoves() {
        return !currentPlayer.hasMoved.Contains(false);
    }

    public void NextPlayer() {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;

        currentPlayer.StartTurn();
    }

    public void Attack(GameObject unit, Vector2Int gridPoint)
    {
        Unit attacker = unit.GetComponent<Unit>();
        Unit defender = UnitAtGrid(new Vector3(gridPoint.x, gridPoint.y, 0f)).GetComponent<Unit>();
        Debug.Log("atk.Weapon: " + attacker.MainWeapon.Might);
        Debug.Log("attacker Str: " + attacker.Strength);
        Debug.Log("Attacker HP: " + attacker.HealthPoints);
        Debug.Log("Defender Def: " + defender.Defense);
        defender.HealthPoints -= DamageCalculator.GetDamage(attacker, defender);
        currentPlayer.MarkUnitAsMoved(unit);
        Debug.Log("Defender HP: " + defender.HealthPoints);

        if(defender.HealthPoints <= 0)
        {
            //the unit is still kinda there, but not really.
            //maybe obliterate the tile
            Destroy(defender.gameObject);
            Destroy(defender);
        }
        //print more stuff to make sure
        //destroy unit when dead
    }
}
