using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

using Assets.Scripts.Campaign;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Tiles;
using Assets.Scripts.ViewModel;
using Assets.Scripts.View;

namespace Assets.Scripts.Application {

    /// <summary>
    /// Initializes the Game Scene when a Player starts a Game
    /// </summary>
    public class GameManager: MonoBehaviour {

        public const string SINGLEPLAYER_GAME_TYPE = "SinglePlayer";
        public const string MULTIPLAYER_GAME_TYPE = "MultiPlayer";
        public const string CAMPAIGN_GAME_TYPE = "Campaign";
        public const string PRACTICE_GAME_TYPE = "Practice";

        #region Public fields

        /// <summary>
        /// A single instance of the GameManager that is active at all times
        /// </summary>
        public static GameManager instance;

        /// <summary>
        /// The Model of the Game System
        /// </summary>
        public GameModel model;

        /// <summary>
        /// The ViewModel of the Game System
        /// </summary>
        public GameViewModel viewModel;

        /// <summary>
        /// The View of the Game System
        /// </summary>
        public GameView view;

        /// <summary>
        /// AI that will control other player moves
        /// </summary>
        public ComputerOpponent.ComputerOpponent cpu; 

        /// <summary>
        /// File containing Map Data to generate and load the map on the Game
        /// </summary>
        public TextAsset mapData;

        /// <summary>
        /// Available Game Types
        /// </summary>
        public enum GameType {
            Singleplayer,
            Multiplayer,
        }

        /// <summary>
        /// Available Singleplayer Game Types
        /// </summary>
        public enum SingleplayerGameType {
            Campaign,
            Practice
        }

        /// <summary>
        /// The current Game Type that will be set for the Game
        /// </summary>
        public GameType gameType;

        /// <summary>
        /// The current SinglePlayer Game Type that will be set for the Game
        /// </summary>
        public SingleplayerGameType singleplayerGameType;

        /// <summary>
        /// Determines if the Game will be played locally on one computer for all Players
        /// </summary>
        public bool localPlay;

        /// <summary>
        /// The number of Players that will be playing in the Game
        /// </summary>
        public int numberOfPlayers;

        /// <summary>
        /// The Player that the user will be controlling in the Game.
        /// Not used if local play is enabled.
        /// </summary>
        public Player ControllingPlayer;

        /// <summary>
        /// The TileFactory to generate the model and view representation of a Tile
        /// </summary>
        public TileFactory tileFactory;

        /// <summary>
        /// The UnitFactory to generate the model and view representation of a Unit
        /// </summary>
        public UnitFactory unitFactory;

        #endregion

        #region Initializers

        /// <summary>
        /// First method invoked when GameManager object is enabled.
        /// This method ensures that only one GameManager instance is active at a time
        /// </summary>
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

