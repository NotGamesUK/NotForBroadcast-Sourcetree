using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InTray : MonoBehaviour {

    public Sprite[] allResourcePages;
    public AudioClip myBeepSound;
    public AudioClip myChangePageSound;
    public AudioClip myChangeTraySound;
    public InTrayCollisionDetection myFaxTray;
    private GUIController myGUIController;
    private List<string> allFaxText;
    private List<float> allFaxFontSizes;
    [HideInInspector]
    public int numberOfFaxes, numberOfResourcePages, currentFax, currentResourcePage;
    private AudioSource mySFX;



	// Use this for initialization
	void Start () {
        allFaxText = new List<string>();
        allFaxFontSizes = new List<float>();
        mySFX = GetComponent<AudioSource>();
        myGUIController = FindObjectOfType<GUIController>();
        ResetMe();
	}
	

    public void ResetMe()
    {
        // Destroy all physical faxes in tray

        // Reset
        allFaxText.Clear();
        allFaxFontSizes.Clear();
        numberOfFaxes = 0;
        numberOfResourcePages = allResourcePages.Length;
        currentFax = 0;
        currentResourcePage = 0;
        myFaxTray.hasContents = false;
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void AddFaxToTray(string thisFaxBodyText, float thisFaxFontSize)
    {
        allFaxText.Add(thisFaxBodyText);
        allFaxFontSizes.Add(thisFaxFontSize);
        currentFax = allFaxText.Count - 1;
        myFaxTray.hasContents = true;
        numberOfFaxes++;
    }

    public bool ChangePage(int thisTray, int thisChange)
    {
        AudioClip thisSound = myChangePageSound;
        if (thisTray == 1)
        {
            currentFax += thisChange;
            if (currentFax>=numberOfFaxes || currentFax < 0)
            {
                currentFax -= thisChange;
                thisSound = myBeepSound;
            }
        }
        else
        {
            currentResourcePage += thisChange;
            if (currentResourcePage >= numberOfResourcePages || currentResourcePage < 0)
            {
                currentResourcePage -= thisChange;
                thisSound = myBeepSound;
            }
        }
        mySFX.clip = thisSound;
        mySFX.Play();
        if (thisSound == myBeepSound) { return false; } else { return true; }
    }


    public string ReturnTextFromTray()
    {
        return allFaxText[currentFax];
    }

    public float ReturnFontSizeFromTray()
    {
        return allFaxFontSizes[currentFax];
    }

    public Sprite ReturnImageFromTray()
    {
        return allResourcePages[currentResourcePage];
    }

    public void PlayTrayChangeSFX()
    {
        mySFX.clip = myChangeTraySound;
        mySFX.Play();
    }
}
