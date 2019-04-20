using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            var fileName = data.TimeStamp.ToFileString();
            var encodedFileName = Encoding.UTF8.GetBytes(fileName);
            var encodedFileNameText = Convert.ToBase64String(encodedFileName);
            var filePath = $"{campaignFolderPath}{encodedFileNameText}.sav";

            try {
                Directory.CreateDirectory(campaignFolderPath);
            } catch(Exception e) {
                Debug.Log("Error creating folder for campaign save data");
                Debug.Log($"{e.Message}");
            }

            try {
                var writer = new StreamWriter(filePath, false);
                string json = JsonUtility.ToJson(data, true);
                var bytesToEncode = Encoding.UTF8.GetBytes(json);
                var encodedText = Convert.ToBase64String(bytesToEncode);
                writer.Write(encodedText);
                writer.Close();
            } catch(Exception e) {
                Debug.Log("Error saving campaign data");
                Debug.Log($"{e.Message}");
            }

        }

        public static List<CampaignData> LoadCampaignData() {
            var loadedCampaignData = new List<CampaignData>();

            try {
                foreach (var campaignFile in Directory.EnumerateFiles(campaignFolderPath, "*.sav")) {
                    var campaignDataFileContents = File.ReadAllText(campaignFile);
                    var decodedBytes = Convert.FromBase64String(campaignDataFileContents);
                    var decodedText = Encoding.UTF8.GetString(decodedBytes);
                    var campaignData = JsonUtility.FromJson<CampaignData>(decodedText);
                    loadedCampaignData.Add(campaignData);
                }
            } catch {
                Debug.Log($"Error reading directory");
            }

            return loadedCampaignData;
        }

        public static void DeleteCampaignData(CampaignData data) {
            var fileName = data.TimeStamp.ToFileString();
            var encodedFileName = Encoding.UTF8.GetBytes(fileName);
            var encodedFileNameText = Convert.ToBase64String(encodedFileName);
            var filePath = $"{campaignFolderPath}{encodedFileNameText}.sav";
            try {
                var fileInfo = new FileInfo(filePath);
                File.Delete(filePath);
            } catch {
                Debug.Log($"Error deleting file {filePath}");
            }
        }
    }
}
