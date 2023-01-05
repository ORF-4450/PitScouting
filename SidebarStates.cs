using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidebarStates : MonoBehaviour
{
    [SerializeField] List<GameObject> Objects;
    [SerializeField] List<bool> Bools;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SwitchState()
    {
        foreach (bool Boolean in Bools) //Set each bool to its opposite
        {
            Bools[Bools.IndexOf(Boolean)] = !Boolean;
        }

        foreach (GameObject gameObjectInList in Objects) //Set game objects to their respective activation booleans
        {
            gameObjectInList.SetActive(Bools[Objects.IndexOf(gameObjectInList)]);
        }
    }
}
