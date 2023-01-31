using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidebarStates : MonoBehaviour
{
    [SerializeField] List<GameObject> Objects;
    [SerializeField] List<bool> BoolsStateZero;
    [SerializeField] List<bool> BoolsStateOne;
    [SerializeField] List<bool> BoolsStateTwo;
    private List<bool> BoolStateList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchState(int State)
    {
        //Set BoolStateList to...
            switch (State)
            {
                case 0: //...BoolStateZero
                    BoolStateList = BoolsStateZero;
                    break;
                case 1: //...BoolStateOne
                    BoolStateList = BoolsStateOne;
                    break;
                case 2: //...BoolStateTwo
                    BoolStateList = BoolsStateTwo;
                    break;
                default: //Sends error message
                    Debug.LogError("SwitchState in SidebarStates has received an invalid value");
                    break;
            }
        //Set SetActive to corresponding boolean in BoolStateList


        foreach (GameObject gameObjectInList in Objects) //Set game objects to their respective activation booleans in BoolStateList
        {
            gameObjectInList.SetActive(BoolStateList[Objects.IndexOf(gameObjectInList)]);
        }
    }
}
