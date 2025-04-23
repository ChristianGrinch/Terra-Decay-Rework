using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public static StartUI Instance { get; private set; }
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
    [Header("Buttons")] 
    public Button playBtn;
    public Button settingsBtn;
    public Button savesBtn;
    public Button exitBtn;
    private void Start()
    {
        if (SavesUI.Instance.defaultSVFName != "")
        {
            playBtn.GetComponentInChildren<TMP_Text>().text = $"Play '{SavesUI.Instance.defaultSVFName}'";
        }
        
        playBtn.onClick.AddListener(StartGame);
        settingsBtn.onClick.AddListener(() => UIManager.Instance.OpenMenu(Menu.Settings));
        savesBtn.onClick.AddListener(() => UIManager.Instance.OpenMenu(Menu.Saves));
        exitBtn.onClick.AddListener(ExitGame);
    }

    private void StartGame()
    {
        if (SaveSystem.FindSaves() == null)
        {
            PopupManager.Instance.OpenPopup(Popup.NoSavesRedirect);
        }
        else
        {
            throw new NotImplementedException();
        }
    }
    private void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
				        Application.Quit();
        #endif
    }
}
