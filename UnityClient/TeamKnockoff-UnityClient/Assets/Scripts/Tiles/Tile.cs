using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    public int MoveCost { get; set; }

    public int XPosition { get; set; }
    public int YPosition { get; set; }

    public List<Tile> Neighbors { get; set; }
}
