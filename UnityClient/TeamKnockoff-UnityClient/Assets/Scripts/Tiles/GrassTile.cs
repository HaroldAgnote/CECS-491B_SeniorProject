using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassTile : Tile
{
    const int GRASS_TILE_MOVE_COST = 1;

    public GrassTile(int xPos, int yPos) : base(xPos, yPos) {
        MoveCost = GRASS_TILE_MOVE_COST;
    }
}
