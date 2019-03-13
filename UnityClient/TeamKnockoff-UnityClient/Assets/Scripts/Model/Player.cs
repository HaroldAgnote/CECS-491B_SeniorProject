using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model {
    public class Player 
    {
        public List<Unit> Units { get; private set; }

        public List<bool> UnitHasMoved { get; private set; }

        public List<bool> UnitIsActive { get; private set; }

        public string name;

        public Player(string name) {
            this.name = name;
            Units = new List<Unit>();
            UnitHasMoved = new List<bool>();
            UnitIsActive = new List<bool>();
        }

        public void AddUnit(Unit unit) {
            Units.Add(unit);
            UnitHasMoved.Add(false);
            UnitIsActive.Add(true);
        }

        public void StartTurn() {
            for (int i = 0; i < UnitHasMoved.Count; i++) {
                if (UnitIsActive[i]) {
                    UnitHasMoved[i] = false;
                }
            }
        }

        public bool CheckUnitHasMoved(Unit unit) {
            int index = Units.FindIndex(x => x == unit);
            return UnitHasMoved[index];
        }

        public bool CheckUnitIsActive(Unit unit) {
            int index = Units.FindIndex(x => x == unit);
            return UnitIsActive[index];
        }

        public void MarkUnitAsMoved(Unit unit) {
            int index = Units.FindIndex(x => x == unit);
            UnitHasMoved[index] = true;
        }

        public void MarkUnitAsInactive(Unit unit) {
            int index = Units.FindIndex(x => x == unit);
            UnitIsActive[index] = false;
            UnitHasMoved[index] = true;
            // units[index].SetActive(false);
        }

        public bool HasAliveUnit() {
            var result = UnitIsActive.Any(x => x == true);
            return result;
        }
        
    }
}
