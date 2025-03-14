using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Buttons")] 
    public Button goBackBtn;
    public Button audioBtn;
    public Button videoBtn;
    [Header("Panels")]
    public GameObject audioPanel;
    public GameObject videoPanel;

    public GameObject currentPanel;
    private void Start()
    {
        goBackBtn.onClick.AddListener(() => UIManager.Instance.GoBack());
        audioBtn.onClick.AddListener(() => OpenPanel(audioPanel));
        videoBtn.onClick.AddListener(() =>  OpenPanel(videoPanel));
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
