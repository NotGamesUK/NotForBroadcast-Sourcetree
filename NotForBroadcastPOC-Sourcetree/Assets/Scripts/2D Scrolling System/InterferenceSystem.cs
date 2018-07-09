using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterferenceSystem : MonoBehaviour {

    public Transform defaultLevel;
    //[HideInInspector]
    //public float audioInterferenceLevel;
    ////[HideInInspector]
    //public float videoInterferenceLevel;

    private Transform currentLevel;
    private LevelController myLevel;
    private SignalControlCamera myCamera;

    // Use this for initialization
    void Start()
    {
        if (defaultLevel)
        {
            SpawnLevel(defaultLevel);
        }
        myCamera = FindObjectOfType<SignalControlCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnLevel(Transform thisLevel)
    {
        if (currentLevel)
        {
            Destroy(currentLevel.gameObject);
        }
        currentLevel = Instantiate(thisLevel, this.transform, false);
        myLevel = currentLevel.GetComponent<LevelController>();
        myCamera.ResetScrollingCamera();
    }

    //public void DestroyLevel()
    //{
    //    Destroy(currentLevel);
    //    currentLevel = null;
    //    myLevel = null;
    //}

    public void StartLevel()
    {
        myLevel.levelHasStarted = true;
    }
}
