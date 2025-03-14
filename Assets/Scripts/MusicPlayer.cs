using System;
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
        StartCoroutine(PlayMusic());
    }

    private IEnumerator PlayMusic()
    {
        while (true)
        {
            audioSource.clip = songs[UnityEngine.Random.Range(0, songs.Length)];
            audioSource.Play();
            yield return new WaitWhile(() => audioSource.isPlaying);
        }
    }
}
