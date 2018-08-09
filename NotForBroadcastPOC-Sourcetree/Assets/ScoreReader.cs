using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;


public class ScoreReader : MonoBehaviour {

    public VideoPlayer myScreen;
    public AudioSource mySpeaker;
    public RenderTexture myVideoPlane;
    public RenderTexture myAudioPlane;
    public Text myUserInputText;
    public Text myStatusDisplay;
    public Text myVideoScore;
    public Text myAudioScore;
    public TextAsset mySequenceDataTest;

    private DataStorage myDataStore;
    private int maxScreen, currentScreen;
    private long lastFrame;
    private ScoringData.ScoreColour lastVideoColour;
    private ScoringData.ScoreColour lastAudioColour;
    private ScoringData.ScoreColour currentVideoColour;
    private ScoringData.ScoreColour currentAudioColour;
    private List<ScoringData> myScoringList = new List<ScoringData>();
    private string mySequenceName;
    private enum ScoreReaderMode { Dormant, Preparing, Reading, Saving }
    private ScoreReaderMode myMode;
    private DataStorage.SequenceData mySequenceData;
    private bool readVideoThisFrame, unSaved;

    // Use this for initialization
    void Start () {
        myStatusDisplay.text = "Waiting for User.";
        myMode = ScoreReaderMode.Dormant;
        myDataStore = FindObjectOfType<DataStorage>();

    }

