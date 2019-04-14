using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Application;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Skills;
using Assets.Scripts.Model.Tiles;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Items;
using Assets.Scripts.Utilities.ExtensionMethods;
using Assets.Scripts.Utilities.ObservableList;

namespace Assets.Scripts.ViewModel {
    /// <summary>
    /// ViewModel component of the Game Application
    /// </summary>
    public class GameViewModel : MonoBehaviour, INotifyPropertyChanged {

        /// <summary>
        /// GameSquare used to keep track of the Tiles in the game, their position, and 
        /// </summary>
        public class GameSquare : INotifyPropertyChanged {

            /// <summary>
            /// Unit on the Square (if any exists)
            /// </summary>
            private Unit mUnit;

            /// <summary>
            /// Tile on the Square
            /// </summary>
            private Tile mTile;

            /// <summary>
            /// Position of the GameSquare
            /// </summary>
            public Vector2Int Position { get; set; }

            /// <summary>
            /// Unit on the Square (if any exists)
            /// </summary>
            public Unit Unit {
                get {
                    return mUnit;
                }

                set {
                    // If Unit changes, raise property change event
                    if (mUnit != value) {
                        mUnit = value;
                        OnPropertyChanged(nameof(Unit));
                    }
                }
            }

            /// <summary>
            /// Tile on the Square
            /// </summary>
            public Tile Tile {
                get {
                    return mTile;
                }

                set {
                    // If Tile changes, raise property change event
                    if (mTile != value) {
                        mTile = value;
                        OnPropertyChanged(nameof(Tile));
                    }
                }
            }

            /// <summary>
            /// Determines if there is an object (e.g. Unit, Item, etc.) at the Square
            /// </summary>
            public bool ObjectAtSquare {
                get {
                    // TODO: If we add items that can be placed here, add more checks
                    return mUnit != null;
                }
            }

