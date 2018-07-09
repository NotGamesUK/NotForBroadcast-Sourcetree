using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class NoSignal : MonoBehaviour {

    public VideoPlayer myScreen;
    private MeshRenderer myImage;
    private bool hasJumpedBack = false;
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

    public void JumpBack()
    {
        if (!hasJumpedBack)
        {
            this.transform.Translate(new Vector3(0f, -0.2f, 0f));
            hasJumpedBack = true;
        }
    }

    public void JumpToFront()
    {
        if (hasJumpedBack)
        {
            this.transform.Translate(new Vector3(0f, 0.2f, 0f));
            hasJumpedBack = false;
        }
    }
}
