using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileData 
{
    public int Column;
    public int Row;
    public string FloorData;
    public string ObstacleData;
    public string WallData;

    public int Player;
    public string UnitData;

    public TileData() {

    }

    public TileData(int col, int row, string floorData, string obstacleData, string wallData) {
        Column = col;
        Row = row;
        FloorData = floorData;
        ObstacleData = obstacleData;
        WallData = wallData;

        Player = 0;
        UnitData = "";
    }

    public TileData(int col, int row, string floorData, string obstacleData, string wallData, int player, string unitData) {
        Column = col;
        Row = row;
        FloorData = floorData;
        ObstacleData = obstacleData;
        WallData = wallData;

        Player = player;
        UnitData = unitData;
    }
}
