using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public double HealthPoints { get; set; }

    public int MoveRange { get; set; }

    public int AttackRange { get; set; }

    public string Name { get; set; }

    public abstract List<Vector2Int> MoveLocations(Vector2Int gridPoint);
    public abstract List<Vector2Int> AttackLocations(Vector2Int gridPoint);
}
