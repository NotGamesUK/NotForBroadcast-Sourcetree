using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevModeObject : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DevModeChange(bool thisMode)
    {
        if (thisMode)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
