using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public static AudioSettings Instance {get; private set;}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [Header("Audio")]
    public Slider masterVolumeSlider;
    public TMP_Text  masterVolumeText;
    public Slider musicVolumeSlider;
    public TMP_Text  musicVolumeText;

    private void Start()
    {
        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
    }
    public void ChangeMasterVolume(float value)
    {
        if(Time.time > 1) SettingsUI.Instance.didChangeSetting = true; // Makes sure not to enable didChangeSetting when loading volume settings at game start.
        
        masterVolumeText.text = value + "%";
        masterVolumeSlider.value = value;
        AudioManager.Instance.UpdateMasterVolume((int)value);
    }

    public void ChangeMusicVolume(float value)
    {
        if(Time.time > 1) SettingsUI.Instance.didChangeSetting = true;
        
        musicVolumeText.text = value + "%";
        musicVolumeSlider.value = value;
        AudioManager.Instance.UpdateMusicVolume((int)value);
    }

}