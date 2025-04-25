using System;
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
    public Button gameplayBtn;
    [Header("Panels")]
    public GameObject audioPanel;
    public GameObject videoPanel;
    public GameObject controlsPanel;
    public GameObject savePanel;
    public GameObject gameplayPanel;
    public GameObject currentPanel;
    [Header("Other")]
    public bool didChangeSetting;

    private GameObject underlinedBtn;
    
    private void Start()
    {
        goBackBtn.onClick.AddListener(GoBack);
        audioBtn.onClick.AddListener(() =>
        {
            UnderlinePanelButton(audioBtn.gameObject);
            OpenPanel(audioPanel);
        });
        videoBtn.onClick.AddListener(() =>
        {
            UnderlinePanelButton(videoBtn.gameObject);
            OpenPanel(videoPanel);
        });
        controlsBtn.onClick.AddListener(() =>
        {
            UnderlinePanelButton(controlsBtn.gameObject);
            OpenPanel(controlsPanel);
        });
        gameplayBtn.onClick.AddListener(() =>
        {
            UnderlinePanelButton(gameplayBtn.gameObject);
            OpenPanel(gameplayPanel);
        });
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
    public void GoBack()
    {
        if (ControlsSettings.Instance.waitingForKey) return;
        
        if (!didChangeSetting)
        {
            UIManager.Instance.GoBack();
        }
        else
        {
            OpenSaveCheck();
        }
    }
    
    private void UnderlinePanelButton(GameObject button)
    {
        // I don't understand the bitwise things, but they work so it's okay
        if (underlinedBtn)
        {
            if (underlinedBtn != button)
            {
                underlinedBtn.GetComponentInChildren<TMP_Text>().fontStyle &= ~FontStyles.Underline; // Bitwise AND NOT to remove the underline
            }
        }
        underlinedBtn = button;
        button.GetComponentInChildren<TMP_Text>().fontStyle |= FontStyles.Underline; // Bitwise OR to ensure the underline is set
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
