using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DataCounter : DataInput {

    public DataStorage ds; //Datastorage script to save count to 
    public string prefix; //String to put before number when displaying
    public string suffix; //String to put after number when displaying
    Text text; //Place to show number
    int defaultCount = 0;
    int count;

    // Use this for initialization
    void Start () {
        count = defaultCount;
    //#Return Error if unknown DataStorage
        if (ds == null)
        {
            Debug.LogError(this.gameObject.name + " was unable to load because no DataStorage was attached!"); //ds = null
            this.gameObject.SetActive(false);
            return;
        }
    //#Find text component that shows the number
        text = GetComponent<Text>();
	}

    void FixedUpdate()
    {
    //#Set respective data in data store to the current count
        ds.addData(this.gameObject.name, count.ToString(), true, this);
    //#Set number display to number, with the prefix and suffix
        text.text = prefix + count + suffix;
    }

	public override void changeData(object amountToChangeTo)
    {
		count = int.Parse(amountToChangeTo.ToString());
    }

    public void adjustCount(int amountToChangeBy)
    {
        count += amountToChangeBy; //add input to 'count'
        if (count < 0) count = 0;
    }
    
    public override void clearData()
    {
        count = defaultCount;
    }
}