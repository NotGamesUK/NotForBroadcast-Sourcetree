using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingFan : MonoBehaviour {

    public GameObject myBlades;
    public GameObject myHead;
    public float bladeMaxSpeed;
    public float bladeAcceleration;
    public float bladeDeceleration;
    public float turnSpeed;
    public float maxTurnAngle;
    public float currentTurnSpeed;
    private float currentBladeSpeed;

    private ButtonAnimating myButton;

	// Use this for initialization
	void Awake () {
        myButton = GetComponentInChildren<ButtonAnimating>();
	}

    private void Start()
    {
        currentTurnSpeed = turnSpeed;
    }
    // Update is called once per frame
    void Update () {
		if (myButton.hasPower)
        {
            // Rotate Blades
            currentBladeSpeed += bladeAcceleration*Time.deltaTime;
            if (currentBladeSpeed>bladeMaxSpeed) { currentBladeSpeed = bladeMaxSpeed; }
            myBlades.transform.Rotate(new Vector3(0, currentBladeSpeed * Time.deltaTime, 0));

            if (!myButton.isDepressed)
            {
                // Turn Head
                myHead.transform.Rotate(new Vector3(0, 0, currentTurnSpeed * Time.deltaTime));
                float thisRot = myHead.transform.localEulerAngles.z;
                if (thisRot >= 180+maxTurnAngle)
                {
                    currentTurnSpeed = -turnSpeed;
                }
                if (thisRot <=180-maxTurnAngle)
                {
                    currentTurnSpeed = turnSpeed;
                }
            }
        } else
        {
            // No Power.  Decelerate Blades if required.
            if (currentBladeSpeed>0)
            {
                currentBladeSpeed -= bladeDeceleration*Time.deltaTime;
                if (currentBladeSpeed<0) { currentBladeSpeed = 0; }
                myBlades.transform.Rotate(new Vector3(0, currentBladeSpeed * Time.deltaTime, 0));
            }
        }
	}

    void PowerOn()
    {
        myButton.hasPower = true;
    }

    void PowerOff()
    {
        myButton.hasPower = false;
    }
}
