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
    public AudioSource myPrintAudiosource;
    public AudioSource myScrollAudiosource;
    public AudioClip[] printingSFX;
    public AudioClip scrollingSFX;
    public AudioClip beepingSFX;
    public GameObject myPrintHead;
    public GameObject myCylinder;
    public float myHeadMin, myHeadMax;

    private bool isPrinting;
    private float headMove;
    private bool isScrolling;
    private FaxPage myPage;
    private int lineCount;
    private int currentLine;
    private int maxPageLines=25;
    //private bool tempToggle;
    
    
	// Use this for initialization
	void Start () {
        myPage = null;
        myPrintHead.transform.localPosition = (new Vector3(0.1f, myPrintHead.transform.localPosition.y, myPrintHead.transform.localPosition.z));
        headMove = headPrintSpeed;
        //ResetMe();
	}

    public void ResetMe()
    {
        if (myPage!=null)
        {
            Destroy(myPage.gameObject);
            myPage = null;
        }
    }
	
    //void TempStartFaxDELETEME()
    //{
    //    Debug.Log("Telling Fax Machine to receive.");
    //    ReceiveFax("Line One<br>Line Two<br><br>Line Four<br>This should be line Five<br><br><br><br><br>And this is line ten<br>");
    //}


	// Update is called once per frame
	void Update () {

        ///////////////// REMOVE NEXT AFTER TESTING ////////////////////////

        //if (!tempToggle)
        //{
        //    Invoke("TempStartFaxDELETEME", 0.5f);
        //    Debug.Log("Invoking Receive Fax.");
        //    tempToggle = true;
        //}

        ///////////////// REMOVE ABOVE AFTER TESTING ///////////////////////

        if (isPrinting)
        {
            // MOVE HEAD.
            myPrintHead.transform.Translate(Vector3.left*headMove*Time.deltaTime);
            //Debug.Log("Print Head X: "+myPrintHead.transform.localPosition.x);
            // IF HEAD AT END OF LINE:
            if (myPrintHead.transform.localPosition.x < myHeadMin)
            {
                MovePageUp();
                // Set to end of Line
                myPrintHead.transform.localPosition = (new Vector3(myHeadMin, myPrintHead.transform.localPosition.y, myPrintHead.transform.localPosition.z));
                // Change direction and speed of head for return journey (one variable with different signs)
                headMove = headReturnSpeed;
                // Increase Line count
                currentLine++;
                // If linecount at max change from isPrinting to isScrolling
                if (currentLine == lineCount)
                {
                    isPrinting = false;
                    isScrolling = true;
                }
            }

            // IF HEAD AT START OF LINE
            if (myPrintHead.transform.localPosition.x > myHeadMax)
            {
                // Change Direction & reset position
                myPrintHead.transform.localPosition = (new Vector3(myHeadMax, myPrintHead.transform.localPosition.y, myPrintHead.transform.localPosition.z));
                headMove = headPrintSpeed;
                // Play one of a few printing sounds
                PlayRandomPrintSound();
            }

        }
        if (isScrolling)
        {
            if (headMove == headReturnSpeed) // If head moving left move head
            {
                myPrintHead.transform.Translate(Vector3.left * headMove * Time.deltaTime);
                // If head is at start of line:
                if (myPrintHead.transform.localPosition.x > myHeadMax)
                {
                    // Change head direction to moving right
                    headMove = headPrintSpeed;
                    ScrollPageUp();
                }
            }
        }

    }

    void PlayRandomPrintSound ()
    {
        int thisClip = Random.Range(0, 3);
        Debug.Log("AudioClip " + thisClip + " selected.");
        myPrintAudiosource.clip = printingSFX[thisClip];
        myPrintAudiosource.Play();
    }

    public void ReceiveFax (string thisText, float thisCharacterSize) // Normally called by Sequence or Master Controller
    {
        if (myPage != null)
        {
            myPage.MoveToTray(false);
        }
        // Create a Fax Page at correct Origin.
        FaxPage myCreatedPage = Instantiate(blankPage).GetComponent<FaxPage>();
        // Count Line Breaks (use | symbol to denote) in text and replace with /n
        lineCount = thisText.Split('<').Length;
        currentLine = 0;
        Debug.Log("There are " + lineCount + " lines in this fax.");
        string myPassingText = thisText.Replace("<br>", "\n");
        Debug.Log("Created Page = " + myCreatedPage);
        Debug.Log("Pre Replace Text = "+thisText);
        Debug.Log("Post Replace Text = "+myPassingText);
        // Give Fax Page its Text
        myCreatedPage.SetText(myPassingText, thisCharacterSize);
        myPage = myCreatedPage;
        // Tell Update to begin Printing
        isPrinting = true;
        headMove = headPrintSpeed; // Set to move right first.
        PlayRandomPrintSound();
    }

    public void MovePageUp() // Called by Animator
    {
        // Move Page Up
        myPage.transform.Translate(Vector3.right*0.01f);// * Time.deltaTime * 1f);
        // Turn Cylinder
        // Play appropriate sound
        myScrollAudiosource.clip = scrollingSFX;
        myScrollAudiosource.Play();
    }

    void ScrollPageUp ()
    {
        // Call Move Page Up
        MovePageUp();
        // Increase Line count
        currentLine++;
        Debug.Log("Current Line: " + currentLine + " of " + maxPageLines + " Max.");
        // If linecount at Page Max...
        if (currentLine == maxPageLines)
        {
            // Beep to show finished
            myPrintAudiosource.clip = beepingSFX;
            myPrintAudiosource.loop = true;
            myPrintAudiosource.Play();

            // clear is Scrolling and isPrinting
            isScrolling = false;
            isPrinting = false;
            // Set Paper to grabable
            myPage.isGrabbable = true;
            myPage.GoGrabbable();
        }
        else        // ELSE (Not at page max)

        {
            // Invoke ScrollPageUp in scrollSpeed seconds
            Invoke("ScrollPageUp", scrollSpeed);
        }

    }

    public void StopBeep()
    {
        myPrintAudiosource.Stop();
        myPrintAudiosource.loop = false;
    }
}
