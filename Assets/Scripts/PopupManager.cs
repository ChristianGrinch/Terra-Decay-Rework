using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Popup
{
    QuitWithoutSaving
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

    public List<GameObject> popups;
    private void Start()
    {
        InitalizePopupGameObjects();
    }

    private void InitalizePopupGameObjects()
    {
        popups = Resources.LoadAll<GameObject>("Popups").ToList();
    }

    public void OpenPopup(Popup popup)
    {
        GameObject instantiatedPopup;
        switch (popup)
        {
            case Popup.QuitWithoutSaving:
                foreach (GameObject item in popups)
                {
                    if (item.name == popup.ToString())
                    {
                        instantiatedPopup = Instantiate(item, UIManager.Instance.parentCanvas.transform);
                    }
                }
                break;
            default:
                Debug.LogError("Invalid popup type!");
                break;
        }
    }
}
