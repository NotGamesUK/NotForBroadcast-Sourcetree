using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudienceVUMeter : MonoBehaviour {

    public VUBar[] myBars;
    private float currentPercentage;
    private int loopStart, loopEnd;

    /// <summary>
    /// FOR TESTING:
    /// 
    public float testPercentageDELETEME = 60;
    /// </summary>
    // Use this for initialization
    void Start () {
        currentPercentage = 0;

        /// For Testing
        /// 
        Invoke("TEMPRandomVUAdjust", 0.2f);
	}

    void TEMPRandomVUAdjust()
    {
        testPercentageDELETEME += Random.Range(- 3, 3);
        if (testPercentageDELETEME<0) { testPercentageDELETEME = 0; }
        if (testPercentageDELETEME > 100) { testPercentageDELETEME = 100; }
        SetToPercentage(testPercentageDELETEME);
        Invoke("TEMPRandomVUAdjust", 0.2f);

    }

    public void SetToPercentage(float thisPercentage)
    {
        if (thisPercentage < currentPercentage)
        {
            loopStart = (int)Mathf.Round( thisPercentage / 2);
            loopEnd = (int)Mathf.Round(currentPercentage / 2);
            for (int n=loopStart; n<=loopEnd; n++)
            {
                myBars[n].LightOff();
            }
        } else if (thisPercentage > currentPercentage)
        {
            loopEnd = (int)Mathf.Round(thisPercentage / 2);
            loopStart = (int)Mathf.Round(currentPercentage / 2);
            for (int n = loopStart; n <= loopEnd; n++)
            {
                myBars[n].LightOn();
            }


        }
        currentPercentage = thisPercentage;
    }
}
