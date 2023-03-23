using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataInputDropdown : DataInput {

    public DataStorage ds;
    public Dropdown dropdown; //Script 'Dropdown' in input GameObject
    [SerializeField] private bool OverrideOutputs;
    [SerializeField] private List<string> InputStrings;
    [SerializeField] private List<string> OutputStrings;

    public override void changeData(object change)
    {
        if (change is string)
        {
            for (int i = 0; i < dropdown.options.Count; i++)
            {
                Dropdown.OptionData option = dropdown.options[i];
                if (change.ToString() == option.text)
                {
                    dropdown.value = i;
                    dropdown.RefreshShownValue();
                    return;
                }
            }
        }
    }

    public override void clearData()
    {
        dropdown.value = 0;
        dropdown.RefreshShownValue();
    }

    void Start()
    {
        dropdown = GetComponent<Dropdown>();
    }

    void FixedUpdate()
    {
        if (!OverrideOutputs)
        {
            ds.addData(this.gameObject.name, dropdown.captionText.text, true, this);

        } else {

            foreach (string stringInList in InputStrings)
            {
                string output = OutputStrings[InputStrings.IndexOf(stringInList)];

                if (stringInList == dropdown.captionText.text)
                {
                    ds.addData(this.gameObject.name, output, true, this);
                }
                
            }

        }
    }
}
