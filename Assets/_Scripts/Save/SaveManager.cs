using System;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static Save _save;
    private const string FILE_NAME = "Save.txt";
    private const string CRYPTING_KEY = "Caracole";
    public static bool IsSaveBlank;
    public static event Action SaveLoadedEvent;

    #region SAVE FIELDS ACCESSORS AND MUTATORS

    public static string GetPlayerName()
    {
        return _save.LocalPlayerName;
    }

    public static void SetPlayerName(string playerName)
    {
        _save.LocalPlayerName = playerName;
        IsSaveBlank = false;
    }

    #endregion

    public static void GetSave()
    {
        try
        {
            string encryptedData = File.ReadAllText($"{Application.persistentDataPath}/{FILE_NAME}");
            string decryptedData = DecryptData(encryptedData);
            _save = JsonUtility.FromJson<Save>(decryptedData);
            IsSaveBlank = false;
        }
        catch
        {
            _save = new Save();
            IsSaveBlank = true;
        }

        SaveLoadedEvent.Invoke();

        string DecryptData(string encryptedData)
        {
            string result = "";

            int keyPosition = 0;
            foreach (char c in encryptedData)
            {
                result += (char)((int)c - (int)CRYPTING_KEY[keyPosition]);
                keyPosition = (keyPosition + 1) % CRYPTING_KEY.Length;
            }

            return result;
        }
    }

    public static void SaveData()
    {
        string rawData = JsonUtility.ToJson(_save);
        string encryptedData = EncryptData(rawData);
        StreamWriter streamWriter = new StreamWriter($"{Application.persistentDataPath}/{FILE_NAME}");
        streamWriter.Write(encryptedData);
        streamWriter.Close();

        string EncryptData(string rawData)
        {
            string result = "";

            int keyPosition = 0;
            foreach (char c in rawData)
            {
                result += (char)((int)c + (int)CRYPTING_KEY[keyPosition]);
                keyPosition = (keyPosition + 1) % CRYPTING_KEY.Length;
            }

            return result;
        }
    }
}
