using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAirLight : MonoBehaviour {

    public MeshRenderer myOnAirText;
    public Light myLight;

	// Use this for initialization
	void Start () {
        LightOff();
	}

    public void LightOn()
    {
        myOnAirText.enabled = true;
        myLight.enabled = true;

    }

    public void LightOff()
    {
        myOnAirText.enabled = false;
        myLight.enabled = false;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
