using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    public const int DEFAULT_MOVE_COST = 1;

    public enum BoardTileType {
        Normal, // Normal Tiles

        // Trees affect movement cost of infantry units and armored units
        // Trees cannot be traversed by cavalry units
        // Trees do not affect flying units
        Tree,

        // Shallow water tiles affect movement cost of infantry units and armored units
        // 
        Shallow,

        // Deep water tiles are ONLY traversed by flying units
        Deep,

        // Mountain tiles affect movement cost of infantry units
        // Mountain tiles cannot be traversed by armored and cavalry units
        Mountain,

        // Obstacles are only traversed by flying units
        Obstacle,

        Damage, // Deal dmg at the beginning of each turn
        Fortify, // Heal HP at the beginning of each turn and increase evasion rate
        Boundary // Impassable tiles
    }

    public int XPosition { get; set; }
    public int YPosition { get; set; }
    public int MoveCost { get; set; }

    public BoardTileType TileType { get; set; }
    public List<Tile> Neighbors { get; set; }

    public Tile(int xPos, int yPos) {
        XPosition = xPos;
        YPosition = yPos;
        Neighbors = new List<Tile>();
        TileType = BoardTileType.Normal;
        MoveCost = DEFAULT_MOVE_COST;
    }

    public Tile(int xPos, int yPos, BoardTileType tileType) {
        XPosition = xPos;
        YPosition = yPos;
        TileType = tileType;
        Neighbors = new List<Tile>();
        MoveCost = DEFAULT_MOVE_COST;
    }

    public Tile(int xPos, int yPos, int moveCost, BoardTileType tileType) {
        XPosition = xPos;
        YPosition = yPos;
        TileType = tileType;
        Neighbors = new List<Tile>();
        MoveCost = moveCost;
    }
}
