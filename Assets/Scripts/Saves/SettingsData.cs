using System.Collections.Generic;
using UnityEngine;
using MessagePack;

[MessagePackObject]
public class SettingsData
{
    // Settings Data (Keys 0-100)
    // Audio (Keys 0-19)
    [Key(0)] public int masterVolume;
    [Key(1)] public int musicVolume;
    // Video (Keys 20-39)
    [Key(20)] public int overallQuality;
    // Controls (Keys 40-69)
    [Key(40)] public Dictionary<Keys, KeyCode> controlKeys;
    // Other Data (Keys > 100)
    [Key(101)] public string defaultSVFName;
    
    public static SettingsData FetchSettingsData()
    {
        return new SettingsData
        {
            masterVolume = (int)AudioSettings.Instance.masterVolumeSlider.value,
            musicVolume = (int)AudioSettings.Instance.musicVolumeSlider.value,
            overallQuality = VideoSettings.Instance.overallQualityDropdown.value,
            controlKeys = ControlsSettings.Instance.controlKeys,
            defaultSVFName = SavesUI.Instance.defaultSVFName
        };
    }
    public static SettingsData CreateDefaultSettingsData()
    {
        return new SettingsData
        {
            masterVolume = 50,
            musicVolume = 75,
            overallQuality = 3,
            controlKeys = ControlsSettings.Instance.ReturnDefaultControlKeys(),
        };
    }
}
