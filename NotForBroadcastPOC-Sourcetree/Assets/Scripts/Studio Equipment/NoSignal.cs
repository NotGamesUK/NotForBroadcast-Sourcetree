using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class NoSignal : MonoBehaviour {

    public VideoPlayer myScreen;
    private MeshRenderer myImage;
	// Use this for initialization

	void Start () {
        myImage = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (myScreen)
        {
            myImage.enabled = true;
            if (myScreen.isPlaying)
            {
                myImage.enabled = false;
            }

        }
    }
}
