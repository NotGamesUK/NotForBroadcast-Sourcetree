using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class Interference2D : MonoBehaviour {

    public enum Type { Default, Audio, Resistance };
    public Type myType;

    public VideoClip myVideo;
    public AudioClip myAudio;

    private InterferenceHigh2D myHigh;
    private InterferenceLow2D myLow;
    private bool isUnskinned = true;



    // Use this for initialization
    void Start()
    {
        myHigh = GetComponentInChildren<InterferenceHigh2D>();
        myLow = GetComponentInChildren<InterferenceLow2D>();


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

            myHigh.SkinYourself(thisSkin);
            myLow.SkinYourself(thisSkin);
            isUnskinned = false;
        }
	}
}
