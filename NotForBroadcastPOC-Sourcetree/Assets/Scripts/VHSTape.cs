using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSTape : MonoBehaviour {
    [Range (0.01f,1f)]
    public float grabShiftInZ = 0.81f;
    public string myTitle="UNTITLED";

    private VHSTapeBox myBox;
    private Vector3 startPosition;
    private TextMesh[] myLabels;
    private TextMesh myTopLabel;
    private TextMesh mySideLabel;

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
            } else
            {
                myTopLabel = thisMesh;
            }
        }
        string myTopLabelText=myTitle.Replace("<br>", "\n");
        string mySideLabelText=myTitle.Replace("<br>", " ");
        mySideLabel.text = mySideLabelText;
        myTopLabel.text = myTopLabelText;

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

    void ReturnToShelf()
    {
        this.transform.position = startPosition;
    }

}
