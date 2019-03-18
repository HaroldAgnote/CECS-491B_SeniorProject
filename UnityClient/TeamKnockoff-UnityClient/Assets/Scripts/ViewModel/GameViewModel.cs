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
    public class GameViewModel : MonoBehaviour, INotifyPropertyChanged
    {
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
                    var unitViewModel = mGameObject as UnitViewModel;
                    return unitViewModel.Unit;
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

        public GameSquare SelectedSquare { get; set; }

        public ObservableList<GameSquare> Squares { get { return mGameSquares; } }

        public Player CurrentPlayer { get { return model.CurrentPlayer; }
        }

        public bool IsControllingPlayersTurn {
            get { return GameManager.instance.localPlay || CurrentPlayer == GameManager.instance.ControllingPlayer; }
        }

        public bool SelectedUnitBelongsToPlayer {
            get {
                return model.DoesUnitBelongToCurrentPlayer(SelectedSquare.Unit);
            }
        }

        public List<Vector2Int> MovesForUnit {
            get {
                return model.GetPossibleUnitMoveLocations(SelectedSquare.Unit);
            }
        }

        public List<Vector2Int> AttacksForUnit {
            get {
                return model.GetPossibleUnitAttackLocations(SelectedSquare.Unit);
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
        }

        public List<Vector2Int> GetShortestPath(Vector2Int endPoint) {
            return model.GetShortestPath(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint);
        }
        
        public List<Vector2Int> GetShortestPathToAttack(Vector2Int endPoint) {
            return model.GetShortestPathToAttack(model.GetUnitAtPosition(SelectedSquare.Position), SelectedSquare.Position, endPoint);
        }

        public List<Vector2Int> GetSurroundingAttackLocationsAtPoint(Vector2Int attackPoint, int range) {
            return model.GetSurroundingAttackLocationsAtPoint(attackPoint, range);
        }

        public bool EnemyAtPoint(Vector2Int position) {
            return model.EnemyAtLocation(position);
        }

        public bool EnemyWithinRange(Vector2Int position, int range) {
            return model.EnemyWithinRange(position, range);
        }

        public bool UnitHasMoved(UnitViewModel unitVm) {
            return model.UnitHasMoved(unitVm.Unit);
        }

        public async void ApplyMove(GameMove gameMove) {
            // If playing a singleplayer (AI) or multiplayer game
            // Wait for other players to finish making their moves
            if (!GameManager.instance.localPlay) {
                while (!IsControllingPlayersTurn) {
                    
                    var moveTask = Task.Run(() => {
                        return GameManager.instance.GetOtherPlayerMove();
                    });

                    var move = await moveTask;

                    model.ApplyMove(move);
                    UpdateMove(move);
                }
            }

            // Should check if move is possible before applying to prevent cheating?
            model.ApplyMove(gameMove);
            UpdateMove(gameMove);
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

            else if (gameMove.MoveType == GameMove.GameMoveType.Attack) {
                var objectViewModel = ObjectViewModels[gameMove.EndPosition];
                if (objectViewModel.GetType().IsSameOrSubClass(typeof(UnitViewModel)))
                {
                    var unitViewModel = objectViewModel as UnitViewModel;
                    if (unitViewModel.Unit.HealthPoints == 0) {
                        unitViewModel.GameObject.SetActive(false);
                        ObjectViewModels.Remove(gameMove.EndPosition);
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
