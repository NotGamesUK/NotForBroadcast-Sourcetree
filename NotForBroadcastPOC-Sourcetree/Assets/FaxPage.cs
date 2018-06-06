using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaxPage : MonoBehaviour {

    public TextMesh myPaper;
    private bool isPrinting;
    private bool isGrabbable;
    private bool isGrabbed;
    private bool isStored;
    private int myText;
    

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SetText(string thisText)
    {
        Debug.Log("Page - My Paper (Text Mesh) = " + myPaper);
        myPaper.text = thisText;
    }
}
