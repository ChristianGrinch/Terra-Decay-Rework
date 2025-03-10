using System;
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
    public List<Interface> interfaces;
    public List<GameObject> menuGameObjects;
    public Canvas parentCanvas;
    
    void Start()
    { 
        InitializeMenuGameObjects();
        //navigationHistory.Add(new Interface { menu = Menu.Start,  gameObject = menuGameObjects[0] });
        //OpenMenu(Menu.Start);
    }

    public void InitializeMenuGameObjects()
    {
        while (menuGameObjects.Count < parentCanvas.transform.childCount - 1)// "i - 1" to offset for the "Other" GameObject in the root Canvas
        {
            menuGameObjects.Add(null);
            interfaces.Add(new Interface());
        }
        for (int i = 0; i < parentCanvas.transform.childCount -1; i++)
        {
            menuGameObjects[i] = parentCanvas.transform.GetChild(i + 1).gameObject; // "i + 1" to offset for the "Other" GameObject in the root Canvas
            interfaces[i].gameObject = menuGameObjects[i];
            
            string menuName = menuGameObjects[i].name.Split(" ")[0];
            
            if (Enum.TryParse(menuName, out Menu parsedMenu)) // attempts to convert menuName into a Menu enum value
            {
                interfaces[i].menu = parsedMenu;
            }
            else
            {
                Debug.LogWarning($"Menu name '{menuName}' does not match any value in the Menu enum.");
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