using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileDataWrapper
{
    public int Columns;
    public int Rows;
    public List<TileData> tileData;

    public TileDataWrapper() {

    }

    public TileDataWrapper(int columns, int rows, List<TileData> tD) {
        Columns = columns;
        Rows = rows;
        tileData = tD;
    }

}
