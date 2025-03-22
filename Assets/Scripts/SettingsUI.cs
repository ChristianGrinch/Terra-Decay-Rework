using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    public Dictionary<Keys, KeyCode> ReturnDefaultControlKeys()
    {
        return new Dictionary<Keys, KeyCode> {
            {Keys.GoBack, KeyCode.Escape},
        };
    }
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
    public Button controlsBtn;
    [Header("Panels")]
    public GameObject audioPanel;
    public GameObject videoPanel;
    public GameObject controlsPanel;
    public GameObject savePanel;
    public GameObject currentPanel;
    [Header("Audio")]
    public Slider masterVolumeSlider;
    public TMP_Text  masterVolumeText;
    public Slider musicVolumeSlider;
    public TMP_Text  musicVolumeText;
    [Header("Video")]
    public TMP_Dropdown overallQualityDropdown;

    [Header("Controls")]
    public Dictionary<Keys, KeyCode> controlKeys;
    public GameObject controlPanelContent;
    public GameObject controlObjectPrefab;
    public TMP_Text keysFunctionText;
    public TMP_Text keyUsedText;
    
    [Header("Other")]
    public bool didChangeSetting;
    
    private void Start()
    {
        InitializeControls();
        
        goBackBtn.onClick.AddListener(GoBack);
        audioBtn.onClick.AddListener(() => OpenPanel(audioPanel));
        videoBtn.onClick.AddListener(() =>  OpenPanel(videoPanel));
        controlsBtn.onClick.AddListener(() => OpenPanel(controlsPanel));
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
    // Thanks to stackoverflow for this extension method!
    // https://stackoverflow.com/questions/5796383/insert-spaces-between-words-on-a-camel-cased-token
    private static string SplitCamelCase(string str) 
    {
        return Regex.Replace( 
            Regex.Replace( 
                str, 
                @"(\P{Ll})(\P{Ll}\p{Ll})", 
                "$1 $2" 
            ), 
            @"(\p{Ll})(\P{Ll})", 
            "$1 $2" 
        );
    }
   
    private void InitializeControls()
    {
        controlKeys = ReturnDefaultControlKeys();
        foreach (var control in controlKeys)
        {
            string key = SplitCamelCase(control.Key.ToString());
            string keyCode = control.Value.ToString();
            
            GameObject controlPanel = Instantiate(controlObjectPrefab, controlPanelContent.transform);
            keysFunctionText = controlPanel.GetComponentInChildren<TMP_Text>();
            Button button = controlPanel.GetComponentInChildren<Button>();
            keyUsedText = button.gameObject.GetComponentInChildren<TMP_Text>();
            button.onClick.AddListener(() =>
            {
                keyUsedText.text = "Waiting for input...";
                StartCoroutine(WaitForKey(control.Key));
            });
            
            keysFunctionText.text = key;
            keyUsedText.text = keyCode;
        }
    }
    // Completely made with ChatGPT. Thanks!
    private IEnumerator WaitForKey(Keys controlKey)
    {
        bool keyDetected = false;
        while (!keyDetected)
        {
            yield return null;
            foreach (KeyCode keyDown in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyDown))
                {
                    controlKeys[controlKey] = keyDown;
                    keyUsedText.text = keyDown.ToString(); // Update UI
                    keyDetected = true;
                    didChangeSetting = true;
                    break;
                }
            }
        }
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
    }
}
