using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("Buttons")] 
    public Button goBackBtn;
    void Start()
    {
        goBackBtn.onClick.AddListener(() => UIManager.Instance.CloseMenu(Menu.Settings));
    }
}
