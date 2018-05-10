using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Interference : MonoBehaviour {

    public enum Type { Default, Audio, Resistance };
    public Type myType;

    public Material[] lowMaterials;
    public Material[] highMaterials;
    public Material[] borderMaterials;

    public VideoClip myVideo;
    public AudioClip myAudio;

    private InterferenceHigh myHigh;
    private InterferenceHighInterior myHighInterior;
    private InterferenceLow myLow;



    // Use this for initialization
    void Start () {
        myHigh = GetComponentInChildren<InterferenceHigh>();
        myLow = GetComponentInChildren<InterferenceLow>();
        myHighInterior = GetComponentInChildren<InterferenceHighInterior>();
        int thisSkin = 0;
        switch (myType){

            case (Type.Audio):
                thisSkin = 1;
                break;

            case (Type.Resistance):
                thisSkin = 2;
                break;

        }

        myHigh.SkinYourself(highMaterials[thisSkin]);
        myHighInterior.SkinYourself(borderMaterials[thisSkin]);
        myLow.SkinYourself(lowMaterials[thisSkin]);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
