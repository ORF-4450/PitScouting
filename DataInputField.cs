using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DataInputField : DataInput {

	public DataStorage ds;
	InputField input;

	void Start()
	{
	//#Error: No Datastorage
		if (ds == null) {
			Debug.LogError (this.gameObject.name + " was unable to load because no DataStorage was attached!"); //ds = null
			this.gameObject.SetActive (false);
			return;
		}
	//#Get InputField component
		input = GetComponent<InputField> ();
	}

	void FixedUpdate()
	{
	//#Fill in DataStorage field
		ds.addData (this.gameObject.name, input.text, true,this);
	}

	public override void changeData(object dataToLoad)
	{
		input.text = dataToLoad.ToString();
	}

	public override void clearData()
    {
        changeData("");
    }
}
