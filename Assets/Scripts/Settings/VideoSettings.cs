using System;
using TMPro;
using UnityEngine;

public class VideoSettings : MonoBehaviour
{
    public static VideoSettings Instance {get; private set;}
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
    private void ChangeOverallQuality(int value)
    {
        if(Time.time > 1) SettingsUI.Instance.didChangeSetting = true;
    }
    [Header("Video")]
    public TMP_Dropdown overallQualityDropdown;

    private void Start()
    {
        overallQualityDropdown.onValueChanged.AddListener(ChangeOverallQuality);
    }
}