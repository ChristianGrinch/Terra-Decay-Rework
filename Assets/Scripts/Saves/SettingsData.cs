using System.Collections.Generic;
using UnityEngine;
using MessagePack;

[MessagePackObject]
public class SettingsData
{
    private static SettingsUI settingsUI =  SettingsUI.Instance;
    // Settings Data (Keys 0-100)
    // Audio (Keys 0-19)
    [Key(0)] public int masterVolume;
    [Key(1)] public int musicVolume;
    // Video (Keys 20-39)
    [Key(20)] public int overallQuality;
    // Controls (Keys 40-69)
    
    public static SettingsData FetchSettingsData()
    {
        return new SettingsData
        {
            masterVolume = (int)settingsUI.masterVolumeSlider.value,
            musicVolume = (int)settingsUI.musicVolumeSlider.value,
            overallQuality = settingsUI.overallQualityDropdown.value,
        };
    }
    public static SettingsData CreateDefaultSettingsData()
    {
        return new SettingsData
        {
            masterVolume = 100,
            musicVolume = 100,
            overallQuality = 3,
        };
    }
}
