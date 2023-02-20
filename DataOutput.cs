using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DataOutput : MonoBehaviour {
	public DataStorage DS;
	public string key;
	public string prefix;
	public string suffix;

	private Text text;

	void Start () {
	//#Disappear if error is encountered
		if (DS == null || key == null) this.enabled = false;
	//#Get Text component to display text in
		text = GetComponent<Text>();
	}
	
	void LateUpdate () {
		if (text != null) {
			if (DS.data.ContainsKey (key))
			{
				text.text = prefix + DS.data [key] + suffix;
		//#Error: Key Doesn't Exist
			} else {
				text.text = "ERROR: Key " + key + " doesn't exist!";
		//
			}
		}
	}
}
