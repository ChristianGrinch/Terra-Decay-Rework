using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum Keys
{
    GoBack,
}
public class SettingsUI : MonoBehaviour
{
    
    public static SettingsUI Instance {get; private set;}
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
    [Header("Buttons")] 
    public Button goBackBtn;
    public Button audioBtn;
    public Button videoBtn;
    public Button saveBtn;
    public Button controlsBtn;
    [Header("Panels")]
    public GameObject audioPanel;
    public GameObject videoPanel;
    public GameObject controlsPanel;
    public GameObject savePanel;
    public GameObject currentPanel;
    [Header("Other")]
    public bool didChangeSetting;
    
    private void Start()
    {
        goBackBtn.onClick.AddListener(GoBack);
        audioBtn.onClick.AddListener(() => OpenPanel(audioPanel));
        videoBtn.onClick.AddListener(() =>  OpenPanel(videoPanel));
        controlsBtn.onClick.AddListener(() => OpenPanel(controlsPanel));
        saveBtn.onClick.AddListener(() =>
        {
            GameManager.Instance.SaveSettings();
            didChangeSetting = false;
        });
    }

    private void Update()
    {
        savePanel.SetActive(didChangeSetting);
    }
    
   

    private void GoBack()
    {
        if (!didChangeSetting)
        {
            UIManager.Instance.GoBack();
        }
        else
        {
            OpenSaveCheck();
        }
    }
    
    private void OpenSaveCheck()
    {
        PopupManager.Instance.OpenPopup(Popup.QuitWithoutSaving);
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
}
