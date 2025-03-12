using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Menu
{
    None,
    Start,
    Settings,
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance {get; private set;}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<Interface> navigationHistory;
    public List<Interface> interfaces;
    public List<GameObject> menuGameObjects;
    public Canvas parentCanvas;
    
    private void Start()
    { 
        InitializeMenuGameObjects();
        OpenMenu(Menu.Start);
    }

    private void InitializeMenuGameObjects()
    {
        while (menuGameObjects.Count < parentCanvas.transform.childCount - 1)// "i - 1" to offset for the "Other" GameObject in the root Canvas
        {
            menuGameObjects.Add(null);
            interfaces.Add(new Interface());
        }
        for (int i = 0; i < parentCanvas.transform.childCount - 1; i++)
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

    private Interface ReturnMenu(Menu menu)
    {
        Interface returnMenu = new();
        
        for (int i = 1; i < parentCanvas.transform.childCount; i++)
        {
            Debug.Log($"{parentCanvas.transform.GetChild(i).gameObject.name.Split(" ")[0]} + {menu.ToString()}");
            if (parentCanvas.transform.GetChild(i).gameObject.name.Split(" ")[0] == menu.ToString())
            {
                Debug.Log("Ran inside if.");
                returnMenu.gameObject = parentCanvas.transform.GetChild(i).gameObject;
                
                string menuName = returnMenu.gameObject.name.Split(" ")[0];
            
                if (Enum.TryParse(menuName, out Menu parsedMenu))
                {
                    returnMenu.menu = parsedMenu;
                }
                else
                {
                    Debug.LogWarning($"Menu name '{menuName}' does not match any value in the Menu enum.");
                }

                break;
            }
            else
            {
                Debug.Log("");
            }
        }
        return returnMenu;
    }

    public void GoBack()
    {
        if (navigationHistory.Count == 1) return;
        
        CloseMenu(navigationHistory[^1].menu);
        OpenMenu(navigationHistory[^1].menu);
    }

    public void CloseAllMenus()
    {
        for (int i = 1; i < navigationHistory.Count - 1; i++)
        {
            string menuName = navigationHistory[i].gameObject.name.Split(" ")[0];
            
            if (Enum.TryParse(menuName, out Menu parsedMenu))
            {
                CloseMenu(parsedMenu);
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

    public void OpenMenu(Menu menu)
    {
        Interface @interface = ReturnMenu(menu);
        Debug.Log($"{@interface.menu} + {@interface.gameObject.name}");
        @interface.gameObject.SetActive(true); // Above the return so that GoBack() can call this method and open the last menu correctly
        if (navigationHistory.Contains(@interface)) return;
        
        navigationHistory.Add(@interface);
    }

    public void CloseMenu(Menu menu)
    {
        Interface @interface = ReturnMenu(menu);
        
        bool isThere = false;
        foreach (Interface navigationElement in navigationHistory)
        {
            // If the menu trying to be closed is not in navigation history, return
            isThere = navigationElement.menu == @interface.menu;
        }
        if (!isThere) return;
        
        foreach (Interface navigationElement in navigationHistory.ToList())
        {
            if (navigationElement.menu == @interface.menu)
            {
                navigationHistory.Remove(navigationElement);
                break;
            }
        }
        @interface.gameObject.SetActive(false);
    }
}

[Serializable]
public class Interface
{
    public Menu menu;
    public GameObject gameObject;
}