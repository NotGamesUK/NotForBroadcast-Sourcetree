using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class BroadcastTV : MonoBehaviour {

    public Material interferanceMaterial;
    public Material resistanceMaterial;
    public Material advertMaterial;
    public VideoPlayer[] myScreens;
    public AudioSource[] myScreenAudioSources;
    public VideoPlayer myAdvertScreen;
    public AudioSource myAdvertAudiosource;
    public VideoPlayer myResistanceScreen;
    public AudioSource myResistanceAudiosource;
    public VideoPlayer myWhiteNoiseScreen;
    public AudioSource myWhiteNoiseAudiosource;
    public AudioSource myAudioInterferenceAudiosource;

    public enum BroadcastModes { Live, Playback }
    public BroadcastModes myMode;

    private Color interferanceAlpha = new Color(1f, 1f, 1f, 0f);
    private Color resistanceAlpha = new Color(1f, 1f, 1f, 0f);
    private Color advertAlpha = new Color(1f, 1f, 1f, 0f);

    private PlayerFrequencyDisplayObject myDot;
    private MasterGauges myGauges;
    private SoundDesk myDesk;
    private SequenceController mySequenceController;
    private MasterController myMasterController;
    private MeshRenderer myNoSignal;
    private bool[] screenPreparing = new bool[4];
    private bool adPreparing;
    private bool resistancePreparing;
    private float lastWhiteNoiseLevel;
    private float lastResistanceLevel;
    private float lastAudioInterferenceLevel;
    private int maxScreen;
    private int requestedScreen;
    public int currentScreen;

    // Use this for initialization
    void Start () {
        myDot = FindObjectOfType<PlayerFrequencyDisplayObject>();
        myGauges = FindObjectOfType<MasterGauges>();
        myDesk = FindObjectOfType<SoundDesk>();
        myNoSignal = GetComponentInChildren<NoSignal>().GetComponent<MeshRenderer>();
        mySequenceController = FindObjectOfType<SequenceController>();
        myMasterController = FindObjectOfType<MasterController>();
        SetResistanceVideoLevel(0);
        SetWhiteNoiseVideoLevel(0);
	}
	
	// Update is called once per frame
	void Update () {
		switch (myMode)
        {
            case (BroadcastModes.Live):
                // Monitor video intereference levels

                // Check Master Gauges for White Noise Level
                float thisWhiteNoiseLevel = myGauges.videoStrength;
                if (thisWhiteNoiseLevel != lastWhiteNoiseLevel)
                {
                    SetWhiteNoiseVideoLevel(thisWhiteNoiseLevel);
                    myDesk.SetWhiteNoiseAudioLevel(thisWhiteNoiseLevel);
                    // Log Change to EDL
                }
                lastWhiteNoiseLevel = thisWhiteNoiseLevel;

                // Check Signal Control Dot for Resistance Level
                float thisResistanceLevel = myDot.currentResistanceLevel;
                if (thisResistanceLevel != lastResistanceLevel)
                {
                    SetResistanceVideoLevel(thisResistanceLevel);
                    myDesk.SetResistanceAudioLevel(thisResistanceLevel);
                    // Log Change to EDL
                }
                lastResistanceLevel = thisResistanceLevel;

                // Check Signal Control Dot for Audio Interference Level
                float thisAudioInterferenceLevel = myDot.currentAudioInterferenceLevel;
                if (thisAudioInterferenceLevel != lastAudioInterferenceLevel)
                {
                    myDesk.SetAudioInterferenceLevel(thisAudioInterferenceLevel);
                    // Log Change to EDL
                }
                lastAudioInterferenceLevel = thisAudioInterferenceLevel;

                break;

            case (BroadcastModes.Playback):
                // Take video intereference levels from EDL


                break;

        }

        // Prepare Monitoring

        if (adPreparing)
        {
            if (myAdvertScreen.isPrepared)
            {
                adPreparing = false;
                myMasterController.preparingAd = false;
                //Debug.Log("Broadcast TV: Advert Prepared.");
            }
        }

        if (resistancePreparing)
        {
            if (myResistanceScreen.isPrepared)
            {
                resistancePreparing = false;
                mySequenceController.preparedScreensCount++;
            }
        }

        for (int n=0; n<4; n++)
        {
            if (screenPreparing[n])
            {
                if (myScreens[n].isPrepared)
                {
                    screenPreparing[n] = false;
                    mySequenceController.preparedScreensCount++;
                }
            }
        }
    }


    public void ScreenChange(int thisScreen)
    {
        //myDesk.TEMPORARYSetBroadcastChannel(requestedScreen);
        requestedScreen = thisScreen;

        // Move Current Screen backwards
        if (currentScreen==0)
        {
            myNoSignal.enabled = false;
            // Bring Selected Screen To Front (0.042)
            myScreens[requestedScreen - 1].transform.Translate(new Vector3(0f, 0.02f, 0f));
            // Tell Desk to Select correct Channel
            //myDesk.TEMPORARYSetBroadcastChannel(requestedScreen);
            //Debug.Log("Changed Broadcast Screen From NO SIGNAL to " + requestedScreen);
        }
        else
        {
            // Move Selected screen forwards and previous screen backwards
            myScreens[currentScreen - 1].transform.Translate(new Vector3(0f, -0.02f, 0f));
            // Bring Selected Screen To Front (0.042)
            myScreens[requestedScreen - 1].transform.Translate(new Vector3(0f, 0.02f, 0f));
            // Tell Desk to Select correct Channel
            //myDesk.TEMPORARYSetBroadcastChannel(requestedScreen);
            // Change Current Screen to Selected Screen
        }
        currentScreen = requestedScreen;
        //Debug.Log("Switched Broadcast Screen to " + currentScreen);

    }

    public void EndAdvertAndStartLiveBroadcast()
    {
        //Debug.Log("BROADCAST TV: Starting Live Broadcast");
        // Stop and Hide Advert Screen
        myAdvertScreen.Stop();
        advertAlpha.a = 0;
        advertMaterial.color = advertAlpha;
        myDesk.SwitchToBroadcastLiveSound();
        if (currentScreen != 0)
        {
            // Bring Selected Screen To Front (0.042)
            myScreens[currentScreen - 1].transform.Translate(new Vector3(0f, 0.002f, 0f));
            // Tell Desk to Select correct Channel
            //myDesk.TEMPORARYSetBroadcastChannel(currentScreen);

        } else
        {
            // If no Screen is selected show NoSignal
            myNoSignal.enabled = true;
            //myDesk.TEMPORARYSetBroadcastChannel(0);
        }
    }

    public void PrepareAdvert(VideoClip thisAdClip, AudioClip thisAdAudioClip)
    {
        //Debug.Log("BROADCAST TV: Preparing " + thisAdClip + " on "+myAdvertScreen);
        myAdvertScreen.Stop();
        myAdvertScreen.clip = thisAdClip;
        myAdvertScreen.isLooping = false;
        myAdvertScreen.Prepare();
        myAdvertAudiosource.Stop();
        myAdvertAudiosource.clip = thisAdAudioClip;
        myAdvertAudiosource.loop = false;
        adPreparing = true;
    }

    public void PlayAdvert()
    {
        //Debug.Log("Broadcast TV: Playing Advert.");
        advertAlpha.a = 1f;
        advertMaterial.color = advertAlpha;
        myAdvertScreen.Play();
        myAdvertAudiosource.Play();
        myNoSignal.enabled = false;
        myDesk.SwitchToAdvertSound();
    }

    public void PlayPreLoop(VideoClip thisPreLoop, AudioClip thisPreLoopAudio)
    {
        advertAlpha.a = 1f;
        advertMaterial.color = advertAlpha;
        myAdvertScreen.clip = thisPreLoop;
        myAdvertScreen.isLooping = true;
        myAdvertScreen.Play();
        myAdvertAudiosource.clip = thisPreLoopAudio;
        myAdvertAudiosource.loop = true;
        myAdvertAudiosource.Play();
        myNoSignal.enabled = false;
        myDesk.SwitchToAdvertSound();

    }

    public void PrepareResistance(VideoClip thisResistClip, AudioClip thisResistAudioClip)
    {
        //Debug.Log("BROADCAST TV: Preparing " + thisResistClip + " on " + myResistanceScreen);
        myResistanceScreen.Stop();
        myResistanceScreen.clip = thisResistClip;
        myResistanceScreen.Prepare();
        myResistanceAudiosource.Stop();
        myResistanceAudiosource.clip = thisResistAudioClip;
        resistancePreparing = true;
    }

    public void PlayResistance()
    {
        myResistanceScreen.Play();
        myResistanceAudiosource.Play();
    }

    public void PrepareScreens(VideoClip[] theseClips, AudioClip[] theseAudioClips, AudioClip thisInterferenceAudioclip)
    {
        maxScreen = theseClips.Length;
        //Debug.Log("Broadcast TV Number of Screens Preparing: " + maxScreen);
        for (int n = 0; n < maxScreen; n++)
        {
            AudioClip thisAudioClip = theseAudioClips[0];
            if (theseAudioClips.Length > 1)
            {
                thisAudioClip = theseAudioClips[n];
            }
            myScreens[n].Stop();
            myScreens[n].clip = theseClips[n];
            myScreens[n].Prepare();
            screenPreparing[n] = true;
            myScreenAudioSources[n].Stop();
            myScreenAudioSources[n].clip = thisAudioClip;
        }
        myAudioInterferenceAudiosource.clip = thisInterferenceAudioclip;

    }

    public void PlayScreens()
    {
        for (int n = 0; n < maxScreen; n++)
        {
            myScreens[n].Play();
            myScreenAudioSources[n].Play();

        }
        // Start White Noise
        myWhiteNoiseAudiosource.Play();
        myWhiteNoiseAudiosource.loop = true;
        myWhiteNoiseScreen.Play();
        myWhiteNoiseScreen.isLooping = true;
        // Start Audio Interference
        if (myAudioInterferenceAudiosource.clip)
        {
            myAudioInterferenceAudiosource.Play();
        }
    }

    public void SetWhiteNoiseVideoLevel (float thisStrength) 
    {
        // CALLED FROM MASTER GAUGES ONCE INTERFERENCE HAS BEEN CALCULATED
        // WILL ALSO BE CALLED BY EDL READER ON PLAYBACK

        // Adjust alpha of interference screen based on Video Signal level
        interferanceAlpha.a = 1 - (thisStrength / 100);
        //Debug.Log("Colour: " + interferanceAlpha);
        interferanceMaterial.color = interferanceAlpha;

    }

    public void SetResistanceVideoLevel(float thisStrength)
    {
        resistanceAlpha.a = (thisStrength / 100);
        //Debug.Log("Colour: " + interferanceAlpha);
        resistanceMaterial.color = resistanceAlpha;
    }

}
