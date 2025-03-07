using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public enum Menu
{
    Start,
    Settings,
}
public class UIManager : MonoBehaviour
{
    public List<Interface> navigationHistory;
    public GameObject[] menuGameObjects;
    public Canvas parentCanvas;
    
    void Start()
    { 
        InitializeMenus();
        navigationHistory.Append(new Interface { menu = Menu.Start,  gameObject = menuGameObjects[0] });
    }

    public void InitializeMenus()
    {
        while (menuGameObjects.Length < parentCanvas.transform.childCount -1) menuGameObjects.Append(null);
        for (var i = 0; i < parentCanvas.transform.childCount -1; i++)
        {
            if (parentCanvas.transform.GetChild(i).gameObject.GetComponent<Canvas>())
            {
                menuGameObjects[i] = parentCanvas.transform.GetChild(i).gameObject;
            }
        }
    }
    public void ToggleMenuStatus(Menu menuToToggle)
    {
        
    }
    public bool GetMenuStatus(Menu menu)
    {
        return navigationHistory.Contains(new Interface { menu = menu } );
    }

    public void SetMenuStatus(Menu menu, bool status)
    {
        
    }

    public void OpenMenu(Interface @interface)
    {
        if (navigationHistory.Contains(@interface)) return;
        
        navigationHistory.Append(@interface);
        @interface.gameObject.SetActive(true);
    }

    public void CloseMenu(Interface @interface)
    {
        if(!navigationHistory.Contains(@interface)) return;
        
        navigationHistory.Remove(@interface);
        @interface.gameObject.SetActive(false);
    }
}

public class Interface
{
    public Menu menu;
    public GameObject gameObject;
}