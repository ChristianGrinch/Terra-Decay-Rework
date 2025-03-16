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
    public int masterVolume;
    public int musicVolume;
    public SettingsData settingsData;
    private SettingsData editedSettings;
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
        throw new System.NotImplementedException();
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

    private void ChangeMasterVolume(float value)
    {
        didChangeSetting = true;
        masterVolume = (int)value;
        // MusicPlayer.Instance.audioSource.volume = value/100f;
        ChangeMusicVolume(musicVolumeSlider.value);
        masterVolumeText.text = value + "%";
    }

    private void ChangeMusicVolume(float value)
    {
        didChangeSetting = true;
        musicVolume = (int)value;
        masterVolume = (int)masterVolumeSlider.value;
        MusicPlayer.Instance.audioSource.volume = (musicVolume / 100f)*(masterVolume / 100f);
        musicVolumeText.text = musicVolume + "%";
    }
}
