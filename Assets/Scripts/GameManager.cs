using System;
using System.IO;
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
    public bool completedFirstSSVFLoad;

    private void Start()
    {
        LoadSettingsData();
    }
    public static bool IsSaveNameValid(string saveName)
    {
        Debug.Log("IsSaveNameValid: " + saveName);
        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char c in invalidChars)
        {
            if (saveName.Contains(c.ToString()) || saveName == "")
            {
                return false;
            }
        }
        return true;
    }
    public void CreateSave(string selectedSaveName)
    {
        if (IsSaveNameValid(selectedSaveName))
        {
            SavesUI.Instance.illegalCharactersWarningText.gameObject.SetActive(false);
            activeSaveName = selectedSaveName;
            SavesUI.Instance.CreateSaveButton(selectedSaveName);
            SaveSystem.CreateSave(selectedSaveName);
        }
        SavesUI.Instance.illegalCharactersWarningText.gameObject.SetActive(true);
    }
    public void SaveGame()
    {
        SaveSystem.SaveGame(activeSaveName);
    }
    public void LoadSave(string selectedSaveName)
    {
        activeSaveName = selectedSaveName;
        GameSaveData gameSaveData = SaveSystem.LoadGame(selectedSaveName);
        
        throw new NotImplementedException();
    }

    public void SaveSettings()
    {
        SaveSystem.SaveSettings();
    }
    public void LoadSettingsData()
    {
        SettingsData settingsData = SaveSystem.LoadSettings();
        
        AudioSettings.Instance.ChangeMasterVolume(settingsData.masterVolume);
        AudioSettings.Instance.ChangeMusicVolume(settingsData.musicVolume);
        VideoSettings.Instance.overallQualityDropdown.value = settingsData.overallQuality;
        ControlsSettings.Instance.controlKeys = settingsData.controlKeys;
        switch (completedFirstSSVFLoad)
        {
            case true:
            {
                foreach(Transform child in ControlsSettings.Instance.controlPanelContent.transform)
                {
                    Destroy(child.gameObject);
                }
                StartCoroutine(ControlsSettings.Instance.InitializeControls());
                break;
            }
            case false:
                completedFirstSSVFLoad = true;
                break;
        }
    }
    
}