            /// <summary>
            /// Event Handler for changed properties
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// Invokes methods subscribed to the PropertyChanged Event
            /// </summary>
            /// <param name="name"></param>
            public void OnPropertyChanged(string name) {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        #region Data members
        /// <summary>
        /// Model representation of the Game
        /// </summary>
        [SerializeField]
        private GameModel model;

        /// <summary>
        /// The Current Turn of the Game
        /// </summary>
        private int mCurrentTurn;

        /// <summary>
        /// Determines if the game is over or not
        /// </summary>
        private bool mGameOver;

        /// <summary>
        /// The Current Player controlling the Game
        /// </summary>
        private Player mCurrentPlayer;

        /// <summary>
        /// The current Square the cursor has selected
        /// </summary>
        private GameSquare mSelectedSquare;

        /// <summary>
        /// The current Square the cursor is over
        /// </summary>
        private GameSquare mHoveredSquare;

        /// <summary>
        /// The current Square the Player is targeting
        /// </summary>
        private GameSquare mTargetSquare;

        /// <summary>
        /// Determines if the Player is in combat mode
        /// </summary>
        private bool mCombatMode;

        private bool mMoveState;

        /// <summary>
        /// List of Game Squares in the Game
        /// </summary>
        private ObservableList<GameSquare> mGameSquares;

        /// <summary>
        /// Last Move that was applied onto the Game
        /// </summary>
        private GameMove mLastMove;

        private Unit mSelectedUnit;

        private bool mGamePaused;

        #endregion

        #region Properties

        #region Observable Properties

        /// <summary>
        /// The Current Turn of the Game
        /// </summary>
        public int CurrentTurn {
            get {
                return mCurrentTurn;
            }

            set {
                if (value != mCurrentTurn) {
                    mCurrentTurn = value;
                    OnPropertyChanged(nameof(CurrentTurn));
                }
            }
        }

        /// <summary>
        /// Current Player controlling the board
        /// </summary>
        public Player CurrentPlayer {
            get {
                return mCurrentPlayer;
            }
            set {
                if (mCurrentPlayer != value) {
                    mCurrentPlayer = value;
                    OnPropertyChanged(nameof(CurrentPlayer));
                }
            }
        }

        /// <summary>
        /// The current Square the cursor has selected
        /// </summary>
        public GameSquare SelectedSquare {
            get {
                return mSelectedSquare;
            }
            set {
                mSelectedSquare = value;
                OnPropertyChanged(nameof(SelectedSquare));
            }
        }

        /// <summary>
        /// The current Square the cursor is over
        /// </summary>
        public GameSquare HoveredSquare {
            get {
                return mHoveredSquare;
            }
            set {
                mHoveredSquare = value;
                OnPropertyChanged(nameof(HoveredSquare));
            }
        }

        /// <summary>
        /// The current Square the Player is targeting
        /// </summary>
        public GameSquare TargetSquare {
            get {
                return mTargetSquare;
            }
            set {
                mTargetSquare = value;
                OnPropertyChanged(nameof(TargetSquare));
            }
        }

        public Unit SelectedUnit {
            get {
                return mSelectedUnit;
            }
            set {
                mSelectedUnit = value;
                OnPropertyChanged(nameof(SelectedUnit));
            }
        }

        /// <summary>
        /// Determines if the Player is in combat mode
        /// </summary>
        public bool CombatMode {
            get {
                return mCombatMode;
            }
            
            set {
                if (mCombatMode != value) {
                    mCombatMode = value;
                    OnPropertyChanged(nameof(CombatMode));
                }
            }
        }

        /// <summary>
        /// List of Game Squares in the Game
        /// </summary>
        public ObservableList<GameSquare> Squares { get { return mGameSquares; } }

        /// <summary>
        /// Last Move that was applied onto the Game
        /// </summary>
        public GameMove LastMove {
            get {
                return mLastMove;
            }

            set {
                if (mLastMove != value) {
                    mLastMove = value;
                    OnPropertyChanged(nameof(LastMove));
                }
            }
        }

        #endregion

        /// <summary>
        /// The Controlling (Human) Player of the Game
        /// </summary>
        public Player ControllingPlayer => GameManager.instance.ControllingPlayer;

        /// <summary>
        /// Determines if it is the Human Player's turn in the Game
        /// </summary>
        public bool IsControllingPlayersTurn {
            get { return GameManager.instance.localPlay || CurrentPlayer == GameManager.instance.ControllingPlayer; }
        }

        public bool IsGameOver {
            get { return mGameOver; }

            set {
                if (mGameOver != value) {
                    mGameOver = value;
                    OnPropertyChanged(nameof(IsGameOver));
                }
            }
        }

        /// <summary>
        /// Determines if the Selected Unit belongs to the Player
        /// </summary>
        public bool SelectedUnitBelongsToPlayer {
            get {
                return model.DoesUnitBelongToCurrentPlayer(SelectedSquare.Unit);
            }
        }

        /// <summary>
        /// Gets the Possible Moves of the Selected Unit
        /// </summary>
        public IEnumerable<Vector2Int> MovesForUnit {
            get {
                return model.GetPossibleUnitMoveLocations(SelectedUnit);
            }
        }

        /// <summary>
        /// Gets the Possible Attack Positions of the Selected Unit
        /// </summary>
        public IEnumerable<Vector2Int> AttacksForUnit {
            get {
                return model.GetPossibleUnitAttackLocations(SelectedUnit);
            }
        }

        public Dictionary<ConsumableItem, HashSet<Vector2Int>> ItemsForUnits
        {
            get {
                return model.GetPossibleUnitItemLocations(SelectedUnit);
            }
        }
        /// <summary>
        /// Gets the Possible Skill Positions of the Selected Unit
        /// </summary>
        public Dictionary<ActiveSkill, HashSet<Vector2Int>> SkillsForUnit {
            get {
                return model.GetUnitPossibleSkillLocations(SelectedUnit); 
            }
        }

        /// <summary>
        /// Gets the Possible Damage Skill Positions of the Selected Unit
        /// </summary>
        public IEnumerable<Vector2Int> DamageSkillsForUnit {
            get {
                return model.GetPossibleUnitDamageSkillLocations(SelectedUnit);
            }
        }

        /// <summary>
        /// Gets the Possible Support Skill Positions of the Selected Unit
        /// </summary>
        public IEnumerable<Vector2Int> SupportSkillsForUnit {
            get {
                return model.GetPossibleUnitSupportSkillLocations(SelectedUnit);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Event handler to used when a Property of the ViewModel has changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler GameFinished;

        /// <summary>
        /// Method used to invoke all methods subscribed to the PropertyChanged event
        /// </summary>
        /// <param name="name">The property that has changed</param>
        public void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Used to initialize the ViewModel and its properties
        /// </summary>
        public void ConstructViewModel() {

            // If model was not initailized properly, get Model from GameManager
            if (model == null) {
                model = GameManager.instance.model;
            }


            // Initialize Properties

            mGameSquares = new ObservableList<GameSquare>(
                VectorExtension.GetRectangularPositions(model.Columns, model.Rows)
                .Select(pos => new GameSquare() {
                    Position = pos,
                    Unit = model.GetUnitAtPosition(pos),
                    Tile = model.GetTileAtPosition(pos),
                })
            );
            CombatMode = false;
        }

        /// <summary>
        /// Initializing more properties at start of game
        /// </summary>
        public void StartGame() {
            CurrentPlayer = model.CurrentPlayer;
            CurrentTurn = model.Turn;
        }

        /// <summary>
        /// Uses Model's implementation to get the Shortest Move Path of the Selected Unit to another Square
        /// </summary>
        /// <param name="endPoint">Endpoint to find the Shortest Path to</param>
        /// <returns>
        /// Enumerable list of positions of the path to the endpoint
        /// </returns>
        public IEnumerable<Vector2Int> GetShortestPath(Vector2Int endPoint) {
            return model.GetShortestPath(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint);
        }
        
        /// <summary>
        /// Uses Model's implementation to get the Shortest Attack Path of the Selected Unit to a target Square
        /// </summary>
        /// <param name="endPoint">Target Point to find the Shortest Path to</param>
        /// <returns>
        /// Enumerable list of positions of the path to the endpoint closest to the target point
        /// </returns>
        public IEnumerable<Vector2Int> GetShortestPathToAttack(Vector2Int endPoint) {
            return model.GetShortestPathToAttack(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint);
        }

        /// <summary>
        /// Uses Model's implementation to get the Shortest Attack Path of the Selected Unit to a target Square
        /// </summary>
        /// <param name="endPoint">Target Point to find the Shortest Path to</param>
        /// <returns>
        /// Enumerable list of positions of the path to the endpoint closest to the target point
        /// </returns>
        public IEnumerable<Vector2Int> GetShortestPathToSkill(Vector2Int endPoint) {
            return model.GetShortestPathToSkill(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint);
        }

        /// <summary>
        /// Uses Model's implementation to get the Shortest Attack Path of the Selected Unit to a target Square
        /// </summary>
        /// <param name="endPoint">Target Point to find the Shortest Path to</param>
        /// <returns>
        /// Enumerable list of positions of the path to the endpoint closest to the target point
        /// </returns>
        public IEnumerable<Vector2Int> GetShortestPathToSkill(Vector2Int endPoint, SingleTargetSkill skill) {
            return model.GetShortestPathToSkill(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint, skill);
        }

        public IEnumerable<Vector2Int> GetShortestPathToItem(Vector2Int endPoint)
        {
            return model.GetShortestPathToItem(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint);
        }

        public IEnumerable<Vector2Int> GetShortestPathToItem(Vector2Int endPoint, ConsumableItem item)
        {
            return model.GetShortestPathToItem(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint, item);
        }

        /// <summary>
        /// Uses Model's implementation to get the Surrounding Attack Points of a Position with some range
        /// </summary>
        /// <param name="attackPoint">Attack Point</param>
        /// <param name="range">Range of attack</param>
        /// <returns>
        /// List of Positions that can be attacked with the range
        /// </returns>
        public IEnumerable<Vector2Int> GetSurroundingLocationsAtPoint(Vector2Int attackPoint, int range) {
            return model.GetSurroundingAttackLocationsAtPoint(attackPoint, range);
        }

        /// <summary>
        /// Uses Model's implementation to determine if there is an Enemy of Player at some Point
        /// </summary>
        /// <param name="position">The position to check for an enemy</param>
        /// <returns>
        /// Returns <c>true</c> if there is an Enemy at the Point, <c>false</c> otherwise
        /// </returns>
        public bool EnemyAtPoint(Vector2Int position) {
            return model.EnemyAtLocation(position);
        }

        /// <summary>
        /// Determines if there is an Enemy within some Attack Range of a position
        /// </summary>
        /// <param name="position">The position to check within range</param>
        /// <param name="range">The range of an attack</param>
        /// <returns>
        /// Returns <c>true</c> if there is an enemy within range, <c>false</c> otherwise
        /// </returns>
        public bool EnemyWithinRange(Vector2Int position, int range) {
            return model.EnemyWithinRange(position, range);
        }

        /// <summary>
        /// Determines if there is an Ally within some Range of a position
        /// </summary>
        /// <param name="position">The position to check within range</param>
        /// <param name="range">The range of an attack</param>
        /// <returns>
        /// Returns <c>true</c> if there is an ally within range, <c>false</c> otherwise
        /// </returns>
        public bool AllyWithinRange(Vector2Int position, int range) {
            return model.AllyWithinRange(position, range);
        }

        /// <summary>
        /// Uses Model's implementation to determine if there is an Ally of Player at some Point
        /// </summary>
        /// <param name="position">The position to check for an Ally</param>
        /// <returns>
        /// Returns <c>true</c> if there is an Ally at the Point, <c>false</c> otherwise
        /// </returns>
        public bool AllyAtPoint(Vector2Int position) {
            return model.AllyAtLocation(position);
        }

        /// <summary>
        /// Determines if a Skill is usable on a target position
        /// </summary>
        /// <param name="skill">The Skill that will be used</param>
        /// <param name="targetPos">The position the Skill should affect</param>
        /// <returns>
        /// Returns <c>true</c> if the Skill is usable, <c>false</c> otherwise
        /// </returns>
        public bool SkillUsableOnTarget(SingleTargetSkill skill, Vector2Int targetPos) {
            return model.SkillIsUsableOnTarget(SelectedUnit, model.GetUnitAtPosition(targetPos), skill);
        }

        public bool ItemUsableOnTarget(ConsumableItem item, Vector2Int targetPos)
        {
            return model.ItemIsUsableOnTarget(SelectedUnit, model.GetUnitAtPosition(targetPos), item);
        }
        /// <summary>
        /// Retrieves the Position of a given Unit
        /// </summary>
        /// <param name="unit">Unit to check Position</param>
        /// <returns>
        /// Returns the (x,y) coordinate of a Position
        /// </returns>
        public Vector2Int GetPositionOfUnit(Unit unit) {
            return model.GridForUnit(unit);
        }

        /// <summary>
        /// Applies a GameMove onto the Model representation of the game
        /// </summary>
        /// <param name="gameMove">The Move that will be applied onto the Game</param>
        public void ApplyMove(GameMove gameMove) {

            // TODO: Should check if move is possible before applying to prevent cheating?
            model.ApplyMove(gameMove);
            LastMove = gameMove;
            RebindState();

            WaitForOtherMoves();
        }

        /// <summary>
        /// Used to wait and apply other player's moves when it is not the controlling human player's turn
        /// </summary>
        public async void WaitForOtherMoves() {
            // If playing a singleplayer (AI) or multiplayer game
            // Wait for other players to finish making their moves
            if (!GameManager.instance.localPlay) {
                while (!IsControllingPlayersTurn && !model.GameHasEnded) {
                    Debug.Log("Getting other player's moves");
                    var moveTask = Task.Run(() => {
                        return GameManager.instance.GetOtherPlayerMove();
                    });

                    var move = await moveTask;

                    model.ApplyMove(move);
                    LastMove = move;
                    RebindState();
                }
            }
        }

        /// <summary>
        /// Rebinds properties of the Game
        /// </summary>
        public void RebindState() {
            var newSquares = VectorExtension.GetRectangularPositions(model.Columns, model.Rows);

            int i = 0;
            foreach (var pos in newSquares) {
                mGameSquares[i].Unit = model.GetUnitAtPosition(pos);
                i++;
            }

            CurrentPlayer = model.CurrentPlayer;
            CurrentTurn = model.Turn;
            IsGameOver = model.GameHasEnded;
        }

        public void PauseGame() {
            mGamePaused = true;
        }

        public void UnpauseGame() {
            mGamePaused = false;
        }

        public void FinishGame() {
            GameFinished?.Invoke(this, new EventArgs());
        }

        #endregion
    }
}
