using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectList
{
    public GameObject Object;
    public bool Bool;
}

[System.Serializable]
public class Menus
{
    public List<ObjectList> ObjectList;
}

public class SidebarStates : MonoBehaviour
{
    [SerializeField] List<Menus> MenusList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState(int MenuState)
    {
        //Set SetActive to corresponding boolean in BoolStateList

        foreach (ObjectList ObjectInList in MenusList[MenuState].ObjectList) //Set game objects to their respective activation booleans in BoolStateList
        {
            GameObject gameObjectInList = ObjectInList.Object;
            gameObjectInList.SetActive(ObjectInList.Bool);
        }
    }
}
