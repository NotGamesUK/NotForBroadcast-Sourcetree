using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrequencyDisplayObject : MonoBehaviour {

    public float topY;
    public float bottomY;
    //[HideInInspector]
    public float currentWhiteNoiseLevel, currentAudioInterferenceLevel, currentResistanceLevel;
    private float yRange;
    private Interference2D thisParent;

	// Use this for initialization
	void Start () {
        yRange = topY - bottomY;

    }

    // Update is called once per frame
    void Update () {
		
	}


    public void MoveDotTo(float thisPosition)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, bottomY + (thisPosition * yRange), transform.localPosition.z );
    }

    float GetInterferenceLevel(Collider2D other) {

        float thisInterferance = 0;
        InterferenceHigh2D thisHigh = other.GetComponent<InterferenceHigh2D>();
        InterferenceLow2D thisLow = other.GetComponent<InterferenceLow2D>();
        if (thisHigh)
        {
            thisInterferance = thisHigh.myInterferenceLevel;
        }
        if (thisLow)
        {
            thisInterferance = thisLow.myInterferenceLevel;
        }

        return thisInterferance;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        thisParent = other.GetComponentInParent<Interference2D>();
        float thisLevel = GetInterferenceLevel(other);

        switch (thisParent.myType)
        {
            case (Interference2D.Type.Default):
                Debug.Log("Triggered by " + other + " for " + thisLevel + " White Noise.");
                if (currentWhiteNoiseLevel < thisLevel) { currentWhiteNoiseLevel = thisLevel; }
                break;


            case (Interference2D.Type.Audio):
                Debug.Log("Triggered by " + other + " for " + thisLevel + " Audio Interference.");
                if (currentAudioInterferenceLevel < thisLevel) { currentAudioInterferenceLevel = thisLevel; }

                break;


            case (Interference2D.Type.Resistance):
                Debug.Log("Triggered by " + other + " for " + thisLevel + " Resistance Hacking.");
                if (currentResistanceLevel < thisLevel) { currentResistanceLevel = thisLevel; }

                break;


        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        thisParent = other.GetComponentInParent<Interference2D>();

        switch (thisParent.myType)
        {
            case (Interference2D.Type.Default):
                Debug.Log("Leaving White Noise" + other);
                currentWhiteNoiseLevel -= 50f;
                break;


            case (Interference2D.Type.Audio):
                Debug.Log("Leaving Audio Interference" + other);
                currentWhiteNoiseLevel -= 50f;

                break;


            case (Interference2D.Type.Resistance):
                Debug.Log("Leaving Resistance Hack" + other);
                currentWhiteNoiseLevel -= 50f;

                break;

                //Debug.Log("Leaving " + other);
        }
    }

}
