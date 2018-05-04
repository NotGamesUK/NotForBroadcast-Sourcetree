using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerBlind : MonoBehaviour {

    public float myMinY;
    public float myMaxY;
    public float mySpeed;
    public ButtonAnimating myUpButton;
    public ButtonAnimating myDownButton;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (myUpButton.isDepressed && myUpButton.hasPower)
        {
            if (this.transform.position.y < myMaxY)
            {
                this.transform.Translate(Vector3.up * mySpeed * Time.deltaTime, Space.World);
            }
        }

        if (myDownButton.isDepressed && myDownButton.hasPower)
        {
            if (this.transform.position.y > myMinY)
            {
                this.transform.Translate(Vector3.down * mySpeed * Time.deltaTime, Space.World);
            }
        }

    }
}
