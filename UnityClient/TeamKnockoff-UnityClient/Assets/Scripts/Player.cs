using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    public List<GameObject> units;
    public List<bool> hasMoved;

    public string name;

    public Player(string name) {
        this.name = name;
        units = new List<GameObject>();
        hasMoved = new List<bool>();
    }

    public void AddUnit(GameObject unit) {
        units.Add(unit);
        hasMoved.Add(false);
    }

    public void StartTurn() {
        for (int i = 0; i < hasMoved.Count; i++) {
            hasMoved[i] = false;
        }
    }

    public bool CheckUnitHasMoved(GameObject unit) {
        int index = units.FindIndex(x => x == unit);
        return hasMoved[index];
    }

    public void MarkUnitAsMoved(GameObject unit) {
        int index = units.FindIndex(x => x == unit);
        hasMoved[index] = true;
    }
    
}
