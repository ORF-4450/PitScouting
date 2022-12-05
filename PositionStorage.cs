using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionStorage : DataInput
{
    public DataStorage ds;
    // Start is called before the first frame update
    public override void changeData(object change)
    {
        if (change is string)
        {
            buttonName = change.ToString();
        }
    }

    public override void clearData()
    {
        buttonName = DefaultButtonName;
        storeName = DefaultStoreName;
    }

    public void SetData()
    {
        ds.addData(this.gameObject.name, storeName, true, this);
    }
    public string DefaultStoreName = null;
    public string DefaultButtonName = null;
    public string storeName = null;
    public string buttonName = null;
}