using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Weapons;

namespace Assets.Scripts.Model {

    [Serializable]
    public class Player {
        [SerializeField]
        private string mName;

        public string Name { get { return mName; } }

        [SerializeField]
        private int mMoney;

        public int Money {
            get {
                return mMoney;
            } set {
                if (mMoney != value) {
                    mMoney = value;
                }
            }
        }

        private List<Unit> mUnits;

        public List<Unit> Units { get { return mUnits; } }

        private List<Unit> mCampaignUnits;

        public List<Unit> CampaignUnits { get { return mCampaignUnits; } }

        private List<Weapon> mWeapons;

        public List<Weapon> Weapons { get { return mWeapons; } }

        public Player() {
            mName = "Hero";
            mMoney = 1000;
            mUnits = new List<Unit>();
            mCampaignUnits = new List<Unit>();
            mWeapons = new List<Weapon>();
        }

        public Player(string name) {
            mName = name;
            mMoney = 1000;
            mUnits = new List<Unit>();
            mCampaignUnits = new List<Unit>();
            mWeapons = new List<Weapon>();
        }

        public void AddUnit(Unit unit) {
            if (mUnits == null) {
                mUnits = new List<Unit>();
            }
            mUnits.Add(unit);
        }

        public void AddCampaignUnit(Unit unit) {
            mCampaignUnits.Add(unit);
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
