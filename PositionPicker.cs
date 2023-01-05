using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionPicker : MonoBehaviour
{
    public PositionStorage PS;
    public Sprite ON;
    public Sprite OFF;
    public string buttonName = null;
    public string storeName = null;
    public bool fullButton = false;

    [SerializeField] public SidebarStates LaunchButton;

    // Start is called before the first frame update
    void Start()
    {
        if (fullButton == false)
        {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PS.buttonName == buttonName) {
            GetComponent<Image>().sprite = ON;
            if (buttonName == "Cam") LaunchButton.SwitchState(1);
        } else {
            GetComponent<Image>().sprite = OFF;
            if (buttonName == "Cam") LaunchButton.SwitchState(0);
        }
    }

    public void selectButton()
    {
        PS.buttonName = buttonName;
        PS.storeName = storeName;
        PS.SetData();
    }
}