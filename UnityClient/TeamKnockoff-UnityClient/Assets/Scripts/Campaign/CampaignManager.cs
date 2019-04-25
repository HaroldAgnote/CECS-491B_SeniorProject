using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Application;
using Assets.Scripts.Model;
using Assets.Scripts.Model.Units;
using Assets.Scripts.Model.Weapons;
using Assets.Scripts.Utilities.FileHandling;
using Assets.Scripts.Model.Items;

namespace Assets.Scripts.Campaign {
    public class CampaignManager : MonoBehaviour {
        const float EASY_MODIFIER = 0.50f;

        const float NORMAL_MODIFIER = 0.70f;

        const float HARD_MODIFIER = 1.00f;

        const float LUNATIC_MODIFIER = 1.20f;

        public static CampaignManager instance;

        public static Dictionary<CampaignDifficulty, float> DifficultyToModifier;

        public List<CampaignSequence> availableCampaigns;

        public CampaignSequence CurrentCampaignSequence { get; private set; }

        public CampaignDifficulty CurrentCampaignDifficulty { get; private set; }

        private bool mCurrentCampaignIsCompleted;

        public bool CurrentCampaignIsCompleted {
            get { return mCurrentCampaignIsCompleted; }
            set {
                if (mCurrentCampaignIsCompleted != value) {
                    mCurrentCampaignIsCompleted = value;
                }
            }
        }

        private int mCurrentCampaignIndex;

        public int CurrentCampaignIndex {
            get { return mCurrentCampaignIndex; }
            set {
                if (mCurrentCampaignIndex != value) {
                    mCurrentCampaignIndex = value;
                }
                
                // Ensure chapter stays at last chapter
                if (mCurrentCampaignIndex == CurrentCampaignSequence.numberOfChapters) {
                    mCurrentCampaignIndex = CurrentCampaignSequence.numberOfChapters - 1;
                    CurrentCampaignIsCompleted = true;
                }

                if (mCurrentCampaignIndex > FarthestCampaignIndex) {
                    FarthestCampaignIndex = mCurrentCampaignIndex;
                }
            }
        }

        private int mFarthestCampaignIndex;

        public int FarthestCampaignIndex {
            get { return mFarthestCampaignIndex; }
            private set {
                if (mFarthestCampaignIndex != value) {
                    mFarthestCampaignIndex = value;
                }
            }
        }

        public enum CampaignEvent {
            OpeningDialogue,
            CampaignMap,
            ClosingDialogue,
            SaveMenu,
            ChapterMenu
        }

        public enum CampaignDifficulty {
            Easy,
            Normal,
            Hard,
            Lunatic
        }

        private CampaignEvent currentCampaignEvent;

        public Player CampaignPlayerData { get; set; }
        public List<UnitWrapper> CampaignPlayerUnitData { get; set; }

        public List<CampaignData> CampaignDataSlots { get; private set; }
        public SortedDictionary<string, CampaignSequence> NamesToCampaignSequences { get; private set; }

        public bool CurrentCampaignSaved { get; private set; }

        public static string DataToString(CampaignData data) {
            var sequence = instance.NamesToCampaignSequences[data.CampaignName];
            var dataString = $"{sequence.campaignName}: Chapter {data.FarthestCampaignIndex + 1} - {sequence.chapterNames[data.FarthestCampaignIndex]} {data.TimeStamp}";
            
            if (data.IsCompleted) {
                dataString += " - COMPLETE";
            }

            return dataString;
        }

