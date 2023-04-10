using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //for ToArray()
using UnityEngine.UI; //for text component

public class ConfigurableInput : MonoBehaviour
{
    public string dataKey;
    public string label;

    public Dictionary<string,ConfigObj> configOptions = new Dictionary<string, ConfigObj>();
    public int selectedConfig;
    public ConfigurableInputOptions configOptionsScript;
    private List<ConfigurationOption> configOptionsList = new List<ConfigurationOption>();
    public GameObject dataInputObject;
    //public DataStorage ds;
    public bool activation;
    public Object dataScript;
    public System.Type dataScriptType;

    // Start is called before the first frame update
    void Awake()
    {
        dataInputObject = gameObject.transform.GetChild(0).gameObject;
        if (dataInputObject == null)
        {
            Debug.LogError("dataInputObject was null. ConfigurableInput does not have children.");
        }
        dataInputObject.SetActive(true);
        gameObject.SetActive(activation);
    }

    public void Setup()
    {
    //#Create Object
        dataInputObject = (GameObject)Instantiate(getObject(selectedConfig), gameObject.transform.position, gameObject.transform.rotation, gameObject.transform);
    //#Setup Object
        dataInputObject.transform.GetChild(1).name = dataKey; //Set data key as name of object
        dataInputObject.transform.GetChild(0).GetComponent<Text>().text = label;
        dataInputObject = gameObject.transform.GetChild(0).gameObject;
    //#Destroy Old DataInput Component
        try
        {
            if (Application.isPlaying)
                Destroy(gameObject.GetComponent<DataInput>());
            else
                DestroyImmediate(gameObject.GetComponent<DataInput>());
        }
        catch { Debug.Log("Did not have a DataInput"); }

        try
        {
            if (Application.isPlaying)
                Destroy(gameObject.GetComponent<Dropdown>());
            else
                DestroyImmediate(gameObject.GetComponent<Dropdown>());
        }
        catch
        {

        }

    //#Create New DataInputComponent
        DataInput dataInputComponent = (DataInput)gameObject.AddComponent(dataInputObject.transform.GetChild(1).GetComponent<DataInput>().GetType());
        dataInputComponent.enabled = false;
    }

    public string objectKey(int objectIndex)
    {
        return configOptions.Keys.ToArray()[objectIndex];
    }

    public GameObject getObject(int objectIndex)
    {
        string ObjKey = objectKey(objectIndex);
        return configOptions[ObjKey].templateObject;
    }

    public void fillConfigOptions()
    {
    //#
        configOptionsList = configOptionsScript.configOptions;
        foreach(ConfigurationOption option in configOptionsList)
        {
            configOptions.Add(option.templateKey,option.ConfigObj);
        }
    //
    }
}
