using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class MainMenuTelly : MonoBehaviour {

    public Camera myFlickerCamera;
    public RenderTexture myFlickerPlane;
    public Light myFlickerLight;
    public Light myWallFlickerLight;
    public string[] frameColourChanges;
    public MenuTitleJudder myJudder;

    private VideoPlayer myScreen;
    private int nextChangeFrame, maxChangePosition, currentArrayPos, lastFrame;
    private Color baseColour, nextBaseColour;
    private string nextColour;

	// Use this for initialization
	void Start () {
        myScreen = GetComponent<VideoPlayer>();
        currentArrayPos = 0;
        maxChangePosition = frameColourChanges.Length;
        nextBaseColour = Color.white * 0.5f;
        nextColour = "W";
        nextChangeFrame = 0;
        baseColour = Color.white * 0.5f;
        lastFrame = 0;
        myJudder.JudderOn();
	}
	
	// Update is called once per frame
	void Update () {
		if (myScreen.isPlaying)
        {
            int thisFrame = (int)myScreen.frame;
            if (thisFrame < lastFrame)
            {
                currentArrayPos = 0;
                nextBaseColour = Color.white * 0.5f;
                nextChangeFrame = 0;
                baseColour = Color.white * 0.5f;
                nextColour = "W";
            }
            lastFrame = thisFrame;
            if (thisFrame > nextChangeFrame)
            {
                baseColour = nextBaseColour;
                if (nextColour == "W")
                {
                    myJudder.JudderOn();
                } else
                {
                    myJudder.JudderOff();
                }
                //while (thisFrame > nextChangeFrame)
                //{
                currentArrayPos++;
                nextColour = frameColourChanges[currentArrayPos].Substring(0, 1);
                string nextFrameString = frameColourChanges[currentArrayPos].Substring(1, 5);
                nextChangeFrame = int.Parse(nextFrameString);
                Debug.Log("Next Colour Code: " + nextColour + "  at frame: " + nextChangeFrame);
                //}
                switch (nextColour) {

                    case "W":
                        nextBaseColour = Color.white * 0.5f;
                        break;

                    case "R":
                        nextBaseColour = Color.red * 0.5f;
                        break;

                    case "B":
                        nextBaseColour = Color.blue * 0.5f;
                        break;

                    case "O":
                        nextBaseColour = new Color(1f,0.5f,0) * 0.5f;
                        break;

                    case "Y":
                        nextBaseColour = Color.yellow * 0.5f;
                        break;


                }

            }
            Texture2D testableTexture = toTexture2D(myFlickerPlane);
            Color testColor = testableTexture.GetPixel(0, 0);
            Color adjustedTestColour = baseColour+testColor;
            myFlickerLight.color = adjustedTestColour;
            myWallFlickerLight.color = adjustedTestColour;
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
