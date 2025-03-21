using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum Keys
{
    GoBack,
}
public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance {get; private set;}
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
    [Header("Buttons")] 
    public Button goBackBtn;
    public Button audioBtn;
    public Button videoBtn;
    public Button saveBtn;
    [Header("Panels")]
    public GameObject audioPanel;
    public GameObject videoPanel;
    public GameObject currentPanel;
    public GameObject savePanel;
    [Header("Audio")]
    public Slider masterVolumeSlider;
    public TMP_Text  masterVolumeText;
    public Slider musicVolumeSlider;
    public TMP_Text  musicVolumeText;
    [Header("Video")]
    public TMP_Dropdown overallQualityDropdown;

    [Header("Controls")]
    public Dictionary<Keys, KeyCode> controlKeys;
    
    [Header("Other")]
    public bool didChangeSetting;
    
    private void Start()
    {
        InitializeControls();
        goBackBtn.onClick.AddListener(GoBack);
        audioBtn.onClick.AddListener(() => OpenPanel(audioPanel));
        videoBtn.onClick.AddListener(() =>  OpenPanel(videoPanel));
        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        saveBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.SaveSettings();
            didChangeSetting = false;
        });
        overallQualityDropdown.onValueChanged.AddListener(ChangeOverallQuality);
    }

    private void Update()
    {
        savePanel.SetActive(didChangeSetting);
    }

    private void InitializeControls()
    {
        controlKeys = new Dictionary<Keys, KeyCode> {
            {Keys.GoBack, KeyCode.Escape},
        };
    }
    private void GoBack()
    {
        if (!didChangeSetting)
        {
            UIManager.Instance.GoBack();
        }
        else
        {
            OpenSaveCheck();
        }
    }
    
    private void OpenSaveCheck()
    {
        PopupManager.Instance.OpenPopup(Popup.QuitWithoutSaving);
    }
    private void OpenPanel(GameObject panel)
    {
        if(currentPanel != null) ClosePanel();
        currentPanel = panel;
        currentPanel.SetActive(true);
    }
    private void ClosePanel()
    {
        currentPanel.SetActive(false);
        currentPanel = null;
    }

    public void ChangeMasterVolume(float value)
    {
        if(Time.time > 1) didChangeSetting = true; // Makes sure not to enable didChangeSetting when loading volume settings at game start.
        
        masterVolumeText.text = value + "%";
        masterVolumeSlider.value = value;
        AudioManager.Instance.UpdateMasterVolume((int)value);
    }

    public void ChangeMusicVolume(float value)
    {
        if(Time.time > 1) didChangeSetting = true;
        
        musicVolumeText.text = value + "%";
        musicVolumeSlider.value = value;
        AudioManager.Instance.UpdateMusicVolume((int)value);
    }

    private void ChangeOverallQuality(int value)
    {
        if(Time.time > 1) didChangeSetting = true;

        throw new NotImplementedException();
    }
}
