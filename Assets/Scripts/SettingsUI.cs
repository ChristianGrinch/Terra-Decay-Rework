using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [Header("Panels")]
    public GameObject audioPanel;
    public GameObject videoPanel;
    [Header("Audio")]
    public Slider masterVolumeSlider;
    public TMP_Text  masterVolumeText;
    public Slider musicVolumeSlider;
    public TMP_Text  musicVolumeText;
    [Header("Other")]
    public GameObject currentPanel;
    public TMP_Text changedSettingsText;

    public bool didChangeSetting;
    public bool didSaveChanges;
    
    private void Start()
    {
        goBackBtn.onClick.AddListener(GoBack);
        audioBtn.onClick.AddListener(() => OpenPanel(audioPanel));
        videoBtn.onClick.AddListener(() =>  OpenPanel(videoPanel));
        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
    }

    private void GoBack()
    {
        if(!didChangeSetting) UIManager.Instance.GoBack();
        if (didChangeSetting && !didSaveChanges) OpenSaveCheck();
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
}
