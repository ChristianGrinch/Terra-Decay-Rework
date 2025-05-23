using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Menu
{
    None,
    Start,
    Settings,
    Saves,
    Game
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
    [Header("Other")] 
    public GameObject rightSettingsPanel;
    
    private void Start()
    { 
        InitializeMenuGameObjects();
        CloseAllMenuGameObjects();
        OpenMenu(Menu.Start);
    }

    private void Update()
    {
        if (Input.GetKeyDown(ControlsSettings.Instance.controlKeys[Keys.GoBack]))
        {
            // Needed because the settings menu must check for changes before just going back
            if (ReturnCurrentMenu() == Menu.Settings) 
            {
                SettingsUI.Instance.GoBack();
                return;
            }
            GoBack();
        }
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

    // ReSharper disable Unity.PerformanceAnalysis
    private Interface ReturnMenu(Menu menu)
    {
        Interface returnMenu = new();
        
        for (int i = 1; i < parentCanvas.transform.childCount; i++)
        {
            //Debug.Log($"{parentCanvas.transform.GetChild(i).gameObject.name.Split(" ")[0]} + {menu.ToString()}");
            if (parentCanvas.transform.GetChild(i).gameObject.name.Split(" ")[0] == menu.ToString())
            {
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
        }
        return returnMenu;
    }

    public void GoBack()
    {
        if (navigationHistory.Count == 1) return;
        
        CloseMenu(navigationHistory[^1].menu);
        //OpenMenu(navigationHistory[^1].menu);
    }

    public Menu ReturnCurrentMenu()
    {
        return navigationHistory[^1].menu;
    }
    private void CloseAllMenuGameObjects()
    {
        for(int i = 0; i < rightSettingsPanel.transform.childCount; i++) rightSettingsPanel.transform.GetChild(i).gameObject.SetActive(false);
        for (int i = 1; i < parentCanvas.transform.childCount; i++) parentCanvas.transform.GetChild(i).gameObject.SetActive(false);
    }
    public void OpenMenu(Menu menu)
    {
        Interface @interface = ReturnMenu(menu);
        //Debug.Log($"{@interface.menu} + {@interface.gameObject.name}");
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