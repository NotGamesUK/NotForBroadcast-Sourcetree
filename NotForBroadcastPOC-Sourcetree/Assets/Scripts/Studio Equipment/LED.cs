using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LED : MonoBehaviour {

    public Material myOffMaterial;
    public Material myRedMaterial;
    public Material myOrangeMaterial;
    public Material myGreenMaterial;
    public enum Colours { Off, Red, Orange, Green }
    public Colours myColour;
    [HideInInspector]
    public bool isFlashing;
    private bool flashIsOn;
    private float myFlashRate = 1f; // Flash Rate in Seconds
    private float myFlashTimer;


    private MeshRenderer myRenderer;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        GoGreen();
	}
	
	// Update is called once per frame
	void Update () {
		if (isFlashing)
        {
            myFlashTimer += Time.deltaTime;
            if (myFlashTimer >= myFlashRate)
            {
                myFlashTimer = 0;
                if (flashIsOn)
                {
                    myRenderer.material = myOffMaterial;
                    flashIsOn = false;
                }
                else
                {
                    switch (myColour)
                    {
                        case Colours.Red:

                            myRenderer.material = myRedMaterial;
                            break;

                        case Colours.Green:

                            myRenderer.material = myGreenMaterial;
                            break;

                        case Colours.Orange:

                            myRenderer.material = myOrangeMaterial;
                            break;

                    }
                    flashIsOn = true;
                }
            }
        }
	}

    public void GoRed()
    {
        myRenderer.material = myRedMaterial;
        myColour = Colours.Red;
    }

    public void GoOrange()
    {
        myRenderer.material = myOrangeMaterial;
        myColour = Colours.Orange;
    }

    public void GoGreen()
    {
        myRenderer.material = myGreenMaterial;
        myColour = Colours.Green;
    }

    public void TurnOff()
    {
        myRenderer.material = myOffMaterial;
        myColour = Colours.Off;
    }

    public void FlashOn(float thisFlashRate)
    {
        //Debug.Log("LED: Starting to Flash");
        flashIsOn = true;
        isFlashing = true;
        myFlashRate = thisFlashRate;
    }

    public void FlashOff()
    {
        //Debug.Log("LED: Stopping Flash");
        isFlashing = false;
        if (!flashIsOn)
        {
            switch (myColour)
            {
                case Colours.Red:

                    myRenderer.material = myRedMaterial;
                    break;

                case Colours.Green:

                    myRenderer.material = myGreenMaterial;
                    break;

                case Colours.Orange:

                    myRenderer.material = myOrangeMaterial;
                    break;

            }
            flashIsOn = true;
        }
    }
}
