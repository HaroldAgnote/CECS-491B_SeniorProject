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
            mLevel = new Stat(mUnit.Level);
            mHealth = new Stat(mUnit.MaxHealthPoints.Base);
            mStrength = new Stat(mUnit.Strength.Base);
            mMagic = new Stat(mUnit.Magic.Base);
            mDefense = new Stat(mUnit.Defense.Base);
            mResistance = new Stat(mUnit.Resistance.Base);
            mSpeed = new Stat(mUnit.Speed.Base);
            mSkill = new Stat(mUnit.Skill.Base);
            mLuck = new Stat(mUnit.Skill.Base);
        }

        public int ExperiencePoints => mUnit.ExperiencePoints;

        private Stat mLevel;

        public Stat Level {
            get {
                return mLevel;
            }

            set {
                if (mLevel.Base != value.Base) {
                    mLevel.Modifier += value.Base - mLevel.Base;
                    CheckStatAfterLevelUp(mHealth, mUnit.MaxHealthPoints);
                    CheckStatAfterLevelUp(mStrength, mUnit.Strength);
                    CheckStatAfterLevelUp(mMagic, mUnit.Magic);
                    CheckStatAfterLevelUp(mDefense, mUnit.Defense);
                    CheckStatAfterLevelUp(mResistance, mUnit.Resistance);
                    CheckStatAfterLevelUp(mSpeed, mUnit.Speed);
                    CheckStatAfterLevelUp(mSkill, mUnit.Skill);
                    CheckStatAfterLevelUp(mLuck, mUnit.Luck);
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

        public Stat Health {
            get {
                return mHealth;
            }
        }

        public Stat Strength {
            get {
                return mStrength;
            }
        }

        public Stat Magic {
            get {
                return mMagic;
            }
        }

        public Stat Defense {
            get {
                return mDefense;
            }
        }

        public Stat Resistance {
            get {
                return mResistance;
            }
        }

        public Stat Speed {
            get {
                return mSpeed;
            }
        }

        public Stat Skill {
            get {
                return mSkill;
            }
        }

        public Stat Luck {
            get {
                return mLuck;
            }
        }

        public void SyncUnit() {
            Level = new Stat(mUnit.Level);
        }

        public void ResetStats() {
            mLevel = new Stat(mUnit.Level);
            mHealth = new Stat(mUnit.MaxHealthPoints.Base);
            mStrength = new Stat(mUnit.Strength.Base);
            mMagic = new Stat(mUnit.Magic.Base);
            mDefense = new Stat(mUnit.Defense.Base);
            mResistance = new Stat(mUnit.Resistance.Base);
            mSpeed = new Stat(mUnit.Speed.Base);
            mSkill = new Stat(mUnit.Skill.Base);
            mLuck = new Stat(mUnit.Luck.Base);
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
