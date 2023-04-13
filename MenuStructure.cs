using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuStructure : MonoBehaviour
{
    public string menuJson;
    public Menu mainMenu;
    public Test testArray = new Test("test1");

    public void Start()
    {
        //menuJson = JsonUtility.ToJson(mainMenu);
        // mainMenu = deJson(menuJson);
        //Debug.Log(JsonUtility.ToJson(mainMenu));
        // Debug.Log(mainMenu.menus[0].menuName);
        //Debug.Log(JsonUtility.ToJson(testArray));
        //Debug.Log(JsonUtility.ToJson(new Test("testStringAAAA")));
    }


    public Menu deJson(string menu)
    {
        return JsonUtility.FromJson<Menu>(menu);

    }
}

[System.Serializable]
public class Menu
{
    public string menuName;
    public Menu[] menus;
    public GameObject[] objects;
}

[System.Serializable]
public class Test
{
    public string testString;

    public Test(string testString)
    {
        this.testString = testString;
    }
}


// class Test
// {
//     public Test(float posX, float posY, GameObject parent, InfoObject infoObject) {
//         this.posX = posX;
//         this.posY = posY;
//         this.parent = parent;
//         this.infoObject = infoObject;
//     }

//     public float posX = gameObject.transform.position.x;
//     public float posY = gameObject.transform.position.x;
//     public GameObject parent = gameObject.transform.Parent;
//     public InfoObject infoObject = GetComponent<InfoObject>();
// }