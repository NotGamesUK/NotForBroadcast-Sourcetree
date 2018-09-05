using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VHSTape : MonoBehaviour {

    public string myTitle="UNTITLED";
    public VideoClip myVideo;
    public VideoClip myVideoSmaller;
    public AudioClip myAudio;
    [HideInInspector]
    public VHSTapeBox myBox;
    public string myName;
    private Vector3 startPosition;
    private TextMesh[] myLabels;
    private TextMesh myTopLabel;
    private MeshRenderer myTopMesh;
    private TextMesh mySideLabel;
    private MeshRenderer mySideMesh;
    private float grabShiftInZ = 0.81f;


    // Use this for initialization
    void Start () {
        myBox = GetComponentInChildren<VHSTapeBox>();
        startPosition = this.transform.position;

        // Label The Tape

        // Find the Labels
        myLabels = GetComponentsInChildren<TextMesh>();
        foreach (TextMesh thisMesh in myLabels)
        {
            if (thisMesh.lineSpacing == 1)
            {
                mySideLabel = thisMesh;
                mySideMesh = thisMesh.GetComponent<MeshRenderer>();
            } else
            {
                myTopLabel = thisMesh;
                myTopMesh = thisMesh.GetComponent<MeshRenderer>();

            }
        }
        RewriteLabel();
    }
	
	// Update is called once per frame
	void Update () {
		if (myBox.isHeld)
        {
            Vector3 temp = Input.mousePosition;
            temp.z = grabShiftInZ; // Set this to be the distance you want the object to be placed in front of the camera.
            this.transform.position = Camera.main.ScreenToWorldPoint(temp);
        }
	}

    public void ReturnToShelf()
    {
        this.transform.position = startPosition;
    }

    public void SetMyFont(Font thisFont, Material thisMaterial)
    {
        myTopLabel.font = thisFont;
        mySideLabel.font = thisFont;
        myTopMesh.material = thisMaterial;
        mySideMesh.material = thisMaterial;
    }

    public void RewriteLabel()
    {
        string myTopLabelText = myTitle.Replace("<br>", "\n");
        string mySideLabelText = myTitle.Replace("<br>", " ");
        mySideLabel.text = mySideLabelText;
        myTopLabel.text = myTopLabelText;

    }
}
