using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Important stuff")] 
    public int difficulty;
    public string activeSaveName = "";

    private void Start()
    {
        LoadSettingsData();
    }

    public void CreateSave(string selectedSaveName)
    {
        activeSaveName = selectedSaveName;
        SavesUI.Instance.instantiatedSavePrefab = Instantiate(SavesUI.Instance.savePrefab, SavesUI.Instance.contentPanel.transform);
        SavesUI.Instance.saveText = SavesUI.Instance.instantiatedSavePrefab.GetComponentInChildren<TMP_Text>();
        SaveSystem.CreateSave(selectedSaveName);
    }
    public void SaveGame()
    {
        SaveSystem.SaveGame(activeSaveName);
    }
    public void LoadSave(string selectedSaveName)
    {
        activeSaveName = selectedSaveName;
        GameSaveData gameSaveData = SaveSystem.LoadGame(selectedSaveName);
        
        
    }

    public void SaveSettings()
    {
        SaveSystem.SaveSettings();
    }
    public void LoadSettingsData()
    {
        SettingsData settingsData = SaveSystem.LoadSettings();
        
        SettingsUI.Instance.ChangeMasterVolume(settingsData.masterVolume);
        SettingsUI.Instance.musicVolume = settingsData.musicVolume;
    }
    
    
}
