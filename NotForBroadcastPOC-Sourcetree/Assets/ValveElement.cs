using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveElement : MonoBehaviour {

    public bool isBlown=false;
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BlowValve()
    {
        // Change visual appearance

        isBlown = true;
    }
}
