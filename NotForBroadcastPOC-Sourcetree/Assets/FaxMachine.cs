using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaxMachine : MonoBehaviour {

    [Tooltip("Translation in pixels per second when print head moving right")]
    public float headPrintSpeed;
    [Tooltip("Translation in pixels per second when print head moving left")]
    public float headReturnSpeed;
    [Tooltip("Number of seconds or part therof between scrolls once done printing")]
    public float scrollSpeed;
    public Transform blankPage;
    public AudioClip[] printingSFX;
    public AudioClip spoolingSFX;
    public GameObject myPrintHead;
    // public GameObject myCylinder;
    public float myHeadMin, myHeadMax;

    private bool isPrinting;
    private bool isScrolling;
    private FaxPage myPage;
    private int lineCount;
    
    
	// Use this for initialization
	void Start () {
        
	}
	
    void TempStartFaxDELETEME()
    {
        Debug.Log("Telling Fax Machine to receive.");
        ReceiveFax("Line One<br>Line Two<br><br>Line Four<br>This should be line Five<br><br><br><br><br>And this is line ten");
    }


	// Update is called once per frame
	void Update () {

        ///////////////// REMOVE NEXT AFTER TESTING ////////////////////////

        if (!isPrinting)
        {
            Invoke("TempStartFaxDELETEME", 3);
            Debug.Log("Invoking Receive Fax.");
            isPrinting = true;
        }

        ///////////////// REMOVE ABOVE AFTER TESTING ///////////////////////

        if (isPrinting)
        {
            // MOVE HEAD.
            myPrintHead.transform.Translate(Vector3.left*headPrintSpeed*Time.deltaTime);
          // IF HEAD AT END OF LINE:
          // Call MovePageUp
          // Set to end of Line
          // Change direction and speed of head for return journey (one variable with different signs)
          // Increase Line count
          // If linecount at max change from isPrinting to isScrolling
          // IF HEAD AT START OF LINE
          // Change Direction & reset position
          // Play one of a few printing sounds
        }
        // If isScrolling
        // If head moving left move head
        // If head is at start of line:
        // Change head direction to moving right
        // Call Scroll Page Up

    }

    public void ReceiveFax (string thisText) // Normally called by Sequence or Master Controller
    {

        // Create a Fax Page at correct Origin.
        FaxPage myCreatedPage = Instantiate(blankPage).GetComponent<FaxPage>();
        // Count Line Breaks (use | symbol to denote) in text and replace with /n
        lineCount = thisText.Split('<').Length;
        Debug.Log("There are " + lineCount + " lines in this fax.");
        string myPassingText = thisText.Replace("<br>", "\n");
        Debug.Log("Created Page = " + myCreatedPage);
        Debug.Log("Pre Replace Text = "+thisText);
        Debug.Log("Post Replace Text = "+myPassingText);
        // Give Fax Page its Text
        myCreatedPage.SetText(myPassingText);
        // Tell Update to begin Printing
        isPrinting = true;

    }

    public void MovePageUp() // Called by Animator
    {
        // Move Page Up
        // Turn Cylinder
        // Play appropriate sound
    }

    void ScrollPageUp ()
    {
        // Call Move Page Up
        // Increase Line count
        // If linecount at Page Max...
            // Beep to show finished
            // clear is Scrolling and isPrinting
            // Set Paper to grabable
        // ELSE (Not at page max)
            // Invoke MovePageUp in scrollSpeed seconds

    }
}
