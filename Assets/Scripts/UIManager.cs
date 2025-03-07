using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public enum Menu
{
    Start,
    Settings,
}
public class UIManager : MonoBehaviour
{
    public Interface[] navigationHistory;
    public GameObject[] menusGameObjects;
    
    void Start()
    {
        navigationHistory.Append(new Interface { menu = Menu.Start,  gameObject = menusGameObjects[0] });
    }
    void Update()
    {
        
    }
    public void ToggleMenuStatus(Menu menuToToggle)
    {
        
    }
    public bool GetMenuStatus(Menu menu)
    {
        return navigationHistory.Contains(new Interface {menu = menu} );
    }

    public void SetMenuStatus(Menu menu, bool status)
    {
        
    }
}

public class Interface
{
    public Menu menu;
    public GameObject gameObject;
}