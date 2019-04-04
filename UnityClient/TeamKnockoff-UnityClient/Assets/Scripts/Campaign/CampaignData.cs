using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Utilities.DateTime;

namespace Assets.Scripts.Campaign {

    [Serializable]
    public class CampaignData {
        
        [SerializeField]
        private string mCampaignName;

        public string CampaignName {
            get { return mCampaignName; }
            private set {
                if (mCampaignName != value) {
                    mCampaignName = value;
                }
            }
        }

        [SerializeField]
        // This keeps track of the last campaign map played in the save data
        private int mCurrentCampaignIndex;

        public int CurrentCampaignIndex {
            get { return mCurrentCampaignIndex; }
            set {
                if (mCurrentCampaignIndex != value) {
                    mCurrentCampaignIndex = value;
                }

                if (mCurrentCampaignIndex > mFarthestCampaignIndex) {
                    FarthestCampaignIndex = mCurrentCampaignIndex;
                }
            }
        }


        [SerializeField]
        // This keeps track of the furthest campaign map played in the save data
        private int mFarthestCampaignIndex;

        public int FarthestCampaignIndex {
            get { return mFarthestCampaignIndex; }

            private set {
                if (mFarthestCampaignIndex != value) {
                    mFarthestCampaignIndex = value;
                }
            }
        }

        [SerializeField]
        private SerializableDateTime mTimeStamp;

        public SerializableDateTime TimeStamp {
            get { return mTimeStamp; }
            set {
                if (mTimeStamp != value) {
                    mTimeStamp = value;
                }
            }
        }

        public CampaignData(string campaignName) {
            CampaignName = campaignName;
            CurrentCampaignIndex = 0;
            FarthestCampaignIndex = 0;
        }

        public CampaignData(string campaignName, int currentIndex) {
            CampaignName = campaignName;
            CurrentCampaignIndex = currentIndex;
        }

        public CampaignData(string campaignName, int currentIndex, int farthestIndex) {
            CampaignName = campaignName;
            CurrentCampaignIndex = currentIndex;
            FarthestCampaignIndex = farthestIndex;
        }
    }
}
