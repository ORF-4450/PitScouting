using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DataToggle : DataInput { //More like "DataBool" or "DataCheckbox"

    public DataStorage ds;  
    private Toggle toggle;
    public string UncheckedResponse = "False";
    public string CheckedResponse = "True";

    public void Start()
    {
        toggle = GetComponent<Toggle>();
        copyValues(ParentObject.GetComponent<DataToggle>());
    }

    public override void changeData(object change)
    {
        bool changeBool = false;
        if (bool.TryParse(change.ToString(), out changeBool)) //Change object to boolean, set it to changeBool?
        {
            toggle.isOn = changeBool;
            return;
        }
        Debug.LogWarning($"Problem changing data for {this.gameObject.name} passed object {change.ToString()} instead of a boolean.");
    }

    public override void clearData()
    {
    //#Set checkbox to off
        toggle.isOn = false;
    }

    public void Update()
    {
        ds.addData(this.gameObject.name, (toggle.isOn ? CheckedResponse : UncheckedResponse), true, this);
    }



    public void copyValues(DataToggle comp)
    {
        try
        {
            ds = comp.ds;
            UncheckedResponse = comp.UncheckedResponse;
            CheckedResponse = comp.CheckedResponse;
        }   
        catch
        {
            Debug.LogError("Something went wrong");
        }
    }
}