    // Update is called once per frame
    void Update () {
		switch (myMode)
        {
            case ScoreReaderMode.Dormant:
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    TEMPReadTesting("01-01 Headlines");
                }

                break;

            case ScoreReaderMode.Preparing:
                if (myScreen.isPrepared)
                {
                    myMode = ScoreReaderMode.Reading;
                    myStatusDisplay.text = mySequenceName + " - Screen " + (currentScreen + 1) + " of " + maxScreen + ".";
                    myScreen.Play();
                    mySpeaker.Play();
                    lastFrame = 0;
                    myMode = ScoreReaderMode.Reading;
                }

                break;

            case ScoreReaderMode.Reading:

                if (myScreen.isPlaying)
                {
                    if (lastFrame != myScreen.frame)
                    {
                        lastAudioColour = currentAudioColour;
                        lastVideoColour = currentVideoColour;
                        lastFrame = myScreen.frame;

                        //if (readVideoThisFrame)
                        //{
                            // READ VIDEO SCORE

                            Texture2D testableTexture = toTexture2D(myVideoPlane);
                            Color testColor = testableTexture.GetPixel(0, 0);
                            //Debug.Log("Video Colour: " + testColor);
                            if (testColor.r == 1)
                            {
                                currentVideoColour = ScoringData.ScoreColour.Red;
                                myVideoScore.color = testColor;
                                myVideoScore.text = "RED";

                            }
                            else if (testColor.g == 1)
                            {
                                currentVideoColour = ScoringData.ScoreColour.Green;
                                myVideoScore.color = testColor;
                                myVideoScore.text = "GREEN";

                            }
                            else if (testColor.b == 1)
                            {
                                currentVideoColour = ScoringData.ScoreColour.Orange;
                                myVideoScore.color = testColor;
                                myVideoScore.text = "BLUE";

                            }

                            if (currentVideoColour != lastVideoColour)
                            {
                                myScoringList.Add(new ScoringData(myScreen.frame, ScoringData.ScoreType.Video, currentScreen, currentVideoColour));
                                Debug.Log("Added Video Change to " + currentVideoColour + " at Frame " + myScreen.frame);
                            }

                            readVideoThisFrame = false;
                            Destroy(testableTexture);

                        //}
                        //else
                        //{

                            Texture2D testableATexture = toTexture2D(myAudioPlane);
                            Color testAColor = testableATexture.GetPixel(0, 0);
                            //Debug.Log("Audio Colour: " + testColor);
                            if (testAColor.r == 1)
                            {
                                currentAudioColour = ScoringData.ScoreColour.Red;
                                myAudioScore.color = testAColor;
                                myAudioScore.text = "RED";

                            }
                            else if (testAColor.g == 1)
                            {
                                currentAudioColour = ScoringData.ScoreColour.Green;
                                myAudioScore.color = testAColor;
                                myAudioScore.text = "GREEN";

                            }
                            else if (testAColor.b == 1)
                            {
                                currentAudioColour = ScoringData.ScoreColour.Orange;
                                myAudioScore.color = testAColor;
                                myAudioScore.text = "BLUE";

                            }
                            if (currentAudioColour != lastAudioColour)
                            {
                                myScoringList.Add(new ScoringData(myScreen.frame, ScoringData.ScoreType.Audio, currentScreen, currentAudioColour));
                                Debug.Log("Added Audio Change to " + currentAudioColour + " at Frame " + myScreen.frame);

                            }

                            readVideoThisFrame = true;
                            Destroy(testableATexture);

                        //}


                    }

                }
                else // Screen has finished Playing
                {
                    currentScreen++;
                    if (currentScreen < maxScreen)
                    {
                        PrepareToReadScreen();
                    }
                    else
                    {
                        myStatusDisplay.text = "Writing Score File for "+mySequenceName;
                        myMode = ScoreReaderMode.Saving;
                        unSaved = true;
                    }

                }


                break;

            case ScoreReaderMode.Saving:

                if (unSaved)
                {
                    SaveListToTextFile();
                }

                break;


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

    public void PrepareToReadSequence()
    {
        mySequenceName = myUserInputText.text;
        mySequenceData = null;
        Debug.Log("Searching For: " + mySequenceName);
        foreach (DataStorage.SequenceData thisSequence in myDataStore.sequenceData)
        {
            if (thisSequence.SequenceName == mySequenceName)
            {
                mySequenceData = thisSequence;

                break;
            }
        }
        if (mySequenceData==null)
        {
            Debug.Log("SEQUENCE NOT FOUND.");
            myStatusDisplay.text = "ERROR - NOT FOUND.";
        } else
        {
            // Sequence Found - Continue....
            Debug.Log("Sequence Found.");

            currentScreen = 0;
            maxScreen = mySequenceData.screenVideo.Length;
            myScoringList.Clear();
            PrepareToReadScreen();
        }
    }

    void PrepareToReadScreen()
    {
        currentAudioColour = ScoringData.ScoreColour.Null;
        currentVideoColour = ScoringData.ScoreColour.Null;
        lastAudioColour = ScoringData.ScoreColour.Null;
        lastVideoColour = ScoringData.ScoreColour.Null;
        readVideoThisFrame = true;
        myScreen.clip = mySequenceData.screenVideo[currentScreen];
        int thisAudioClip = currentScreen;
        if (thisAudioClip >= mySequenceData.screenAudio.Length)
        {
            thisAudioClip = mySequenceData.screenAudio.Length - 1;
        }
        mySpeaker.clip = mySequenceData.screenAudio[thisAudioClip];
        myScreen.Prepare();
        myStatusDisplay.text = "PREPARING... ";
        myMode = ScoreReaderMode.Preparing;
    }

    void SaveListToTextFile()
    {
        myScoringList.Sort();
        StreamWriter myTextWriter = new StreamWriter(Application.persistentDataPath + "/Video Reader Output/"+mySequenceName+".txt"); // .nvgd (not games video data)
        Debug.Log("Creating File: "+mySequenceName+".txt");
        myTextWriter.WriteLine("Screen Scoring Data for "+mySequenceName);
        myTextWriter.WriteLine("");

        foreach (ScoringData thisLine in myScoringList)
        {
            string thisEditType="A";

            if (thisLine.editType == ScoringData.ScoreType.Video)
            {
                thisEditType = "V";
            }

            string thisColour = "R";

            if (thisLine.scoreColour == ScoringData.ScoreColour.Orange)
            {
                thisColour = "O";
            }
            else if (thisLine.scoreColour== ScoringData.ScoreColour.Green)
            {
                thisColour = "G";
            }

            string writeMe = thisEditType + thisLine.channelNumber + thisColour + thisLine.scoreFrame;
            Debug.Log("Writng " + writeMe + " to .txt file.");
            myTextWriter.WriteLine(writeMe);

        }
        myTextWriter.WriteLine("XXX");
        myTextWriter.Close();
        myStatusDisplay.text = "File "+mySequenceName+"Created.  Waiting for new Sequence name.";
        myUserInputText.text = "";
        myMode = ScoreReaderMode.Dormant;

    }

    void TEMPReadTesting(string thisFileName)
    {
        TextAsset thisScoringFile = (TextAsset)Resources.Load(thisFileName, typeof(TextAsset)); // "/NGVDs/"+thisFileName+".ngvd"
        StringReader myTextReader = new StringReader(thisScoringFile.text);
        if (myTextReader == null)
        {
            Debug.Log("File not found or not readable");
        }
        else
        {
            // Read each line from the file
            string thisLine = myTextReader.ReadLine();
            while (thisLine != "XXX") {
                Debug.Log("-->" + thisLine);
                thisLine = myTextReader.ReadLine();
            }
        }
    }
}
