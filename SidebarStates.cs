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
        switch (State)
        {
        case 0:
            BoolStateList = BoolsStateZero;
            break;
        case 1:
            BoolStateList = BoolsStateOne;
            break;
        case 2:
            BoolStateList = BoolsStateTwo;
            break;
        default:
            Debug.LogError("SwitchState in SidebarStates has received an invalid value");
            break;
        }



        foreach (GameObject gameObjectInList in Objects) //Set game objects to their respective activation booleans in BoolStateList
        {
            gameObjectInList.SetActive(BoolStateList[Objects.IndexOf(gameObjectInList)]);
        }
    }

    //Required to call functions within Unity
    /*public void SetStateOne(int hello)
    {
        SetState(BoolsStateOne);
    }

    public void SetStateTwo()
    {
        SetState(BoolsStateTwo);
    }

    public void SetStateThree()
    {
        SetState(BoolsStateThree);
    }*/
}
