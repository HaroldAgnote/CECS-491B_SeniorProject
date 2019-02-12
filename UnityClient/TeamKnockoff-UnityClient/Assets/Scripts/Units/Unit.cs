using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public double HealthPoints { get; set; }

    public int MoveRange { get; set; }

    public int AttackRange { get; set; }

    public string Name { get; set; }

    public List<Vector2Int> MoveLocations { get; set; }

    public abstract List<Vector2Int> GetMoveLocations(Vector2Int gridPoint);
    public abstract List<Vector2Int> GetAttackLocations(Vector2Int gridPoint);
}
