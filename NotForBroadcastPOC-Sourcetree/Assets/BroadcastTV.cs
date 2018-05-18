using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BroadcastTV : MonoBehaviour {

    public Material interferanceMaterial;
    public Material resistanceMaterial;
    public enum BroadcastModes { Live, Playback}
    public BroadcastModes myMode;

    private Color interferanceAlpha = new Color(1f, 1f, 1f, 0f);
    private Color resistanceAlpha = new Color(1f, 1f, 1f, 0f);
    private PlayerFrequencyDisplayObject myDot;
    private MasterGauges myGauges;
    private SoundDesk myDesk;
    private float lastWhiteNoiseLevel;
    private float lastResistanceLevel;
    private float lastAudioInterferenceLevel;

    // Use this for initialization
    void Start () {
        myDot = FindObjectOfType<PlayerFrequencyDisplayObject>();
        myGauges = FindObjectOfType<MasterGauges>();
        myDesk = FindObjectOfType<SoundDesk>();
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
