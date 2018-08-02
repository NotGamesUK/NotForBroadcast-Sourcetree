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
    public AudioSource mySFXPlayer;
    public VUBar myBleepLight;
    public enum ScoringCameraType { Video, Audio, BleepMonitor }
    public ScoringCameraType myType;
    public int myAudioChannel;
    public float myIntialDelay;
    [HideInInspector]
    public bool screenChanged;


    private ScoringController myScoringController;
    private MasterController myMasterController;
    private Color lastColour;
    public enum ScoreColour { Red, Green, Orange, Null}
    [HideInInspector]
    public ScoreColour currentColour, lastScoreColour;
    private bool watchingChannel;
    [HideInInspector]
    public static int bleepWatchingCount;
    [HideInInspector]
    public static bool bleepWarningSoundPlayed;


    // Use this for initialization
    void Start () {
        currentColour = ScoreColour.Null;
        lastScoreColour = currentColour;
        Invoke("SetupSplitRead", myIntialDelay);
        myScoringController = FindObjectOfType<ScoringController>();
        myMasterController = FindObjectOfType<MasterController>();
        screenChanged = false;
        watchingChannel = false;
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
            if (lastColour != testColor || screenChanged)
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
                    if (lastScoreColour != currentColour)
                    {
                        Debug.Log("Video Colour Change from " + lastScoreColour + " to " + currentColour);
                        myScoringController.FootageColourChange(currentColour);
                        screenChanged = false;
                    }
                    else if (screenChanged)
                    {
                        Debug.Log("Screen Changed but colour HOLDS " + lastScoreColour + " still " + currentColour);
                        myScoringController.FootageCounterReset(currentColour);
                        screenChanged = false;
                    }
                }
                else if (myType == ScoringCameraType.Audio)
                {
                    if (lastScoreColour != currentColour)
                    {
                        myScoringController.AudioColourChange(currentColour, myAudioChannel);
                    }
                }
                else if (myType == ScoringCameraType.BleepMonitor)
                {
                    if (lastScoreColour != currentColour)
                    {
                        if (currentColour == ScoreColour.Red)
                        {
                            bleepWatchingCount++;
                            watchingChannel = true;
                        }
                        else if (watchingChannel)
                        {
                            if (!bleepWarningSoundPlayed)
                            {
                                if (!mySFXPlayer.isPlaying)
                                {
                                    mySFXPlayer.Play();
                                    
                                }
                            }
                            watchingChannel = false;
                            myBleepLight.LightOn();
                            Invoke("BleepLightOff", myMasterController.broadcastScreenDelayTime);
                        }
                    }
                }
            }

            lastColour = testColor;
            lastScoreColour = currentColour;
        }
	}

    void BleepLightOff()
    {
        bleepWatchingCount--;
        if (bleepWatchingCount <= 0)
        {
            bleepWatchingCount = 0;
            myBleepLight.LightOff();
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
