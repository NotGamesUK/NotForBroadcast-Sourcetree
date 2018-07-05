using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalControlCamera : MonoBehaviour {

    private Vector3 myStartPosition;
	// Use this for initialization
	void Start () {
        myStartPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetScrollingCamera()
    {
        this.transform.position = myStartPosition;
    }
}
