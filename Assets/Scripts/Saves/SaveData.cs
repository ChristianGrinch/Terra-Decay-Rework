using UnityEngine;
using MessagePack;

public class SaveData
{
    // Player Data (Keys 0-49)
    
    // Game Data (Keys 50-99)
    
    // Settings Data (Keys 100-149)
    [Key(100)] public int masterVolume;
    [Key(101)] public int musicVolume;

    public static SaveData AssignData()
    {
        return new SaveData()
        {

        };
    }
    public static SaveData AssignSettingsData()
    {
        return new SaveData()
        {
            masterVolume = (int)SettingsUI.Instance.musicVolumeSlider.value,
            musicVolume = (int)SettingsUI.Instance.musicVolumeSlider.value,
        };
    }

    public static SaveData CreateDefaultData()
    {
        return new SaveData()
        {

        };
    }

    public static SaveData CreateDefaultSettingsData()
    {
        return new SaveData()
        {
            masterVolume = 100,
            musicVolume = 100,
        };
    }
}
