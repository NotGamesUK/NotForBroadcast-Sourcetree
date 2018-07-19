using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ScoringPlane : MonoBehaviour {

    public RenderTexture myColourPlane;

    // FOR TESTING:

    public BackWallLight myRedLight;
    public BackWallLight myOrangeLight;
    public BackWallLight myGreenLight;
    public enum ScoringCameraType { Video, Audio }
    public ScoringCameraType myType;
    public int myAudioChannel;
    public float myIntialDelay;

    private ScoringController myScoringController;
    private Color lastColour;
    public enum ScoreColour { Red, Green, Orange, Null}
    [HideInInspector]
    public ScoreColour currentColour;


    // Use this for initialization
    void Start () {
        currentColour = ScoreColour.Null;
        Invoke("SetupSplitRead", myIntialDelay);
        myScoringController = FindObjectOfType<ScoringController>();
	}

    void SetupSplitRead()
    {
        InvokeRepeating("ReadTV", 1, 0.5f);
    }

    // Update is called once per frame
    void ReadTV () {
        if (myScoringController.broadcastScreensLive)
        {
            Texture2D testableTexture = toTexture2D(myColourPlane);
            Color testColor = testableTexture.GetPixel(0, 0);
            //Debug.Log("Current Colour: " + testColor);
            if (lastColour != testColor)
            {
                // Adjust Lights as necessary
                switch (currentColour)
                {
                    case ScoreColour.Red:
                        myRedLight.LightOff();
                        break;

                    case ScoreColour.Green:
                        myGreenLight.LightOff();
                        break;

                    case ScoreColour.Orange:
                        myOrangeLight.LightOff();
                        break;
                }
                if (testColor.r == 1)
                {
                    currentColour = ScoreColour.Red;
                    myRedLight.LightOn();
                }
                else if (testColor.g == 1)
                {
                    currentColour = ScoreColour.Green;
                    myGreenLight.LightOn();
                }
                else if (testColor.b == 1)
                {
                    currentColour = ScoreColour.Orange;
                    myOrangeLight.LightOn();
                }

                if (myType == ScoringCameraType.Video)
                {
                    myScoringController.FootageColourChange(currentColour);
                } else if (myType == ScoringCameraType.Audio)
                {
                    myScoringController.AudioColourChange(currentColour, myAudioChannel);
                }
            }

            lastColour = testColor;
        }
	}

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();
        return tex;
    }
}
