using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Application;

namespace Assets.Scripts.Campaign {
    public class CampaignManager : MonoBehaviour {
        public static CampaignManager instance;

        public CampaignSequence campaign;

        public bool CurrentCampaignIsFinished {
            get { return campaign.IsCampaignOver; }
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

        void Start() {
            campaign.RestartCampaign();
        }

        public void RestartCampaign() {
            campaign.RestartCampaign();
        }

        public void LoadNextMap() {
            var nextMap = campaign.CurrentMap.name;
            SceneLoader.SetParam(SceneLoader.LOAD_MAP_PARAM, nextMap);
            SceneLoader.SetParam(SceneLoader.GAME_TYPE, GameManager.SINGLEPLAYER_GAME_TYPE);
            SceneLoader.SetParam(SceneLoader.SINGLEPLAYER_GAME_TYPE, GameManager.CAMPAIGN_GAME_TYPE);
            SceneLoader.instance.GoToMap();
        }

        public void CompleteCurrentCampaignMap() {
            campaign.UpdateCampaign();
        }
    }
}
