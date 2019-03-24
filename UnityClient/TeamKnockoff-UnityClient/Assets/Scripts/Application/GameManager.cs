using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Tiles;
using Assets.Scripts.ViewModel;
using Assets.Scripts.View;
using Assets.Scripts.ComputerOpponent;

namespace Assets.Scripts.Application {
    public class GameManager: MonoBehaviour {

        public static GameManager instance;

        public enum GameType {
            Singleplayer,
            Multiplayer,
        }

        public GameModel model;
        public GameViewModel viewModel;
        public GameView view;
        public ComputerOpponent.ComputerOpponent cpu; 

        public TextAsset mapData;
        public GameType gameType;
        public bool localPlay;
        public int numberOfPlayers;

        public Player ControllingPlayer;

        public TileFactory tileFactory;
        public UnitFactory unitFactory;

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
            int cols = tileData.Columns;
            int rows = tileData.Rows;

            var players = new List<Player>();

            // Set up Players
            for (int player = 1; player <= numberOfPlayers; player++) {
                // model.AddPlayer($"Player {player}");
                Player newPlayer = new Player($"Player {player}");
                players.Add(newPlayer);
            }

            if (gameType == GameType.Singleplayer) {
                ControllingPlayer = players.First();
            }

            if (gameType == GameType.Multiplayer) {
                // TODO: Set up logic to find out which player the human in controlling
            }

            model.ConstructModel(cols, rows, players);

            var newObjectViews = new Dictionary<Vector2Int, ObjectView>();

            foreach(var tile in tileData.tileData) {
                var newTile = tileFactory.CreateTile(tile, view.transform);
                model.AddTile(newTile);

                if (tile.Player != 0) {
                    var newUnitObject = unitFactory.CreateUnit(tile, view.gameObject.transform);
                    var newUnitModel = newUnitObject.GetComponent<Unit>();

                    var newUnitView= new UnitView(newUnitObject);

                    newObjectViews.Add(new Vector2Int(tile.Column, tile.Row), newUnitView);

                    model.AddUnit(newUnitModel, tile.Player, tile.Column, tile.Row);
                }
            }
            model.AddNeighbors();

            viewModel.ConstructViewModel();

            view.ConstructView(cols, rows, newObjectViews);

            StartGame();
            
        }

        public void StartGame()
        {
            model.StartGame();
            viewModel.StartGame();

            // If first player is not the Controlling Player
            // Wait for other player's to make Move's first
            if (model.CurrentPlayer != ControllingPlayer) {
                viewModel.WaitForOtherMoves();
            }
        }

        public GameMove GetOtherPlayerMove() {
            // Need to use AI or MP calls here
            while (true) {
                if (gameType == GameType.Singleplayer) {
                    // Call and return AI Best Move
                    Debug.Log("Getting CPU Move");
                    return cpu.FindBestMove();
                }
                
                if (gameType == GameType.Multiplayer) {
                    // TODO: Call and return MP Move
                    
                }
            }
            throw new NotImplementedException();
        }

        private void Update() {

        }
    }
}
