using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScreenHalo : MonoBehaviour {

    [Range(0.01f, 5f)]
    public float myFlashTime;

    private Image myRenderer;

    private Color myColour = new Color(255f, 255f, 255f, 0f);
    private bool isFlashing;
    private float flashCoundown;


    // Use this for initialization
    void Start () {
        myRenderer = GetComponent<Image>();
        myRenderer.color = myColour;
	}
	
	// Update is called once per frame
	void Update () {
		if (isFlashing)
        {
            float thisPercentage = flashCoundown / myFlashTime;
            flashCoundown -= Time.deltaTime;
            if (flashCoundown<=0)
            {
                flashCoundown = 0;
                isFlashing = false;
                thisPercentage = 0;
            }
            Debug.Log("Flash Alpha " + (thisPercentage * 255));
            myColour.a = Mathf.Round (thisPercentage * 255);
            Debug.Log("Colour " + myColour);

            myRenderer.color = myColour;
        }

        // FOR TESTING

        if (Input.GetButtonDown("TEMPFlash"))
        {
            Debug.Log("FLASH!!!!!");
            FlashMe();
        }

    }



    public void FlashMe()
    {
        myColour.a = 255f;
        flashCoundown = myFlashTime;
        isFlashing = true;
    }

    public void ResetMe()
    {
        myColour.a = 0f;
        myRenderer.color = myColour;
    }
}
