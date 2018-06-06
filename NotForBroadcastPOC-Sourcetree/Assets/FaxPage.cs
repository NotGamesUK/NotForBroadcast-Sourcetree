using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaxPage : MonoBehaviour {

    public TextMesh myPaper;
    public Material myDefaultMaterial;
    public Material mySelectedMaterial;
    public Quaternion targetRotation = Quaternion.Euler(0, -90, 0);
    //[HideInInspector]
    public bool isGrabbable;
    public float grabShiftInZ;
    private bool isGrabbed;
    private bool isStored;
    private bool isSelected;
    private int myText;
    private MeshRenderer myRenderer;
    

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (isGrabbed)
        {
            {
                Vector3 temp = Input.mousePosition;
                temp.z = grabShiftInZ; // Set this to be the distance you want the object to be placed in front of the camera.
                this.transform.position = Camera.main.ScreenToWorldPoint(temp);
            }

        }
    }

    private void OnMouseEnter()
    {
        if (isGrabbable)
        {
            Debug.Log("Mouse On Page.");
            myRenderer.material = mySelectedMaterial;
            isSelected = true;
        }
    }

    private void OnMouseExit()
    {
        if (isSelected)
        {
            myRenderer.material = myDefaultMaterial;
            isSelected = false;
        }
    }

    private void OnMouseDown()
    {
        if (isSelected)
        {
            isGrabbed = true;
            
            transform.rotation = targetRotation;
        }

    }

    public void SetText(string thisText)
    {
        Debug.Log("Page - My Paper (Text Mesh) = " + myPaper);
        myPaper.text = thisText;
    }


}
