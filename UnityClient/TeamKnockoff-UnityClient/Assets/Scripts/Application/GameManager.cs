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

        public TextAsset mapData;
        public GameType gameType;
        public bool localPlay;
        public int numberOfPlayers;

        public Player ControllingPlayer;

        public TileFactory tileFactory;
        public UnitFactory unitFactory;

        public int Rows { get; private set; }

        public int Columns { get; private set; }

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

            model.ConstructModel();

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

            model.AddPlayers(players);

            var newUnitViewModels = new Dictionary<Vector2Int, ObjectViewModel>();

            foreach(var tile in tileData.tileData) {
                var newTile = tileFactory.CreateTile(tile, view.transform);
                model.AddTile(newTile);

                if (tile.Player != 0) {
                    var newUnitObject = unitFactory.CreateUnit(tile, view.transform);
                    var newUnitModel = newUnitObject.GetComponent<Unit>();

                    var newUnitViewModel = new UnitViewModel(newUnitObject, newUnitModel);
                    newUnitViewModels.Add(new Vector2Int(tile.Column, tile.Row), newUnitViewModel);

                    model.AddUnit(newUnitModel, tile.Player, tile.Column, tile.Row);
                }
            }
            model.AddNeighbors();

            viewModel.ConstructViewModel(newUnitViewModels);

            view.ConstructView();

            model.StartGame();
            
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
                } else {
                    // Call and return MP Move
                }
            }
            throw new NotImplementedException();
        }

        private void Update() {

        }
    }
}
