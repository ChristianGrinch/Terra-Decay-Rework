using UnityEngine;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public Button settingsBtn;
    void Start()
    {
        settingsBtn.onClick.AddListener(() => UIManager.Instance.OpenMenu(Menu.Settings));
    }
}
