using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Unity;
using UnityEngine;

namespace Assets.Scripts.Campaign {
    public class CampaignSequence : MonoBehaviour {

        public string campaignName;
        public List<TextAsset> mapSequence;
        public List<TextAsset> preMapDialogueSequence;
        public List<TextAsset> postMapDialogueSequence;

        private int currentCampaignIndex;

        public void LoadFromFile(string fileName) {
            throw new NotImplementedException();
        }

        public TextAsset CurrentMap {
            get { return mapSequence[currentCampaignIndex]; }
        }

        public TextAsset CurrentPreMapDialogue {
            get { return preMapDialogueSequence[currentCampaignIndex]; }
        }

        public TextAsset CurrentPostMapDialogue {
            get { return postMapDialogueSequence[currentCampaignIndex]; }
        }

        public void RestartCampaign() {
            currentCampaignIndex = 0;
        }

        public bool IsCampaignOver {
            get { return currentCampaignIndex == mapSequence.Count; }
        }

        public void UpdateCampaign() {
            currentCampaignIndex++;
        }
    }
}
