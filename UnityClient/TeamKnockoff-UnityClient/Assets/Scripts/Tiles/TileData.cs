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

    public TileData() {

    }

    public TileData(int col, int row, string floorData, string obstacleData, string wallData) {
        Column = col;
        Row = row;
        FloorData = floorData;
        ObstacleData = obstacleData;
        WallData = wallData;
    }
}
