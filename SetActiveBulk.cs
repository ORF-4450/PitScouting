using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveBulk : MonoBehaviour
{
    [SerializeField] List<GameObject> Objects;
    [SerializeField] List<bool> Bools;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetState()
    {
        /*foreach (GameObject gameObjectInList in Objects)
        {
            gameObjectInList.SetActive(Bools[Objects.IndexOf(gameObjectInList)]);
        }*/
    }
}