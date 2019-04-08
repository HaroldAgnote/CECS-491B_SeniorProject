using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using Assets.Scripts.Campaign;
using Assets.Scripts.Utilities.DateTime;

namespace Assets.Scripts.Utilities.FileHandling {
    public static class CampaignDataFileHandler {
        public static string HOME_FOLDER = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public static string CAMPAIGN_SAVE_DATA_FILE_PATH = @"\Documents\HeraldOfANewEra\CampaignSaveData\";

        public static string campaignFolderPath = $"{HOME_FOLDER}{CAMPAIGN_SAVE_DATA_FILE_PATH}";

        public static void SaveCampaignData(CampaignData data) {
            data.TimeStamp = new SerializableDateTime(System.DateTime.Now);
            var filePath = $"{campaignFolderPath}{data.TimeStamp.ToFileString()}.txt";

            try {
                Directory.CreateDirectory(campaignFolderPath);
            } catch(Exception e) {
                Debug.Log("Error creating folder for campaign save data");
                Debug.Log($"{e.Message}");
            }

            try {
                var writer = new StreamWriter(filePath, false);
                string json = JsonUtility.ToJson(data, true);
                writer.Write(json);
                writer.Close();
            } catch(Exception e) {
                Debug.Log("Error saving campaign data");
                Debug.Log($"{e.Message}");
            }

        }

        public static List<CampaignData> LoadCampaignData() {
            var loadedCampaignData = new List<CampaignData>();

            try {
                foreach (var campaignFile in Directory.EnumerateFiles(campaignFolderPath, "*.txt")) {
                    var campaignDataFileContents = File.ReadAllText(campaignFile);
                    var campaignData = JsonUtility.FromJson<CampaignData>(campaignDataFileContents);
                    loadedCampaignData.Add(campaignData);
                }
            } catch {
                Debug.Log($"Error reading directory");
            }

            return loadedCampaignData;
        }

        public static void DeleteCampaignData(CampaignData data) {
            var filePath = $"{campaignFolderPath}{data.TimeStamp.ToFileString()}.txt";
            try {
                var fileInfo = new FileInfo(filePath);
                File.Delete(filePath);
            } catch {
                Debug.Log($"Error deleting file {filePath}");
            }
        }
    }
}
