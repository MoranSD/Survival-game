using UnityEngine;

namespace Utils
{
	public class ProfileData
	{
        private static ProfileData _instance;
		public static ProfileData Instance
        {
            get
            {
                if (_instance == null) _instance = new ProfileData();

                return _instance;
            }
        }

        private ProfileData()
        {

        }
        //переделать под Json формат
        public T GetData<T>(string key)
        {
            string jsonDataString = PlayerPrefs.GetString(key);

            T convertedData = JsonUtility.FromJson<T>(jsonDataString);

            return convertedData;
        }
        public void SaveData<T>(string key, T data)
        {
            string jsonDataString = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, jsonDataString);
        }
	}
}
