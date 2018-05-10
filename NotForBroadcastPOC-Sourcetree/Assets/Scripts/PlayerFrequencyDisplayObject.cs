using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrequencyDisplayObject : MonoBehaviour {

    public float topY;
    public float bottomY;
    public float currentInterferenceLevel;
    private float yRange;

	// Use this for initialization
	void Start () {
        yRange = topY - bottomY;

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        float thisLevel = GetInterferenceLevel(other);
        Debug.Log("Triggered by " + other + " for " + thisLevel + " Interference.");
        if (currentInterferenceLevel < thisLevel) { currentInterferenceLevel = thisLevel; }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Leaving " + other);
        currentInterferenceLevel -= 50f;
    }

    public void MoveDotTo(float thisPosition)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, bottomY + (thisPosition * yRange), transform.localPosition.z );
    }

    float GetInterferenceLevel(Collider other) {

        float thisInterferance = 0;
        InterferenceHigh thisHigh = other.GetComponent<InterferenceHigh>();
        InterferenceLow thisLow = other.GetComponent<InterferenceLow>();
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
}
