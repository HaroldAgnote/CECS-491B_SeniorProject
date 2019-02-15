using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    public const int DEFAULT_MOVE_COST = 1;

    public enum BoardTileType {
        Normal, // Normal Tiles
        Obstacle, // Tiles with some object, but can be traversed by some units
        Damage, // Deal 10 dmg at the beginning of each turn
        Fortify, // Heal at the beginning of each turn and increase evasion rate
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
