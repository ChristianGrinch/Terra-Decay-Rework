using System;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [Header("Buttons")] 
    public Button playBtn;
    public Button settingsBtn;
    public Button savesBtn;
    public Button exitBtn;
    private void Start()
    {
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
