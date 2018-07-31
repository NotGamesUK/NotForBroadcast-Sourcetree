using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceVUMeter : MonoBehaviour {

    public VUBar[] myBars;

    private float currentPercentage;
    private int loopStart, loopEnd;



    // Use this for initialization
    void Start () {
        currentPercentage = 0;

	}


    public void SetToPercentage(float thisPercentage)
    {
        //Debug.Log("Setting VU Bar to " + thisPercentage + " percent.");
        if (thisPercentage < currentPercentage)
        {
            loopStart = (int)Mathf.Round( thisPercentage / 2);
            loopEnd = (int)Mathf.Round(currentPercentage / 2);
            for (int n=loopStart; n<loopEnd; n++)
            {
                myBars[n].LightOff();
            }
        } else if (thisPercentage > currentPercentage)
        {
            loopEnd = (int)Mathf.Round(thisPercentage / 2);
            loopStart = (int)Mathf.Round(currentPercentage / 2);
            for (int n = loopStart; n < loopEnd; n++)
            {
                myBars[n].LightOn();
            }


        }
        currentPercentage = thisPercentage;
    }
}
