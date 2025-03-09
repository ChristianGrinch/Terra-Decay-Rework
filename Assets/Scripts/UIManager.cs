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
    public List<GameObject> menuGameObjects;
    public Canvas parentCanvas;
    
    void Start()
    { 
        InitializeMenuGameObjects();
        //navigationHistory.Add(new Interface { menu = Menu.Start,  gameObject = menuGameObjects[0] });
    }

    public void InitializeMenuGameObjects()
    {
        while (menuGameObjects.Count < parentCanvas.transform.childCount - 1)
        {
            menuGameObjects.Add(null);
            navigationHistory.Add(new  Interface());
        }
        for (int i = 1; i < parentCanvas.transform.childCount -1; i++)
        {
            menuGameObjects[i] = parentCanvas.transform.GetChild(i).gameObject;
            navigationHistory[i].gameObject = menuGameObjects[i];
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
        
        navigationHistory.Add(@interface);
        @interface.gameObject.SetActive(true);
    }

    public void CloseMenu(Interface @interface)
    {
        if(!navigationHistory.Contains(@interface)) return;
        
        navigationHistory.Remove(@interface);
        @interface.gameObject.SetActive(false);
    }
}

[System.Serializable]
public class Interface
{
    public Menu menu;
    public GameObject gameObject;
}