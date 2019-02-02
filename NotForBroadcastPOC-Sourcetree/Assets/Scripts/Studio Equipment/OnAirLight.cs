using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAirLight : MonoBehaviour {

    public MeshRenderer myOnAirText;
    public Light myLight;
    public GameObject myBulbCover;

	// Use this for initialization
	void Start () {
        LightOff();
	}

    public void LightOn()
    {
        myOnAirText.enabled = true;
        myLight.enabled = true;
        myBulbCover.SetActive(false);
    }

    public void LightOff()
    {
        myOnAirText.enabled = false;
        myLight.enabled = false;
        myBulbCover.SetActive(true);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
