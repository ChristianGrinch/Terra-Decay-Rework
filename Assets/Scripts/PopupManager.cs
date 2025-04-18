using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Popup
{
    QuitWithoutSaving,
    NoSavesRedirect
}
public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance { get; private set; }
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

    [Header("Popup Objects")] 
    public GameObject popupPrefab;
    public GameObject instantiatedPopup;
    public TMP_Text titleText;
    public Button action1Btn;
    public Button action2Btn;
    public Button cancelBtn;
    // ReSharper disable Unity.PerformanceAnalysis
    public void OpenPopup(Popup popup)
    {
        switch (popup)
        {
            case Popup.QuitWithoutSaving:
                InitializeMenuGameObjects();

                titleText.text = "Quit without saving?";

                action1Btn.GetComponentInChildren<TMP_Text>().text = "Save and exit";
                action1Btn.onClick.AddListener(() =>
                {
                    GameManager.Instance.SaveSettings();
                    SettingsUI.Instance.didChangeSetting = false;
                    UIManager.Instance.GoBack();
                    Destroy(instantiatedPopup.transform.parent.gameObject);
                });

                action2Btn.GetComponentInChildren<TMP_Text>().text = "Exit without saving";
                action2Btn.onClick.AddListener(() =>
                {
                    GameManager.Instance.LoadSettingsData();
                    SettingsUI.Instance.didChangeSetting = false;
                    UIManager.Instance.GoBack();
                    Destroy(instantiatedPopup.transform.parent.gameObject);
                });
                cancelBtn.onClick.AddListener(() => Destroy(instantiatedPopup.transform.parent.gameObject));
                break;
            case Popup.NoSavesRedirect:
                Debug.Log("ran");
                InitializeMenuGameObjects();

                titleText.text = "No save detected! You must create a save before playing";

                action1Btn.GetComponentInChildren<TMP_Text>().text = "Open save creator";
                action1Btn.onClick.AddListener(() =>{
                    UIManager.Instance.OpenMenu(Menu.Saves);
                    Destroy(instantiatedPopup.transform.parent.gameObject);
                });
                
                action2Btn.GetComponentInChildren<TMP_Text>().text = "OK";
                action2Btn.onClick.AddListener(() => Destroy(instantiatedPopup.transform.parent.gameObject));
                
                cancelBtn.onClick.AddListener(() => Destroy(instantiatedPopup.transform.parent.gameObject));
                break;
            default:
                Debug.LogError("Invalid popup type!");
                break;
        }
    }

    private void InitializeMenuGameObjects()
    {
        instantiatedPopup = Instantiate(popupPrefab, UIManager.Instance.parentCanvas.transform).transform.Find("Panel").gameObject;
        titleText = instantiatedPopup.GetComponentInChildren<TMP_Text>();
        action1Btn = instantiatedPopup.transform.Find("Action Btn").GetComponent<Button>();
        action2Btn = instantiatedPopup.transform.Find("Action 2 Btn").GetComponent<Button>();
        cancelBtn = instantiatedPopup.transform.Find("Cancel Btn").GetComponent<Button>();
    }
}
