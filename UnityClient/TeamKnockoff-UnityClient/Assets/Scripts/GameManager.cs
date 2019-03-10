using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Units;
using UnityEngine;
using Assets.Scripts;

public class GameManager: MonoBehaviour {

    public static GameManager instance;

    public enum GameType {
        Singleplayer,
        Multiplayer,
    }

    public GameModel model;
    public GameViewModel viewModel;
    public GameView view;

    public TextAsset mapData;
    public GameType gameType;
    public int numberOfPlayers;

    public int Rows {
        get;
        private set;
    }

    public int Columns {
        get;
        private set;
    }

    private void Awake() {
        //Check if instance already exists
        if (instance == null) {
            //if not, set instance to this
            instance = this;
        }

        //If instance already exists and it's not this:
        else if (instance != this) {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManagerOrig.
            Destroy(gameObject);
        }
    }

    private void Start() {
        var tileData = JsonUtility.FromJson<TileDataWrapper>(mapData.text);
        Rows = tileData.Rows;
        Columns = tileData.Columns;

        // Set up Players
        for (int player = 1; player <= numberOfPlayers; player++) {
            model.AddPlayer($"Player {player}");
        }

        foreach(var tile in tileData.tileData) {

        }
    }
}
