using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System; //This allows the IComparable Interface


[System.Serializable]
public class AdvertTape : IComparable<AdvertTape>
{
    public string advertName;
    public string tapeLabel;
    [HideInInspector]
    public float adNumber;
    public VideoClip smallVideo;
    public VideoClip largeVideo;
    public AudioClip adAudio;
    public Font labelFont;
    public Material labelMaterial;

    public int CompareTo(AdvertTape other)
    {
        if (other == null)
        {
            return 1;
        }

        int toSendback = 0;
        if (adNumber - other.adNumber < 0)
        {
            toSendback = -1;
        }
        else if (adNumber - other.adNumber > 0)
        {
            toSendback = 1;
        }
        return toSendback;
    }

}
