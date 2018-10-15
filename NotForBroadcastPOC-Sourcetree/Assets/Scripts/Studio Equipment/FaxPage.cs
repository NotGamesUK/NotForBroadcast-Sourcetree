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
    public float holdingFaxFocusLength;
    public float holdingFaxAperture;
    public CameraMovement mainCamera;

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
    private GUIController myGUIController;
    private FaxMachine myFaxMachine;
    // private Shredder myShredder; -- FOR FINAL VERSION
    private bool mouseIsOver;
    
    
    

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        mouseIsOver = false;
        mySFX = GetComponent<AudioSource>();
        myInTray = FindObjectOfType<InTray>();
        myGUIController = FindObjectOfType<GUIController>();
        myFaxMachine = FindObjectOfType<FaxMachine>();
        // myShredder = FindObjectOfType<Shredder>(); -- FOR FINAL VERSION
    }

    // Update is called once per frame
    void Update () {
    }

    public void GoGrabbable()
    {
        if (mouseIsOver)
        {
            myRenderer.material = mySelectedMaterial;
            isSelected = true;
        }
    }

    private void OnMouseEnter()
    {
        mouseIsOver = true;
        if (isGrabbable)
        {
            Debug.Log("Mouse On Page.");
            myRenderer.material = mySelectedMaterial;
            isSelected = true;
        }
    }

    private void OnMouseExit()
    {
        mouseIsOver = false;
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
            mySFX.clip = myTearSFX;
            myInTray.PlayTrayChangeSFX();
            MoveToTray(true);
        }

    }

    public void MoveToTray(bool andOpenGUI)
    {
        myInTray.AddFaxToTray(myPaper.text, myPaper.characterSize);
        if (andOpenGUI)
        {
            myGUIController.ShowPaperwork(true);
        }
        myFaxMachine.StopBeep();
        //Object.Destroy(this.gameObject); // REPLACE THIS LINE WITH CODE TO PUT FAX PAPER ON IN TRAY AND GO DORMANT.
        isGrabbable = false;
        myRenderer.material = myDefaultMaterial;
        Vector3 myNewPosition = myInTray.myFaxTray.transform.position;
        Quaternion myNewRotation = myInTray.myFaxTray.transform.rotation;
        this.transform.position = myNewPosition;
        this.transform.rotation = myNewRotation;
        myFaxMachine.myPage = null;
    }


    public void SetText(string thisText, float thisCharacterSize)
    {
        //Debug.Log("Page - My Paper (Text Mesh) = " + myPaper);
        myPaper.characterSize = thisCharacterSize;
        myPaper.text = thisText;
    }

}
