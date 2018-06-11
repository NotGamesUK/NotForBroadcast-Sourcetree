using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFrequencyDisplayObject : MonoBehaviour {

    public float topY= 0.407f;
    public float bottomY= -0.397f;
    public float fadeSpeed=25;
    //[HideInInspector]
    public float currentWhiteNoiseLevel, currentAudioInterferenceLevel, currentResistanceLevel;
    public float targetWhiteNoiseLevel, targetAudioInterferenceLevel, targetResistanceLevel;
    private float yRange;
    public AudioSource audioInterferencePlayer;
    private Interference2D thisParent;

	// Use this for initialization
	void Start () {
        yRange = topY - bottomY;

    }

    // Update is called once per frame
    void Update () {
		if (currentWhiteNoiseLevel != targetWhiteNoiseLevel)
        {
            float thisDirection = Mathf.Sign(targetWhiteNoiseLevel - currentWhiteNoiseLevel);
            //Debug.Log("White noise fade direction=" + thisDirection);
            if (thisDirection==-1) { thisDirection = -3; } // Fade Out Faster than fade In
            currentWhiteNoiseLevel += thisDirection * fadeSpeed * Time.deltaTime;
            if (currentWhiteNoiseLevel  <0) { currentWhiteNoiseLevel = 0; }
            if (currentWhiteNoiseLevel >100) { currentWhiteNoiseLevel = 100; }
            if (currentWhiteNoiseLevel > 49 && currentWhiteNoiseLevel < 51 && targetWhiteNoiseLevel == 50) { currentWhiteNoiseLevel = 50; }
            //Debug.Log("WHITE NOISE - Current: " + currentWhiteNoiseLevel + "   Target: " + targetWhiteNoiseLevel);
        }

        if (currentAudioInterferenceLevel != targetAudioInterferenceLevel)
        {
            float thisDirection = Mathf.Sign(targetAudioInterferenceLevel - currentAudioInterferenceLevel);
            //Debug.Log("Audio Interference fade direction=" + thisDirection);
            if (thisDirection == -1) { thisDirection = -3; } // Fade Out Faster than fade In
            currentAudioInterferenceLevel += thisDirection * fadeSpeed * Time.deltaTime;
            if (currentAudioInterferenceLevel < 0) { currentAudioInterferenceLevel = 0; }
            if (currentAudioInterferenceLevel > 100) { currentAudioInterferenceLevel = 100; }
            if (currentAudioInterferenceLevel > 49 && currentAudioInterferenceLevel < 51 && targetAudioInterferenceLevel == 50) { currentAudioInterferenceLevel = 50; }
            //Debug.Log("AUDIO - Current: " + currentAudioInterferenceLevel + "   Target: " + targetAudioInterferenceLevel);
        }

        if (currentResistanceLevel != targetResistanceLevel)
        {
            float thisDirection = Mathf.Sign(targetResistanceLevel - currentResistanceLevel);
            //Debug.Log("Audio Interference fade direction=" + thisDirection);
            if (thisDirection == -1) { thisDirection = -3; } // Fade Out Faster than fade In
            currentResistanceLevel += thisDirection * fadeSpeed * Time.deltaTime;
            if (currentResistanceLevel < 0) { currentResistanceLevel = 0; }
            if (currentResistanceLevel > 100) { currentResistanceLevel = 100; }
            if (currentResistanceLevel > 49 && currentResistanceLevel < 51 && targetResistanceLevel == 50) { currentResistanceLevel = 50; }
            //Debug.Log("AUDIO - Current: " + currentResistanceLevel + "   Target: " + targetResistanceLevel);
        }


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
                if (targetWhiteNoiseLevel < thisLevel) { targetWhiteNoiseLevel = thisLevel; }
                break;


            case (Interference2D.Type.Audio):
                Debug.Log("Triggered by " + other + " for " + thisLevel + " Audio Interference.");
                if (other.tag != "ChangeOnly")
                {
                    if (targetAudioInterferenceLevel < thisLevel) { targetAudioInterferenceLevel = thisLevel; }

                }
                if (thisParent.myAudio != audioInterferencePlayer.clip && thisParent.myAudio)
                {
                    audioInterferencePlayer.clip = thisParent.myAudio;
                    audioInterferencePlayer.Play();
                }
                break;


            case (Interference2D.Type.Resistance):
                Debug.Log("Triggered by " + other + " for " + thisLevel + " Resistance Hacking.");
                if (targetResistanceLevel < thisLevel) { targetResistanceLevel = thisLevel; }

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
                targetWhiteNoiseLevel -= 50f;
                if (targetWhiteNoiseLevel < 0) { targetWhiteNoiseLevel = 0; }
                break;


            case (Interference2D.Type.Audio):
                Debug.Log("Leaving Audio Interference" + other);
                targetAudioInterferenceLevel -= 50f;
                if (targetAudioInterferenceLevel < 0) { targetAudioInterferenceLevel = 0; }
                break;


            case (Interference2D.Type.Resistance):
                Debug.Log("Leaving Resistance Hack" + other);
                targetResistanceLevel -= 50f;
                if (targetResistanceLevel < 0) { targetResistanceLevel = 0; }
                break;

        }
    }

}
