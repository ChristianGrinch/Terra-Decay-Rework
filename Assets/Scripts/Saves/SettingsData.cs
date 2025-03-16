using UnityEngine;
using MessagePack;

public class SettingsData
{
    // Settings Data (Keys 0-100)
    [Key(0)] public int masterVolume;
    [Key(1)] public int musicVolume;
    
    public static SettingsData FetchSettingsData()
    {
        return new SettingsData
        {
            masterVolume = (int)SettingsUI.Instance.musicVolumeSlider.value,
            musicVolume = (int)SettingsUI.Instance.musicVolumeSlider.value,
        };
    }
    public static SettingsData CreateDefaultSettingsData()
    {
        return new SettingsData
        {
            masterVolume = 100,
            musicVolume = 100,
        };
    }
}
