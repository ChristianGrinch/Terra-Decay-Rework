using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance {get; private set;}
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
    [Header("Volumes")]
    public int  masterVolume;
    public int  musicVolume;

    public void UpdateMasterVolume(int value)
    {
        masterVolume = value;
        MusicPlayer.Instance.ChangeVolume();
    }

    public void UpdateMusicVolume(int value)
    {
        musicVolume = value;
        MusicPlayer.Instance.ChangeVolume();
    }
}