        /// <summary>
        /// Initializes the Game with configured settings.
        /// </summary>
        private void Start() {
            var selectedMap = SceneLoader.GetParam(SceneLoader.LOAD_MAP_PARAM);

            if (selectedMap != "") {
                mapData = MapLoader.instance.GetMapAsset(selectedMap);
            }

            var gameTypeString = SceneLoader.GetParam(SceneLoader.GAME_TYPE_PARAM);

            if (gameTypeString == GameManager.SINGLEPLAYER_GAME_TYPE) {
                gameType = GameType.Singleplayer;
            } else if (gameTypeString == GameManager.MULTIPLAYER_GAME_TYPE) {
                gameType = GameType.Multiplayer;
            } else {
                gameType = GameType.Singleplayer;
            }

            var singleGameTypeString = SceneLoader.GetParam(SceneLoader.SINGLEPLAYER_GAME_TYPE_PARAM);
            if (singleGameTypeString == GameManager.CAMPAIGN_GAME_TYPE) {
                singleplayerGameType = SingleplayerGameType.Campaign;
            } else if (singleGameTypeString == GameManager.PRACTICE_GAME_TYPE) {
                singleplayerGameType = SingleplayerGameType.Practice;
            } else {
                singleplayerGameType = SingleplayerGameType.Practice;
            }

            // Read map data from text file to get columns and rows
            var tileData = JsonUtility.FromJson<TileDataWrapper>(mapData.text);
            int cols = tileData.Columns;
            int rows = tileData.Rows;

            // Set up Players
            var players = new List<Player>();

            for (int player = 1; player <= numberOfPlayers; player++) {
                Player newPlayer = null;

                if (gameType == GameType.Singleplayer) {
                    newPlayer = new Player($"Player {player}");
                }

                if (gameType == GameType.Multiplayer) {
                    // TODO: Get logic to set up Players with unique names from server?
                }

                players.Add(newPlayer);
            }

            // If playing a Singleplayer game, the user always controls the first Player (Player 1)
            if (gameType == GameType.Singleplayer) {
                ControllingPlayer = players.First();
            }

            // If playing a Multiplayer game, the server decides which player the user controls
            if (gameType == GameType.Multiplayer) {
                // TODO: Set up logic to find out which player the human in controlling
            }

            model.ConstructModel(cols, rows, players);

            var newObjectViews = new Dictionary<Vector2Int, ObjectView>();

            // Initializing Model/View component of the map with Tiles/Units
            foreach(var tile in tileData.tileData) {

                // Initialize Tile Model/View
                var newTile = tileFactory.CreateTile(tile, view.transform);
                model.AddTile(newTile);

                // If Tile contains Player data, initialize Unit Model/View
                if (tile.Player != 0) {
                    var newUnitTuple = unitFactory.CreateUnit(tile, view.gameObject.transform);
                    var newUnitModel = newUnitTuple.Item1;
                    var newUnitObject = newUnitTuple.Item2;

                    var newUnitView= new UnitView(newUnitObject);

                    newObjectViews.Add(new Vector2Int(tile.Column, tile.Row), newUnitView);

                    model.AddUnit(newUnitModel, tile.Player, tile.Column, tile.Row);
                }
            }

            // Invoke model to set up tile neighbors
            model.AddNeighbors();

            viewModel.ConstructViewModel();

            view.ConstructView(cols, rows, newObjectViews);

            StartGame();
        }

        /// <summary>
        /// Starts the Game and applys other player moves if necessary for the first turn
        /// </summary>
        private void StartGame()
        {
            model.StartGame();
            viewModel.StartGame();

            // If first player is not the Controlling Player
            // Wait for other player's to make Move's first
            if (model.CurrentPlayer != ControllingPlayer) {
                viewModel.WaitForOtherMoves();
            }

            viewModel.GameFinished += ViewModel_GameFinished;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Acquires the GameMoves from other Players that will be applied on the game
        /// when it is not the controlling player's turn.
        /// </summary>
        /// <returns></returns>
        public GameMove GetOtherPlayerMove() {
            while (true) {

                // Call and return AI Best Move
                if (gameType == GameType.Singleplayer) {
                    Debug.Log("Getting CPU Move");
                    return cpu.FindBestMove();
                }
                
                // TODO: Call and return MP Move
                if (gameType == GameType.Multiplayer) {
                    throw new NotImplementedException();
                }
            }
        }

        public void SaveGame() {
            // TODO: Serialize CurrentControllingPlayer Unit Data here

            // TODO: Serialize CurrentCampaignSequence Data here
        }

        public void RestartGame() {
            SceneLoader.instance.ReloadMap();
        }

        #endregion

        #region Private Methods

        private void ViewModel_GameFinished(object sender, EventArgs e) {
            if (gameType == GameType.Singleplayer) {
                if (singleplayerGameType == SingleplayerGameType.Practice) {
                    SceneLoader.instance.GoToLastMenu();
                }

                if (singleplayerGameType == SingleplayerGameType.Campaign) {
                    if (ControllingPlayer.HasAliveUnit()) {
                        CampaignManager.instance.CampaignPlayerData = ControllingPlayer;
                        CampaignManager.instance.LoadNextCampaignEvent();
                    } else {
                        SceneLoader.instance.GoToCampaignMenu();
                    }
                }
            }

            if (gameType == GameType.Multiplayer) {
                // TODO: Do stuff here
            }
        }

        #endregion
    }
}
