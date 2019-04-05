﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Assets.Scripts.Model;
using Assets.Scripts.Utilities.DateTime;

namespace Assets.Scripts.Campaign {

    [Serializable]
    public class CampaignData : IComparable<CampaignData> {
        
        [SerializeField]
        private string mCampaignName;

        public string CampaignName {
            get { return mCampaignName; }
            set {
                if (mCampaignName != value) {
                    mCampaignName = value;
                }
            }
        }

        [SerializeField]
        private bool mIsCompleted;

        public bool IsCompleted {
            get { return mIsCompleted; }
            set {
                if (mIsCompleted != value) {
                    mIsCompleted = value;
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
            set {
                if (mFarthestCampaignIndex != value) {
                    mFarthestCampaignIndex = value;
                }
            }
        }

        [SerializeField]
        private Player mPlayerData;

        public Player PlayerData {
            get { return mPlayerData; }
            set {
                if (mPlayerData != value) {
                    mPlayerData = value;
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
            mCampaignName = campaignName;
            mCurrentCampaignIndex = 0;
            mFarthestCampaignIndex = 0;
            mIsCompleted = false;
            mPlayerData = new Player("Hero");
        }

        public CampaignData(string campaignName, int currentIndex) {
            mCampaignName = campaignName;
            mCurrentCampaignIndex = currentIndex;
        }

        public CampaignData(string campaignName, int currentIndex, int farthestIndex) {
            mCampaignName = campaignName;
            mCurrentCampaignIndex = currentIndex;
            mFarthestCampaignIndex = farthestIndex;
        }

        public CampaignData(string campaignName, int currentIndex, int farthestIndex, bool isCompleted, Player playerData) {
            mCampaignName = campaignName;
            mCurrentCampaignIndex = currentIndex;
            mFarthestCampaignIndex = farthestIndex;
            mIsCompleted = isCompleted;
            mPlayerData = playerData;
        }

        public int CompareTo(CampaignData other) {
            return this.mTimeStamp.CompareTo(other.mTimeStamp);
        }
    }
}