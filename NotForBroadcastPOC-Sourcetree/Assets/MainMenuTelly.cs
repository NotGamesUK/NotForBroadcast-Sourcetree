using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class MainMenuTelly : MonoBehaviour {

    public Camera myFlickerCamera;
    public RenderTexture myFlickerPlane;
    public Light myFlickerLight;
    private VideoPlayer myScreen;
    

	// Use this for initialization
	void Start () {
        myScreen = GetComponent<VideoPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (myScreen.isPlaying)
        {
            Texture2D testableTexture = toTexture2D(myFlickerPlane);
            Color testColor = testableTexture.GetPixel(0, 0);
            Debug.Log("Test Colour: " + testColor);
            myFlickerLight.color = testColor;
            Destroy(testableTexture);
        }
    }

    public void PauseVideo()
    {
        if (myScreen.isPlaying)
        {
            myScreen.Pause();
        }
    }

    public void PlayVideo()
    {
        if (!myScreen.isPlaying)
        {
            myScreen.Play();
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
