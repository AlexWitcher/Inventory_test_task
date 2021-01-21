using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace TestTask.API
{
    public class SavesManager
    {
        private const string SAVE_KEY = "backpack";

        public void SaveData(Dictionary<int, int> data)
        {
            PlayerPrefs.SetString(SAVE_KEY,JsonConvert.SerializeObject(data));
        }

        public Dictionary<int, int> LoadData()
        {
            var savedString = PlayerPrefs.GetString(SAVE_KEY);

            if (string.IsNullOrEmpty(savedString))
                return null;

            try
            {
                return JsonConvert.DeserializeObject<Dictionary<int, int>>(savedString);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }      
        }
    }
}
