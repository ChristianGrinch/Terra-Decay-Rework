using UnityEngine;
using System.IO;
using MessagePack;
using System.Collections.Generic;

// .svf for SaVeFile
// .dsvf for Default SaVeFile
// .ssvf for Settings SaVeFile

public static class SaveSystem
{
	public static void SaveGame(string saveName)
	{
		GameSaveData gameSaveData = GameSaveData.FetchSaveData();
		byte[] bytes = MessagePackSerializer.Serialize(gameSaveData);
        string path = Path.Combine(Application.persistentDataPath, saveName + ".svf");


        File.WriteAllBytes(path, bytes);
		Debug.Log("Saved file with length: " + bytes.Length + " bytes.");
	}
    public static void CreateSave(string saveName)
    {
        GameSaveData gameSaveData = GameSaveData.CreateDefaultData();
        byte[] bytes = MessagePackSerializer.Serialize(gameSaveData);
        string path = Path.Combine(Application.persistentDataPath, saveName + ".svf");

        File.WriteAllBytes(path, bytes);
        Debug.Log("Created new save file with length: " + bytes.Length + " bytes.");
    }
	public static GameSaveData LoadGame(string saveName)
	{
        string path = Path.Combine(Application.persistentDataPath, saveName + ".svf");


        if (File.Exists(path))
		{
			
			byte[] readBytes = File.ReadAllBytes(path);
			GameSaveData data = MessagePackSerializer.Deserialize<GameSaveData>(readBytes);

			Debug.Log("Loaded file with length: " + readBytes.Length + " bytes.");
			return data;
		}
		else
		{
			Debug.LogError("Save file not found in " + path);
			return null;
		}
	}

    public static List<string> FindSaves()
    {
        string path = Application.persistentDataPath;
        //Debug.Log($"Searching in path: {path}");
        string[] files = Directory.GetFiles(path, "*.svf");

        //Debug.Log($"Files found: {files.Length}");

        if (files.Length == 0)
        {
            Debug.Log("No save files found.");
            return null;
        }

        List<string> saveFileNames = new List<string>();

        foreach (string filePath in files)
        {
	        if (File.Exists(filePath))
	        {
		        string saveName = Path.GetFileNameWithoutExtension(filePath);
		        saveFileNames.Add(saveName);

		        //Debug.Log("Found save file: " + saveName);
	        }
        }

        return saveFileNames;
    }

    public static bool FindSavesBool(string saveName)
    {
        string path = Application.persistentDataPath;
        string[] files = Directory.GetFiles(path, "*.svf");

        if (files.Length == 0)
        {
            Debug.Log("No save files found.");
            return false;
        }

        foreach (string filePath in files)
        {
            if (File.Exists(filePath))
            {
                string existingSaveName = Path.GetFileNameWithoutExtension(filePath);
                if(existingSaveName == saveName)
                {
                    Debug.Log("Found save file: " + existingSaveName);
                    return true;
                }

                
            }
        }
        Debug.Log("Save file not found: " + saveName);
        return false;
    }

    public static void DeleteSave(string saveName)
    {
        string path = Path.Combine(Application.persistentDataPath, saveName + ".svf");

        if (File.Exists(path))
        {
            File.Delete(path);
        }
        else
        {
            Debug.LogError("File does not exist! Cannot delete a nonexistent file.");
        }
    }

    public static void SetDefaultSave(string saveName)
    {
        string path = Path.Combine(Application.persistentDataPath, "Default" + ".dsvf"); // Default SaVeFile

        byte[] bytes = MessagePackSerializer.Serialize(saveName);

        File.WriteAllBytes(path, bytes);
        Debug.Log("Saved Default save name to unique save file.");
    }
    public static string LoadDefaultSave()
    {
        string path = Path.Combine(Application.persistentDataPath, "Default" + ".dsvf");

        if (File.Exists(path))
        {
            byte[] readBytes = File.ReadAllBytes(path);
            string defaultSaveName = MessagePackSerializer.Deserialize<string>(readBytes);
			if (FindSavesBool(defaultSaveName))
			{
				return defaultSaveName;
			}
			Debug.LogWarning("No default save assigned!");
			return null;
        }

        Debug.LogWarning("No default save assigned!");
        return null;
    }
	public static void SaveSettings()
	{
		string path = Path.Combine(Application.persistentDataPath, "Settings" + ".ssvf"); // Settings SaVeFile

		byte[] bytes = MessagePackSerializer.Serialize(SettingsData.FetchSettingsData());

		File.WriteAllBytes(path, bytes);
		Debug.Log("Saved .ssvf");
	}
	public static SettingsData LoadSettings()
	{
		string path = Path.Combine(Application.persistentDataPath, "Settings" + ".ssvf");

		if (File.Exists(path))
		{
			byte[] readBytes = File.ReadAllBytes(path);
			SettingsData data = MessagePackSerializer.Deserialize<SettingsData>(readBytes);
			Debug.Log("Loaded .ssvf");
			return data;
		}
		
		Debug.LogWarning("No .ssvf file detected! Creating a new one.");
		return CreateSettingsSave();
	}
	public static SettingsData CreateSettingsSave()
	{
		string path = Path.Combine(Application.persistentDataPath, "Settings" + ".ssvf"); // Settings SaVeFile

		byte[] bytes = MessagePackSerializer.Serialize(SettingsData.CreateDefaultSettingsData());

		File.WriteAllBytes(path, bytes);
		Debug.Log("Created .ssvf");
		return MessagePackSerializer.Deserialize<SettingsData>(bytes);
	}
}
