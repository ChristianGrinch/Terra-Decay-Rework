using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button settingsBtn;
    void Start()
    {
        settingsBtn.onClick.AddListener(() => UIManager.Instance.OpenMenu(Menu.Settings));
    }
}
