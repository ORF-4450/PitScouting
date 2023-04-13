using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStructure : MonoBehaviour
{
    public Menu[] menusArray;
    public Dictionary<string,Menu> menus = new();

    public void Start()
    {
        setupDictionary();
    }
    
    public void setupDictionary()
    {
        foreach (Menu menu in menusArray)
        {
            menus.Add(menu.menuName, menu);
            menu.setupDictionary();
        }
    }
}

[System.Serializable]
public class Menu
{
    public string menuName;
    public Menu[] menusArray;
    public GameObject[] objects;

    public Dictionary<string,Menu> menus = new();

    public void setupDictionary()
    {
        foreach (Menu menu in menusArray)
        {
            menus.Add(menu.menuName,menu);
            menu.setupDictionary();
        }
    }
}