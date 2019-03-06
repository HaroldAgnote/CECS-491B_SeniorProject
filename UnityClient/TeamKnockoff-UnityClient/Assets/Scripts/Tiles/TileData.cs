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
    public string BoundaryData;

    public int Player;
    public string UnitData;

    public TileData() {

    }

    public TileData(int col, int row, string floorData, string wallData, string obstacleData, string boundaryData) {
        Column = col;
        Row = row;
        FloorData = floorData;
        ObstacleData = obstacleData;
        WallData = wallData;
        BoundaryData = boundaryData;

        Player = 0;
        UnitData = "";
    }

    public TileData(int col, int row, string floorData, string wallData, string obstacleData, string boundaryData, int player, string unitData) {
        Column = col;
        Row = row;
        FloorData = floorData;
        ObstacleData = obstacleData;
        WallData = wallData;
        BoundaryData = boundaryData;

        Player = player;
        UnitData = unitData;
    }
}
