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
using Assets.Scripts.Model.Tiles;
using Assets.Scripts.Model.Units;
using Assets.Scripts.ExtensionMethods;

namespace Assets.Scripts.ViewModel {
    public class GameViewModel : MonoBehaviour, INotifyPropertyChanged {
        public class GameSquare : INotifyPropertyChanged {
            public Vector2Int Position;

            private ObjectViewModel mGameObject;

            public ObjectViewModel GameObject {
                get { return mGameObject; }
                set {
                    mGameObject = value;
                    OnPropertyChanged(nameof(GameObject));
                }
            }

            public Unit Unit {
                get {
                    if (GameObject is UnitViewModel) {
                        var unitViewModel = mGameObject as UnitViewModel;
                        return unitViewModel.Unit;
                    } else {
                        return null;
                    }
                }
            }

            public Tile Tile {
                get;
                set;
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void OnPropertyChanged(string name) {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        [SerializeField]
        private GameModel model;

        private ObservableList<GameSquare> mGameSquares;

        private int mCurrentTurn;

        private GameSquare mSelectedSquare;

        public GameSquare SelectedSquare {
            get {
                return mSelectedSquare;
            }
            set {
                mSelectedSquare = value;
                OnPropertyChanged(nameof(SelectedSquare));
            }
        }

        public ObservableList<GameSquare> Squares { get { return mGameSquares; } }

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

        public Player CurrentPlayer { get { return model.CurrentPlayer; } }

        public Player ControllingPlayer { get { return GameManager.instance.ControllingPlayer; } }

        public bool IsControllingPlayersTurn {
            get { return GameManager.instance.localPlay || CurrentPlayer == GameManager.instance.ControllingPlayer; }
        }

        public bool SelectedUnitBelongsToPlayer {
            get {
                return model.DoesUnitBelongToCurrentPlayer(SelectedSquare.Unit);
            }
        }

        public IEnumerable<Vector2Int> MovesForUnit {
            get {
                return model.GetPossibleUnitMoveLocations(SelectedSquare.Unit);
            }
        }

        public IEnumerable<Vector2Int> AttacksForUnit {
            get {
                return model.GetPossibleUnitAttackLocations(SelectedSquare.Unit);
            }
        }

        public IEnumerable<Vector2Int> SkillsForUnit {
            get {
                return model.GetPossibleUnitSkillLocations(SelectedSquare.Unit);
            }
        }

        public IEnumerable<Vector2Int> DamageSkillsForUnit {
            get {
                return model.GetPossibleUnitDamageSkillLocations(SelectedSquare.Unit);
            }
        }

        public IEnumerable<Vector2Int> SupportSkillsForUnit {
            get {
                return model.GetPossibleUnitSupportSkillLocations(SelectedSquare.Unit);
            }
        }

        public Dictionary<Vector2Int, ObjectViewModel> ObjectViewModels { get; set; }

        public void ConstructViewModel(Dictionary<Vector2Int, ObjectViewModel> objectViewModels) {
            if (model == null) {
                model = GameManager.instance.model;
            }

            ObjectViewModels = objectViewModels;

            mGameSquares = new ObservableList<GameSquare>(
                VectorExtension.GetRectangularPositions(GameManager.instance.Rows, GameManager.instance.Columns)
                .Select(pos => new GameSquare() {
                    Position = pos,
                    GameObject = objectViewModels.SingleOrDefault(vm => vm.Key == pos).Value,
                    Tile = model.GetTileAtPosition(pos)
                })
            );
            CurrentTurn = model.Turn;
        }

        public IEnumerable<Vector2Int> GetShortestPath(Vector2Int endPoint) {
            return model.GetShortestPath(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint);
        }
        
        public IEnumerable<Vector2Int> GetShortestPathToAttack(Vector2Int endPoint) {
            return model.GetShortestPathToAttack(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint);
        }

        public IEnumerable<Vector2Int> GetShortestPathToSkill(Vector2Int endPoint) {
            return model.GetShortestPathToSkill(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint);
        }

        public IEnumerable<Vector2Int> GetSurroundingAttackLocationsAtPoint(Vector2Int attackPoint, int range) {
            return model.GetSurroundingAttackLocationsAtPoint(attackPoint, range);
        }

        public bool EnemyAtPoint(Vector2Int position) {
            return model.EnemyAtLocation(position);
        }

        public bool EnemyWithinRange(Vector2Int position, int range) {
            return model.EnemyWithinRange(position, range);
        }

        public bool AllyWithinRange(Vector2Int position, int range) {
            return model.AllyWithinRange(position, range);
        }

        public bool AllyAPoint(Vector2Int position) {
            return model.AllyAtLocation(position);
        }

        public bool UnitHasMoved(UnitViewModel unitVm) {
            return model.UnitHasMoved(unitVm.Unit);
        }

        public Vector2Int GetPositionOfUnit(Unit unit) {
            return model.GridForUnit(unit);
        }

        public void ApplyMove(GameMove gameMove) {
            // Should check if move is possible before applying to prevent cheating?
            model.ApplyMove(gameMove);
            UpdateMove(gameMove);
            if (!model.GameHasEnded) {
                WaitForOtherMoves();
            }
            CurrentTurn = model.Turn;
        }

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
                    UpdateMove(move);
                }
                CurrentTurn = model.Turn;
            }

        }

        public void UpdateMove(GameMove gameMove) {
            if (gameMove.MoveType == GameMove.GameMoveType.Move) {
                if (gameMove.StartPosition != gameMove.EndPosition) {
                    var objectViewModel = ObjectViewModels[gameMove.StartPosition];
                    objectViewModel.UpdatePosition(gameMove.EndPosition);

                    var square = mGameSquares.SingleOrDefault(sq => sq.Position == gameMove.StartPosition);
                    var newSquare = mGameSquares.SingleOrDefault(sq => sq.Position == gameMove.EndPosition);

                    square.GameObject = null;
                    newSquare.GameObject = objectViewModel;

                    ObjectViewModels.Remove(gameMove.StartPosition);
                    ObjectViewModels.Add(gameMove.EndPosition, objectViewModel);
                }
            }

            else if (gameMove.MoveType == GameMove.GameMoveType.Attack || 
                    gameMove.MoveType == GameMove.GameMoveType.Skill) {
                var endObjectViewModel = ObjectViewModels[gameMove.EndPosition];
                if (endObjectViewModel.GetType().IsSameOrSubClass(typeof(UnitViewModel))) {
                    var unitViewModel = endObjectViewModel as UnitViewModel;
                    if (!unitViewModel.Unit.IsAlive) {
                        unitViewModel.GameObject.SetActive(false);
                        ObjectViewModels.Remove(gameMove.EndPosition);
                    }
                }

                var startObjectViewModel = ObjectViewModels[gameMove.StartPosition];
                if (startObjectViewModel.GetType().IsSameOrSubClass(typeof(UnitViewModel))) {
                    var unitViewModel = startObjectViewModel as UnitViewModel;
                    if (!unitViewModel.Unit.IsAlive) {
                        unitViewModel.GameObject.SetActive(false);
                        ObjectViewModels.Remove(gameMove.StartPosition);
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
