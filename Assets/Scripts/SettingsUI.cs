using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
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
    private void Start()
    {
        goBackBtn.onClick.AddListener(() => UIManager.Instance.GoBack());
        audioBtn.onClick.AddListener(() => OpenPanel(audioPanel));
        videoBtn.onClick.AddListener(() =>  OpenPanel(videoPanel));
        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
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
        // MusicPlayer.Instance.audioSource.volume = value/100f;
        ChangeMusicVolume(musicVolumeSlider.value);
        masterVolumeText.text = value + "%";
    }

    private void ChangeMusicVolume(float value)
    {
        int musicVolume = (int)value;
        int masterVolume = (int)masterVolumeSlider.value;
        MusicPlayer.Instance.audioSource.volume = (musicVolume / 100f)*(masterVolume / 100f);
        musicVolumeText.text = musicVolume + "%";
    }
}
