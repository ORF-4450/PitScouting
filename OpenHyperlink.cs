using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenHyperlink : MonoBehaviour
{

    public string Hyperlink;

    public void Openchannel()
    {
        Application.OpenURL(Hyperlink);
        Debug.Log("TEST");
    }
}
