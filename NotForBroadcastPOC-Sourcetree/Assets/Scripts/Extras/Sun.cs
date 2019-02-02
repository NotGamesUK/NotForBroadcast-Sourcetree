using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

    public SkySetup[] mySkySetups;    

    private Light mySun;
    private Color myDefaultColour;
    private float myDefaultIntensity;
    public Vector3 myDefaultRotation;
    public Material myDefaultSkybox;
    private SkySetup myCurrentSkySetup;

	// Use this for initialization
	void Start () {
        mySun = GetComponent<Light>();
        myDefaultColour = mySun.color;
        myDefaultIntensity = mySun.intensity;
        myDefaultRotation = transform.eulerAngles;
        myDefaultSkybox = RenderSettings.skybox;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetSky(int thisLevel)
    {
        myCurrentSkySetup = null;
        for (int n=0; n<mySkySetups.Length; n++)
        {
            if (mySkySetups[n].levelNumber == thisLevel)
            {
                myCurrentSkySetup = mySkySetups[n];
                break;
            }
        }
        if (myCurrentSkySetup != null)
        {
            // Set Skybox
            this.transform.eulerAngles = myCurrentSkySetup.sunRotation;
            RenderSettings.skybox = myCurrentSkySetup.skyboxMaterial;
            mySun.color = myCurrentSkySetup.lightColour;
            mySun.intensity = myCurrentSkySetup.lightIntensity;

        }
        else
        {
            Debug.Log("ERROR: Sky Setup details NOT FOUND");
        }

    }


    public void ResetSky()
    {
        RenderSettings.skybox = myDefaultSkybox;
        transform.eulerAngles = myDefaultRotation;
        mySun.intensity = myDefaultIntensity;
        mySun.color = myDefaultColour;
    }
}
