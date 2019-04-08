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
        public int numberOfChapters;
        public List<string> chapterNames;
        public List<TextAsset> mapSequence;
        public List<TextAsset> preMapDialogueSequence;
        public List<TextAsset> postMapDialogueSequence;

        void Start() {
            if (numberOfChapters != chapterNames.Count) {
                Debug.Log("Number of Chapters does not match Chapter Names");
            }

            if (numberOfChapters != mapSequence.Count) {
                Debug.Log("Number of Chapters does not match Map Sequences");
            }

            if (numberOfChapters != preMapDialogueSequence.Count) {
                Debug.Log("Number of Chapters does not match Pre map dialogue scenes");
            }

            if (numberOfChapters != postMapDialogueSequence.Count) {
                Debug.Log("Number of Chapters does not match Post map dialogue scenes");
            }
        }
    }
}
