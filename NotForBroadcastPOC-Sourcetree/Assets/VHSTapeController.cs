using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VHSTapeController : MonoBehaviour
{

    public VHSTape[] myTapes;
    public List<AdvertTape> advertVideos;

    private string[] tapeNames;
    private AdvertTape thisAdvert;

    public void SetAllTapesFromString(string loadInstructions)
    {
        // Return any tapes currently loaded to home position
        foreach (VHSTape thisTaoe in myTapes)
        {
            thisTaoe.ReturnToShelf();
        }

        // break Text into substrings
        tapeNames = loadInstructions.Split(char.Parse("#"));
        Debug.Log("1:" + tapeNames[0] + ".  2:" + tapeNames[1] + ".  3:" + tapeNames[2]);

        // Loop through tapes
        for (int n = 0; n < 8; n++)
        {
            if (AssignVideoToTape(n, tapeNames[n]))
            {
                myTapes[n].SetMyFont(thisAdvert.labelFont, thisAdvert.labelMaterial);
                myTapes[n].RewriteLabel();
                ShowTape(n);
                Debug.Log("Setting Tape " + n + " to " + thisAdvert.tapeLabel);
            }
            // Tell tape to Reset labeltext if necessary
            else
            {
                // Hide any unused tapes
                HideTape(n);
                Debug.Log("Hiding Tape " + n + " because label is " + tapeNames[n]);
            }

        }
    }

    public bool AssignVideoToTape(int thisTape, string thisAdName)
    {
        thisAdvert = null;
        if (thisAdName != "XXX")
        {
            for (int m = 0; m < advertVideos.Count; m++)
            {
                if (advertVideos[m].advertName == thisAdName)
                {
                    thisAdvert = advertVideos[m];
                }
            }
            if (thisAdvert != null)
            {
                // Set Label Text VideoReferences
                myTapes[thisTape].myTitle = thisAdvert.tapeLabel;
                myTapes[thisTape].myVideo = thisAdvert.largeVideo;
                myTapes[thisTape].myVideo = thisAdvert.smallVideo;
                myTapes[thisTape].myAudio = thisAdvert.adAudio;
                return true;
            }

        }
        return false;

    }

    public void HideTape(int thisTape)
    {
        myTapes[thisTape].gameObject.SetActive(false);
    }

    public void ShowTape(int thisTape)
    {
        myTapes[thisTape].gameObject.SetActive(true);

    }


}
