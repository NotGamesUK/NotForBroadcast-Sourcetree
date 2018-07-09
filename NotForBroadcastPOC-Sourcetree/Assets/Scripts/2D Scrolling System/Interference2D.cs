using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Interference2D : MonoBehaviour {

    public enum Type { Default, Audio, Resistance };
    public Type myType;

    public VideoClip myVideo;
    public AudioClip myAudio;

    private InterferenceHigh2D[] myHighs;
    private InterferenceLow2D[] myLows;
    private bool isUnskinned = true;



    // Use this for initialization
    void Start()
    {
        myHighs = GetComponentsInChildren<InterferenceHigh2D>();
        myLows = GetComponentsInChildren<InterferenceLow2D>();


    }

    // Update is called once per frame
    void Update () {
		if (isUnskinned)
        {
            int thisSkin = 0;
            switch (myType)
            {

                case (Type.Audio):
                    thisSkin = 1;
                    break;

                case (Type.Resistance):
                    thisSkin = 2;
                    break;

            }

            foreach (InterferenceHigh2D thisHigh in myHighs)
            {
                thisHigh.SkinYourself(thisSkin);
            }
            foreach (InterferenceLow2D thisLow in myLows)
            {
                thisLow.SkinYourself(thisSkin);
                isUnskinned = false;
            }
        }
	}
}
