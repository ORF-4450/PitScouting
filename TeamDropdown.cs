using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamDropdown : DataInput
{

    public Dropdown dropdown;
    public EventTeamData ETD;
    public DataStorage DS;
    private string DefaultText = "ERR - Please Sync";

    // Use this for initialization
    void Start()
    {
        refresh();
        dropdown.AddOptions(new List<Dropdown.OptionData>() { new Dropdown.OptionData(DefaultText) });
        dropdown.RefreshShownValue();
        DS.addData("TeamNumber", "0", true, this);
    }

    void LateUpdate()
    {
        if (dropdown.captionText.text == DefaultText) return;
        string teamNumber = dropdown.captionText.text.Split(' ')[0];
        DS.addData("TeamNumber", teamNumber.Split(' ')[0], true);
    }

    public void clear()
    {
        dropdown.ClearOptions();
        dropdown.RefreshShownValue();
    }

    public void refresh()
    {
        List<Dropdown.OptionData> tmpList = new List<Dropdown.OptionData>();
    //#Read team numbers and names.
        foreach (TeamInfo ti in ETD.getTeams(ETD.getSelectedEvent().key))
        {
            tmpList.Add(new Dropdown.OptionData(ti.team_number + " - " + ti.nickname));
        }
    //#Clear and refresh with the new data
        clear();
        dropdown.AddOptions(tmpList);
        dropdown.RefreshShownValue();
        Debug.Log("Refreshed Team Dropdown");
    }

    public override void changeData(object change)
    {
        return;
    }

    public override void clearData()
    {
        refresh();
    }
}
