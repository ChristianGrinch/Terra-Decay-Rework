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
    public bool IsSaveNameValid(string saveName)
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

    public bool DoesSaveNameExist(string saveName)
    {
        return File.Exists(Application.persistentDataPath + "/" + saveName + ".svf");
    }
    public void CreateSave(string selectedSaveName)
    {
        if (DoesSaveNameExist(selectedSaveName))
        {
            SavesUI.Instance.illegalSaveNameWarningText.gameObject.SetActive(true);
            return;
        }
        SavesUI.Instance.illegalSaveNameWarningText.gameObject.SetActive(false);
            
        if (IsSaveNameValid(selectedSaveName))
        {
            activeSaveName = selectedSaveName;
            SavesUI.Instance.CreateSaveButton(selectedSaveName);
            SaveSystem.CreateSave(selectedSaveName);
        }
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
        SavesUI.Instance.defaultSVFName = settingsData.defaultSVFName;
        StartUI.Instance.playBtn.GetComponentInChildren<TMP_Text>().text = $"Play '{settingsData.defaultSVFName}'";
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
