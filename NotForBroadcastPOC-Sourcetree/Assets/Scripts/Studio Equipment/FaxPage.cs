using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaxPage : MonoBehaviour {

    public TextMesh myPaper;
    public Material myDefaultMaterial;
    public Material mySelectedMaterial;
    public AudioClip myTearSFX;
    public AudioClip myPlaceSFX;
    public AudioClip myPickupSFX;
    
    //[HideInInspector]
    public bool isGrabbable;
    public float grabShiftInZ=0.5f;
    private bool isGrabbed;
    private bool isStored;
    private bool isSelected;
    private int myText;
    private MeshRenderer myRenderer;
    private AudioSource mySFX;
    private InTray myInTray;
    // private Shredder myShredder; -- FOR FINAL VERSION
    
    
    

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        mySFX = GetComponent<AudioSource>();
        myInTray = FindObjectOfType<InTray>();
        // myShredder = FindObjectOfType<Shredder>(); -- FOR FINAL VERSION
    }

    // Update is called once per frame
    void Update () {
		if (isGrabbed)
        {
            {
                Vector3 temp = Input.mousePosition;
                temp.z = grabShiftInZ; // Set this to be the distance you want the object to be placed in front of the camera.
                this.transform.position = Camera.main.ScreenToWorldPoint(temp);
                Vector3 directionToCamera = Camera.main.transform.position - this.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera, Vector3.back);
                this.transform.rotation = targetRotation;
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
            myRenderer.material = myDefaultMaterial;
            if (isStored)
            {
                // On InTray
                mySFX.clip = myPickupSFX;

            } else
            {
                // On FaxMachine
                mySFX.clip = myTearSFX;
            }
            mySFX.Play();
        }

    }

    private void OnMouseUp()
    {
        if (isGrabbed)
        {
            if (myInTray)
            {
                this.transform.position = myInTray.transform.position;
                this.transform.Translate(new Vector3(0f, 0.08f, 0f), Space.World);
                this.transform.rotation = myInTray.transform.rotation;
                this.transform.Rotate(Vector3.left, 90);
                // Play Putting Page Down Sound
                mySFX.clip = myPlaceSFX;
                mySFX.Play();
                isGrabbed = false;
                isStored = true;

            } else
            {
                // No In-Tray exists - check for Shredder and act accordingly. -- FOR FINAL VERSION
            }
        }
    }

    public void SetText(string thisText)
    {
        //Debug.Log("Page - My Paper (Text Mesh) = " + myPaper);
        myPaper.text = thisText;
    }


}
