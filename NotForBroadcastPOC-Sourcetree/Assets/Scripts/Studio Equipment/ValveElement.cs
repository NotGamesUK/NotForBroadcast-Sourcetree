using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveElement : MonoBehaviour {

    public bool isBlown=false;
    private ValveBox myBox;
    
	// Use this for initialization
	void Start () {
        myBox = GetComponentInParent<ValveBox>();
		if (!isBlown)
        {
            myBox.valveCount++;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BlowValve()
    {
        // Change visual appearance

        isBlown = true;
        myBox.valveCount--;
    }
}
