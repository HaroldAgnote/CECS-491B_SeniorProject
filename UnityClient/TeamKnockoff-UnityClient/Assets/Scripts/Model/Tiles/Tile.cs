using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Model.Tiles {
    public class Tile {
        public static Dictionary<string, BoardTileType> TILE_TYPES = new Dictionary<string, BoardTileType>() {
            { "Normal" , BoardTileType.Normal },
            { "Wall" , BoardTileType.Wall },
            { "Rough" , BoardTileType.Rough },
            { "Slope" , BoardTileType.Slope },
            { "Obstacle" , BoardTileType.Obstacle },
            { "Boundary" , BoardTileType.Boundary },
        };
        
        public static Dictionary<string, BoardTileEffect> TILE_EFFECTS = new Dictionary<string, BoardTileEffect>() {
            { "Normal" , BoardTileEffect.Normal },
            { "Damage" , BoardTileEffect.Damage },
            { "Fortify" , BoardTileEffect.Fortify },
        };

        public enum BoardTileType {

            // Normal Tiles
            Normal, 

            // Walls can only be traversed by flying units
            // Examples: Deep water, trees, etc.)
            Wall,

            // Rough tiles slow down land units
            // Rough tiles do not affect flying units
            // Examples: Sand, shallow water, swamps
            Rough,

            // Slope tiles affect movement cost of infantry units and flying units
            // Slope tiles cannot be traversed by armored and cavalry units
            // Examples: Mountains
            Slope,

            // Obstacles slow down flying units
            // Obstacles do not affect land units
            // Examples: The top of trees
            Obstacle,

            // Impassable tiles
            // This will usually sit on the out of bounds area of the map.
            // But you can also place boundary tiles within the map when needed.
            Boundary 
        }

        public enum BoardTileEffect {
            // This tile has no effect
            Normal,

            // Deal dmg at the beginning of each turn
            Damage,

            // Heal HP at the beginning of each turn and increase evasion rate
            Fortify,
        }

        public Vector2Int Position { get; set; }

        public BoardTileType TileType { get; set; }
        public BoardTileEffect TileEffect { get; set; }
        public List<Tile> Neighbors { get; set; }

        public Tile(int xPos, int yPos) {
            Position = new Vector2Int(xPos, yPos);
            Neighbors = new List<Tile>();
            TileType = BoardTileType.Normal;
        }

        public Tile(int xPos, int yPos, BoardTileType tileType) {
            Position = new Vector2Int(xPos, yPos);
            TileType = tileType;
            Neighbors = new List<Tile>();
        }
    }
}
