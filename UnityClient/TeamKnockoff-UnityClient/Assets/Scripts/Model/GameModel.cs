using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Units;

public class GameModel : MonoBehaviour {

    #region Member fields

    private Unit[,] mUnits;
    private Tile[,] mTiles;

    private int mTurns;

    private List<Player> mPlayers;

    private Player mCurrentPlayer;

    #endregion


    #region Properties

    #endregion

    private void Start() {
        int columns = GameManager.instance.Columns;
        int rows = GameManager.instance.Rows;

        mUnits = new Unit[columns, rows];
        mTiles = new Tile[columns, rows];

        // Start turns
        mTurns = 1;
    }

    public void AddPlayer(string playerName) {
        mPlayers.Add(new Player(playerName));
    }

    public void AddUnit(Unit unit) {

    }

    public void AddTile(Tile tile) {

    }
}
