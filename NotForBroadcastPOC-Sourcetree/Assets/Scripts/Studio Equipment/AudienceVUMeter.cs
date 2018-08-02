using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceVUMeter : MonoBehaviour {

    public AudioClip myDownSound;
    public AudioClip myUpSound;
    public VUBar[] myBars;

    private float currentPercentage;
    private int loopStart, loopEnd;
    private AudioSource mySFXPlayer;



    // Use this for initialization
    void Start () {
        currentPercentage = 0;
        mySFXPlayer = GetComponent<AudioSource>();
	}


    public void SetToPercentage(float thisPercentage)
    {
        bool soundPlayed = false;
        //Debug.Log("Setting VU Bar to " + thisPercentage + " percent.");
        if (thisPercentage < currentPercentage)
        {
            loopStart = (int)Mathf.Round( thisPercentage / 2);
            loopEnd = (int)Mathf.Round(currentPercentage / 2);
            for (int n=loopStart; n<loopEnd; n++)
            {
                myBars[n].LightOff();
                if (!soundPlayed)
                {
                    mySFXPlayer.clip = myDownSound;
                    mySFXPlayer.Play();
                    soundPlayed = true;
                }
            }
        } else if (thisPercentage > currentPercentage)
        {
            loopEnd = (int)Mathf.Round(thisPercentage / 2);
            loopStart = (int)Mathf.Round(currentPercentage / 2);
            for (int n = loopStart; n < loopEnd; n++)
            {
                myBars[n].LightOn();
                if (!soundPlayed)
                {
                    mySFXPlayer.clip = myUpSound;
                    mySFXPlayer.Play();
                    soundPlayed = true;
                }

            }


        }
        currentPercentage = thisPercentage;
    }
}
