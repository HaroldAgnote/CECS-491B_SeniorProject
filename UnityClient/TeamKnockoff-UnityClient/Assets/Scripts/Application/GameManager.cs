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
using Assets.Scripts.Model.Items;
using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Utilities.ExtensionMethods;
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
        private TileFactory tileFactory;

        /// <summary>
        /// The UnitFactory to generate the model and view representation of a Unit
        /// </summary>
        private UnitFactory unitFactory;

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
            tileFactory = TileFactory.instance;
            unitFactory = UnitFactory.instance;

            var selectedMap = SceneLoader.GetParam(SceneLoader.LOAD_MAP_PARAM);

            if (selectedMap != "") {
                mapData = MapLoader.instance.GetMapAsset(selectedMap);
            } else {
            }

            var gameTypeString = SceneLoader.GetParam(SceneLoader.GAME_TYPE_PARAM);

            if (gameTypeString == GameManager.SINGLEPLAYER_GAME_TYPE) {
                gameType = GameType.Singleplayer;
            } else if (gameTypeString == GameManager.MULTIPLAYER_GAME_TYPE) {
                gameType = GameType.Multiplayer;
            } else {
            }

            if (gameType == GameType.Singleplayer) {
                var singleGameTypeString = SceneLoader.GetParam(SceneLoader.SINGLEPLAYER_GAME_TYPE_PARAM);

                if (singleGameTypeString == GameManager.CAMPAIGN_GAME_TYPE) {
                    singleplayerGameType = SingleplayerGameType.Campaign;

                } else if (singleGameTypeString == GameManager.PRACTICE_GAME_TYPE) {
                    singleplayerGameType = SingleplayerGameType.Practice;
                } else {
                    throw new Exception("Singleplayer Game Type not set!");
                }
            } else if (gameType == GameType.Multiplayer) {
            }

            // Read map data from text file to get columns and rows
            var tileData = JsonUtility.FromJson<TileDataWrapper>(mapData.text);
            int cols = tileData.Columns;
            int rows = tileData.Rows;

            // Set up Players
            var players = new List<Player>();

            for (int playerNum = 1; playerNum <= numberOfPlayers; playerNum++) {
                Player newPlayer = null;

                if (gameType == GameType.Singleplayer) {
                    if (singleplayerGameType == SingleplayerGameType.Practice) {
                        newPlayer = new Player($"Player {playerNum}", playerNum);
                    } else if (singleplayerGameType == SingleplayerGameType.Campaign) {
                        if (playerNum == 1) {
                            newPlayer = CampaignManager.instance.CampaignPlayerData;
                            newPlayer.Units.Clear();
                        } else {
                            newPlayer = new Player($"Player {playerNum}", playerNum);
                        }
                    } else {
                        throw new Exception($"Error setting up Player {playerNum}");
                    }
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

            Tuple<Unit, GameObject> newUnitTuple = null;

            var boardTileData = tileData.tileData;
            var boardUnitData = boardTileData.Where(data => data.Player != 0).Select(data =>
                new {
                    Player = data.Player,
                    UnitPosition = new Vector3(data.Column, data.Row, 0f),
                    UnitData = data.UnitData
                }
            );

            foreach (var boardTile in boardTileData) {
                // Initialize Tile Model/View
                var newTile = tileFactory.CreateTile(boardTile, view.tiles.transform);
                model.AddTile(newTile);
            }

            foreach (var boardUnit in boardUnitData) {
                var unitData = boardUnit.UnitData;
                var unitPosition = boardUnit.UnitPosition;
                var unitPlayer = boardUnit.Player;

                if (gameType == GameType.Singleplayer) {
                    if (singleplayerGameType == SingleplayerGameType.Campaign) {
                        if (unitPlayer == 1) {

                            const char DELIMITER = '_';
                            const int UNIT_NAME_INDEX = 3;

                            if (unitData == "FREE_ALLY_SQUARE") {

                            } else {
                                var split_string = unitData.Split(DELIMITER);

                                if (split_string.Length == 4) {
                                    // Named Unit
                                    var unitName = split_string[UNIT_NAME_INDEX];
                                    var existingUnit =
                                        CampaignManager.instance.CampaignPlayerData.CampaignUnits.SingleOrDefault(
                                            campaignUnit => campaignUnit.Name == unitName);

                                    if (existingUnit != null) {
                                        var cloneUnit = existingUnit.Clone();
                                        newUnitTuple = unitFactory.ImportUnitForBoard(unitPosition, view.units.transform, cloneUnit, unitData);
                                    } else {
                                        newUnitTuple = unitFactory.CreateUnitForBoard(unitPosition, view.units.transform, unitData);
                                    }
                                } else {
                                    throw new NotImplementedException();
                                }
                            }

                        } else {
                            newUnitTuple = unitFactory.CreateUnitForBoard(boardUnit.UnitPosition, view.units.transform, unitData);
                        }

                    } else if (singleplayerGameType == SingleplayerGameType.Practice) {
                        newUnitTuple = unitFactory.CreateUnitForBoard(boardUnit.UnitPosition, view.units.transform, unitData);
                    } else {
                        throw new Exception("Singleplayer Game Type not set");
                    }
                } else if (gameType == GameType.Multiplayer) {

                } else {
                    throw new Exception("Game Type not set");
                }

                var newUnitModel = newUnitTuple.Item1;
                var newUnitObject = newUnitTuple.Item2;

                var newUnitView= new UnitView(newUnitObject);
                var unitPos = unitPosition.ToVector2Int();
                newObjectViews.Add(unitPos, newUnitView);
                model.AddUnit(newUnitModel, unitPlayer, unitPos.x, unitPos.y);

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
        private void StartGame() {
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

                while (view.IsUpdating) { }

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
            if (!model.GameHasEnded) {
                model.EndGame();
            }
            SceneLoader.instance.ReloadMap();
        }

        public void QuitGame() {
            if (!model.GameHasEnded) {
                model.EndGame();
            }
            SceneLoader.instance.GoToMainMenu();
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
                        var campaignUnits = CampaignManager.instance.CampaignPlayerData.CampaignUnits;
                        for (int unitIndex = 0; unitIndex < campaignUnits.Count; unitIndex++) {
                            foreach (var unit in ControllingPlayer.Units) {
                                if (campaignUnits[unitIndex].Name == unit.Name) {
                                    campaignUnits[unitIndex] = unit;
                                }
                            }
                        }

                        var newUnits = ControllingPlayer.Units.Where(unit => !campaignUnits.Contains(unit));
                        CampaignManager.instance.CampaignPlayerData.CampaignUnits.AddRange(newUnits);
                        CampaignManager.instance.CampaignPlayerUnitData = CampaignManager.instance
                            .CampaignPlayerData.CampaignUnits.Select(unit => new UnitWrapper(unit)).ToList();

                        // TODO: Make better reward system
                        CampaignManager.instance.CampaignPlayerData.Money += 1000;

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
