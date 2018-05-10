using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterferenceSystem : MonoBehaviour {

    public Transform defaultLevel;
    //[HideInInspector]
    public float audioInterferenceLevel;
    //[HideInInspector]
    public float videoInterferenceLevel;

    private Transform currentLevel;

    // Use this for initialization
    void Start()
    {
        if (defaultLevel)
        {
            SpawnLevel(defaultLevel);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnLevel(Transform thisLevel)
    {
        currentLevel = Instantiate(thisLevel, this.transform, false);
    }

    public void DestroyLevel()
    {
        Destroy(currentLevel);
        currentLevel = null;
    }
}
