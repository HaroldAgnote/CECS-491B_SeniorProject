using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model {
    public class Player 
    {
        public List<Unit> Units { get; private set; }

        public string name;

        public Player(string name) {
            this.name = name;
            Units = new List<Unit>();
        }

        public void AddUnit(Unit unit) {
            Units.Add(unit);
        }

        public void StartTurn() {
            foreach(var unit in Units) {
                unit.StartTurn();
            }
        }

        public bool HasAliveUnit() {
            return Units.Any(unit => unit.IsAlive);
        }
        
    }
}
