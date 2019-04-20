using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Assets.Scripts.Model.Units;

namespace Assets.Scripts.ViewModel {
    public class UnitViewModel : INotifyPropertyChanged {

        private Unit mUnit;

        public Unit Unit { get { return mUnit; } }

        public UnitViewModel(Unit unit) {
            mUnit = unit;
        }

        private int mLevel;

        public int Level {
            get {
                return mLevel;
            }

            set {
                if (mLevel != value) {
                    mLevel = value;
                    OnPropertyChanged(nameof(Level));
                }
            }
        }

        private Stat mHealth;

        private Stat mStrength;

        private Stat mMagic;
        
        private Stat mDefense;
        
        private Stat mResistance;
        
        private Stat mSpeed;
        
        private Stat mSkill;
        
        private Stat mLuck;

        public UnitViewModel(Unit newUnit, params int [] optional) {
            mUnit = newUnit;
            mHealth = new Stat(mUnit.MaxHealthPoints.Base);
            mStrength = new Stat(mUnit.Strength.Base);
            mMagic = new Stat(mUnit.Magic.Base);
            mDefense = new Stat(mUnit.Defense.Base);
            mResistance = new Stat(mUnit.Resistance.Base);
            mSpeed = new Stat(mUnit.Speed.Base);
            mSkill = new Stat(mUnit.Skill.Base);
            mLuck = new Stat(mUnit.Skill.Base);
        }

        public void SyncUnit() {
            Level = mUnit.Level;
            CheckStatAfterLevelUp(mHealth, mUnit.MaxHealthPoints);
            CheckStatAfterLevelUp(mStrength, mUnit.Strength);
            CheckStatAfterLevelUp(mMagic, mUnit.Magic);
            CheckStatAfterLevelUp(mDefense, mUnit.Defense);
            CheckStatAfterLevelUp(mResistance, mUnit.Resistance);
            CheckStatAfterLevelUp(mSpeed, mUnit.Speed);
            CheckStatAfterLevelUp(mSkill, mUnit.Skill);
            CheckStatAfterLevelUp(mLuck, mUnit.Luck);
        }

        public void CheckStatAfterLevelUp(Stat viewModelStat, Stat unitStat) {
            viewModelStat.Modifier += unitStat.Base - viewModelStat.Base;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
