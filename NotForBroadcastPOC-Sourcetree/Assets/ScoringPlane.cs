using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringPlane : MonoBehaviour {

    public RenderTexture myColourPlane;

    // FOR TESTING:

    public BackWallLight myRedLight;
    public BackWallLight myBlueLight;
    public BackWallLight myGreenLight;

    private Color lastColour;
    private enum ScoreColour { Red, Green, Blue, Null}
    private ScoreColour currentColour;


    // Use this for initialization
    void Start () {
        currentColour = ScoreColour.Null;
        InvokeRepeating("ReadTV", 1, 0.2f);
	}
	
	// Update is called once per frame
	void ReadTV () {
        Texture2D testableTexture = toTexture2D(myColourPlane);
        Color testColor = testableTexture.GetPixel (0, 0);
        Debug.Log("Current Colour: " + testColor);
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

                case ScoreColour.Blue:
                    myBlueLight.LightOff();
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
                currentColour = ScoreColour.Blue;
                myBlueLight.LightOn();
            }
        }

        lastColour = testColor;
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
