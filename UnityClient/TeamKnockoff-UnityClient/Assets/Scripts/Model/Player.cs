using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.Model {

    [Serializable]
    public class Player {
        [SerializeField]
        private string mName;

        public string Name { get { return mName; } }

        [SerializeField]
        private int mMoney;

        public int Money { get { return mMoney; } }

        [SerializeField]
        private List<Unit> mUnits;

        public List<Unit> Units { get { return mUnits; } }

        public Player() {
            mName = "Hero";
            mMoney = 1000;
            mUnits = new List<Unit>();
        }

        public Player(string name) {
            mName = name;
            mMoney = 1000;
            mUnits = new List<Unit>();
        }

        public void AddUnit(Unit unit) {
            mUnits.Add(unit);
        }

        public void StartTurn() {
            foreach(var unit in mUnits) {
                unit.StartTurn();
            }
        }

        public bool HasAliveUnit() {
            return mUnits.Any(unit => unit.IsAlive);
        }
        
    }
}
