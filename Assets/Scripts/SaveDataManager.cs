using System.Collections.Generic;
using UnityEngine;

public static class SaveDataManager
{
    private static SaveData data;
    private const string NameSavePrefs = "data";

    public static bool IsFirstGameStarted => PlayerPrefs.HasKey(NameSavePrefs);
    public static List<ItemColor> GetAllSavedColors => GetSaveData().SavedColor;

    private static SaveData GetSaveData()
    {
        if (data == null)
        {
            if (PlayerPrefs.HasKey(NameSavePrefs))
                data = JsonUtility.FromJson<SaveData>(PlayerPrefs.GetString(NameSavePrefs));
            else
                data = new SaveData();
        }
        return data;
    }
    
    public static void SaveData(List<ItemColor> list)
    {
        data.SavedColor = list;
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(NameSavePrefs, json);
    }
}