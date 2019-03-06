using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Units;

public class Player 
{
    public List<GameObject> units;
    public List<bool> hasMoved;
    public List<bool> isActive;

    public string name;

    public Player(string name) {
        this.name = name;
        units = new List<GameObject>();
        hasMoved = new List<bool>();
        isActive = new List<bool>();
    }

    public void AddUnit(GameObject unit) {
        units.Add(unit);
        hasMoved.Add(false);
        isActive.Add(true);
    }

    public void StartTurn() {
        for (int i = 0; i < hasMoved.Count; i++) {
            if (isActive[i]) {
                hasMoved[i] = false;
            }
        }
    }

    public bool CheckUnitHasMoved(GameObject unit) {
        int index = units.FindIndex(x => x == unit);
        return hasMoved[index];
    }

    public bool CheckUnitIsActive(GameObject unit) {
        int index = units.FindIndex(x => x == unit);
        return isActive[index];
    }

    public void MarkUnitAsMoved(GameObject unit) {
        int index = units.FindIndex(x => x == unit);
        hasMoved[index] = true;
    }

    public void MarkUnitAsInactive(GameObject unit) {
        int index = units.FindIndex(x => x == unit);
        isActive[index] = false;
        hasMoved[index] = true;
        units[index].SetActive(false);
    }

    public bool HasAliveUnit() {
        var result = isActive.Any(x => x == true);
        return result;
    }
    
}