        private void Awake() {
            //Check if instance already exists
            if (instance == null) {
                //if not, set instance to this
                instance = this;
            }

            //If instance already exists and it's not this:
            else if (instance != this) {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }

        public void Start() {
            DifficultyToModifier = new Dictionary<CampaignDifficulty, float>() {
                { CampaignDifficulty.Easy,  EASY_MODIFIER},
                { CampaignDifficulty.Normal,  NORMAL_MODIFIER},
                { CampaignDifficulty.Hard, HARD_MODIFIER },
                { CampaignDifficulty.Lunatic, LUNATIC_MODIFIER }
            };

            NamesToCampaignSequences = new SortedDictionary<string, CampaignSequence>();
            foreach (var availableCampaign in availableCampaigns) {
                NamesToCampaignSequences.Add(availableCampaign.campaignName, availableCampaign);
            }

            CampaignDataSlots = CampaignDataFileHandler.LoadCampaignData();
        }

        public void LoadNextCampaignEvent() {

            // Load the next event given the current campaign event
            switch (currentCampaignEvent) {
                case CampaignEvent.OpeningDialogue:
                    LoadNextMap();
                    //LoadOpeningDialogue();
                    break;
                case CampaignEvent.CampaignMap:
                    CompleteCurrentCampaignMap();
                    LoadCampaignSaveMenu();
                    break;
                case CampaignEvent.ClosingDialogue:
                    CompleteCurrentCampaignMap();
                    LoadCampaignSaveMenu();
                    break;
                case CampaignEvent.SaveMenu:
                    LoadCampaignChapterMenu();
                    break;
                case CampaignEvent.ChapterMenu:
                    LoadNextMap();
                    break;
            }
        }

        public void StartNewCampaign(CampaignSequence newSequence, CampaignDifficulty campaignDifficulty) {
            CurrentCampaignSequence = newSequence;
            CurrentCampaignIndex = 0;
            FarthestCampaignIndex = 0;

            CampaignPlayerData = new Player("Hero", 1);
            CurrentCampaignDifficulty = campaignDifficulty;

            CurrentCampaignIsCompleted = false;
            // LoadOpeningDialogue();
            LoadNextMap();
        }

        public void LoadCampaign(CampaignData data) {
            CurrentCampaignSequence = NamesToCampaignSequences[data.CampaignName];
            CurrentCampaignIndex = data.CurrentCampaignIndex;
            FarthestCampaignIndex = data.FarthestCampaignIndex;
            CurrentCampaignIsCompleted = data.IsCompleted;
            CampaignPlayerData = data.PlayerData.Clone();
            CurrentCampaignDifficulty = data.Difficulty;

            // Regenerate Units with Skills
            CampaignPlayerData.CampaignUnits = new List<Unit>();
            foreach (var unitWrapper in data.UnitWrapperData) {
                var unit = UnitFactory.instance.GenerateUnit(unitWrapper);
                CampaignPlayerData.AddCampaignUnit(unit);
            }

            CampaignPlayerUnitData = CampaignPlayerData.CampaignUnits.Select(unit => new UnitWrapper(unit)).ToList();

            CampaignPlayerData.Weapons = new List<Weapon>();
            foreach (var weaponWrapper in data.WeaponWrapperData) {
                var weapon = WeaponFactory.instance.GenerateWeapon(weaponWrapper.WeaponName);
                CampaignPlayerData.Weapons.Add(weapon);
            }

            // TODO: Sebastian
            // Now you need to regenerate Items using the wrapper data of items that was saved in
            // Campaign Data. See above for reference!
            CampaignPlayerData.Items = new List<Item>();
            foreach (var itemWrapper in data.ItemWrapperData)
            {
                var item = ItemFactory.instance.GenerateItem(itemWrapper.ItemName);
                CampaignPlayerData.Items.Add(item);
            }

            LoadCampaignChapterMenu();
        }

        public void LoadCampaignChapterMenu() {
            currentCampaignEvent = CampaignEvent.ChapterMenu;
            SceneLoader.instance.GoToCampaignChapterMenu();
        }

        private void LoadNextMap() {
            currentCampaignEvent = CampaignEvent.CampaignMap;
            var nextOpeningDialogue = CurrentCampaignSequence.preMapDialogueSequence[CurrentCampaignIndex].text;
            var nextClosingDialogue = CurrentCampaignSequence.postMapDialogueSequence[CurrentCampaignIndex].text;
            var nextMap = CurrentCampaignSequence.mapSequence[CurrentCampaignIndex].name;
            CampaignPlayerData.Units = new List<Unit>();
            SceneLoader.SetParam(SceneLoader.LOAD_OPENING_DIALOGUE_PARAM, nextOpeningDialogue);
            SceneLoader.SetParam(SceneLoader.LOAD_CLOSING_DIALOGUE_PARAM, nextClosingDialogue);
            SceneLoader.SetParam(SceneLoader.LOAD_MAP_PARAM, nextMap);
            SceneLoader.SetParam(SceneLoader.GAME_TYPE_PARAM, GameManager.SINGLEPLAYER_GAME_TYPE);
            SceneLoader.SetParam(SceneLoader.SINGLEPLAYER_GAME_TYPE_PARAM, GameManager.CAMPAIGN_GAME_TYPE);
            SceneLoader.instance.GoToMap();
        }

        private void LoadOpeningDialogue() {
            //THIS IS WEHRE Matthews LOGIC COMES IN
            currentCampaignEvent = CampaignEvent.OpeningDialogue;
            var nextDialogue = CurrentCampaignSequence.preMapDialogueSequence[CurrentCampaignIndex].text;
            SceneLoader.SetParam(SceneLoader.LOAD_OPENING_DIALOGUE_PARAM, nextDialogue);
            SceneLoader.instance.GoToDialogue();
            //throw new NotImplementedException();
        }

        private void LoadClosingDialogue() {
            currentCampaignEvent = CampaignEvent.ClosingDialogue;
            var nextDialogue = CurrentCampaignSequence.postMapDialogueSequence[CurrentCampaignIndex].text;
            SceneLoader.SetParam(SceneLoader.LOAD_CLOSING_DIALOGUE_PARAM, nextDialogue);
            SceneLoader.instance.GoToDialogue();
            //throw new NotImplementedException();
        }

        public void LoadCampaignSaveMenu() {
            currentCampaignEvent = CampaignEvent.SaveMenu;
            SceneLoader.instance.GoToCampaignSaveMenu();
        }

        public void LoadCampaignEquipmentMenu() {
            SceneLoader.instance.GoToEquipmentMenu();
        }

        public void LoadCampaignShopMenu() {
            SceneLoader.instance.GoToShopMenu();
        }

        public CampaignData SaveNewCampaign() {
            var newData = new CampaignData(CurrentCampaignSequence.campaignName, CurrentCampaignIndex, FarthestCampaignIndex, CurrentCampaignIsCompleted, CampaignPlayerData);
            CampaignDataSlots.Add(newData);

            CampaignDataFileHandler.SaveCampaignData(newData);

            return newData;
        }

        public CampaignData SaveCampaign(CampaignData data) {

            // Overwrite existing data
            CampaignDataFileHandler.DeleteCampaignData(data);

            data.CampaignName = CurrentCampaignSequence.campaignName;
            data.CurrentCampaignIndex = CurrentCampaignIndex;
            data.FarthestCampaignIndex = FarthestCampaignIndex;
            data.IsCompleted = CurrentCampaignIsCompleted;
            data.PlayerData = CampaignPlayerData;
            data.Difficulty = CurrentCampaignDifficulty;
            data.UnitWrapperData = CampaignPlayerData.CampaignUnits.Select(unit => new UnitWrapper(unit)).ToList();
            data.WeaponWrapperData = CampaignPlayerData.Weapons.Select(weapon => new WeaponWrapper(weapon)).ToList();
            data.ItemWrapperData = CampaignPlayerData.Items.Select(item => new ItemWrapper(item)).ToList();

            CampaignDataFileHandler.SaveCampaignData(data);

            CurrentCampaignSaved = true;
            return data;
        }

        private void CompleteCurrentCampaignMap() {
            CurrentCampaignSaved = false;
            CurrentCampaignIndex++;
        }

        public void ModifyUnitStats(Unit unit) {
            var modifier = DifficultyToModifier[CurrentCampaignDifficulty];

            unit.Strength.Base = (int) (unit.Strength.Base * modifier);
            unit.Magic.Base = (int) (unit.Magic.Base * modifier);
            unit.Speed.Base = (int) (unit.Speed.Base * modifier);
            unit.Skill.Base = (int) (unit.Skill.Base * modifier);
            unit.Luck.Base = (int) (unit.Luck.Base * modifier);

            if (CurrentCampaignDifficulty != CampaignDifficulty.Lunatic) {
                unit.Defense.Base = (int) (unit.Defense.Base * modifier);
                unit.Resistance.Base = (int) (unit.Resistance.Base * modifier);
            }
        }
    }
}
