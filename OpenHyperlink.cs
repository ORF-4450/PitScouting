using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To Use: Attach script to an object.
//Fill out hyperlink field with your link.
//Add a button component to your object.
//As an action, call the Openchannel function in the script, in the object.

public class OpenHyperlink : MonoBehaviour
{

    //Adds field for a hyperlink
    public string Hyperlink;

    public void Openchannel()
    {
        //Opens URL Hyperlink
        Application.OpenURL(Hyperlink);
    }
}
