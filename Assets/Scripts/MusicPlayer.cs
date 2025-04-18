using System.Collections;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance {get; private set;}
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
    public AudioSource audioSource;
    public AudioClip[] songs;
    private void Start()
    {
        Application.runInBackground = true;
        StartCoroutine(PlayMusic());
    }

    private IEnumerator PlayMusic()
    {
        int randomNumber = Random.Range(0, songs.Length);
        while (true)
        {
            int newRandomNumber = Random.Range(0, songs.Length);
            while (newRandomNumber == randomNumber)
            {
                newRandomNumber = Random.Range(0, songs.Length);
            }
            randomNumber = newRandomNumber;
            audioSource.clip = songs[randomNumber];
            audioSource.Play();
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(audioSource.clip.length);
        }
        // ReSharper disable once IteratorNeverReturns
    }

    public void ChangeVolume()
    {
        int masterVolume = AudioManager.Instance.masterVolume;
        int musicVolume = AudioManager.Instance.musicVolume;
        audioSource.volume = (musicVolume / 100f)*(masterVolume / 100f);
    }
}
