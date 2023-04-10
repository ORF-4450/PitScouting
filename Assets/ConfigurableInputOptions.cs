using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurableInputOptions : MonoBehaviour
{
    [SerializeField] public List<ConfigurationOption> configOptions;
    // Start is called before the first frame update
    void Awake()
    {
        
    }
}

[System.Serializable]
public class ConfigurationOption
{
    public string templateKey;
    public ConfigObj ConfigObj;
}

[System.Serializable]
public class ConfigObj
{
    public GameObject templateObject;
    public DataInput dataScript;
}