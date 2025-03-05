using UnityEngine;

namespace CoreLib
{
    public static class ClientSaveData
    {
        public static string Language
        {
            get => GetData("Language", "Korean");
            set => SetData("Language", value);
        }
    
        public static float BGMVolume
        {
            get => GetData("BGMVolume", 1f);
            set => SetData("BGMVolume", value);
        }
    
        public static float SFXVolume
        {
            get => GetData("SFXVolume", 1f);
            set => SetData("SFXVolume", value);
        }
    
        public static int FrameRate
        {
            get => GetData("FrameRate", 60);
            set => SetData("FrameRate", value);
        }
    
        public static int StageProgress
        {
            get => GetData("StageProgress", 0);
            set => SetData("StageProgress", value);
        }
    
        private static string GetData(string key, string defVal)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetString(key);
            }
            else
            {
                PlayerPrefs.SetString(key, defVal);
            }
        
            return defVal;
        }
    
        private static void SetData(string key, string val)
        {
            PlayerPrefs.SetString(key, val);
        }
    
        private static float GetData(string key, float defVal)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetFloat(key);
            }
            else
            {
                PlayerPrefs.SetFloat(key, defVal);
            }
        
            return defVal;
        }
    
        private static void SetData(string key, float val)
        {
            PlayerPrefs.SetFloat(key, val);
        }
    
        private static int GetData(string key, int defVal)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key);
            }
            else
            {
                PlayerPrefs.SetInt(key, defVal);
            }
        
            return defVal;
        }
    
        private static void SetData(string key, int val)
        {
            PlayerPrefs.SetInt(key, val);
        }
    }
}
