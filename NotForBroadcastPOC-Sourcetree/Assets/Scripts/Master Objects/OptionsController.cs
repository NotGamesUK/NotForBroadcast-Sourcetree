using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour {

    public Toggle myDevToggle;
    public Toggle myFreelookToggle;
    public Slider myMasterVolSlider;
    public Slider mySignalVolSlider;
    public Slider myRoomVolSlider;
    public Slider myMusicVolSlider;

    public GUIController myGUIController;

    private int thisToggleCheck;
    private float thisVolCheck;

    // Use this for initialization
    void Start() {
        Invoke("LoadAndSetOptions", 0.2f);
    }

    void LoadAndSetOptions() {

        if (PlayerPrefs.HasKey("DevMode"))
        {
            thisToggleCheck = PlayerPrefs.GetInt("DevMode");
            if (thisToggleCheck == 0)
            {
                myDevToggle.isOn = false;
                myGUIController.DevModeToggle(false);

            }
            else if (thisToggleCheck == 1)
            {
                myDevToggle.isOn = true;
                myGUIController.DevModeToggle(true);

            }

        }
        else
        {
            PlayerPrefs.SetInt("DevMode", 0);
            myGUIController.DevModeToggle(false);
        }

        if (PlayerPrefs.HasKey("FreeLook"))
        {
            thisToggleCheck = PlayerPrefs.GetInt("FreeLook");
            if (thisToggleCheck == 0)
            {
                myFreelookToggle.isOn = false;
                myGUIController.FreeLookToggle(false);

            }
            else if (thisToggleCheck == 1)
            {
                myFreelookToggle.isOn = true;
                myGUIController.FreeLookToggle(true);

            }

        }
        else
        {
            PlayerPrefs.SetInt("FreeLook", 0);
            myGUIController.FreeLookToggle(false);
        }

        if (PlayerPrefs.HasKey("MasterVol"))
        {
            myMasterVolSlider.value = PlayerPrefs.GetFloat("MasterVol");
        }
        else
        {
            PlayerPrefs.SetFloat("MasterVol", myMasterVolSlider.value);
        }

        if (PlayerPrefs.HasKey("SignalVol"))
        {
            mySignalVolSlider.value = PlayerPrefs.GetFloat("SignalVol");
        }
        else
        {
            PlayerPrefs.SetFloat("SignalVol", mySignalVolSlider.value);
        }

        if (PlayerPrefs.HasKey("RoomVol"))
        {
            myRoomVolSlider.value = PlayerPrefs.GetFloat("RoomVol");
        }
        else
        {
            PlayerPrefs.SetFloat("RoomVol", myRoomVolSlider.value);
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            myMusicVolSlider.value = PlayerPrefs.GetFloat("MusicVol");
        }
        else
        {
            PlayerPrefs.SetFloat("MusicVol", myMusicVolSlider.value);
        }

    }

    public void SaveAllOptions()
    {
        if (myDevToggle.isOn)
        {
            PlayerPrefs.SetInt("DevMode", 1);
        } else
        {
            PlayerPrefs.SetInt("DevMode", 0);
        }

        if (myFreelookToggle.isOn)
        {
            PlayerPrefs.SetInt("FreeLook", 1);
        }
        else
        {
            PlayerPrefs.SetInt("FreeLook", 0);
        }
        PlayerPrefs.SetFloat("MasterVol", myMasterVolSlider.value);
        PlayerPrefs.SetFloat("SignalVol", mySignalVolSlider.value);
        PlayerPrefs.SetFloat("RoomVol", myRoomVolSlider.value);
        PlayerPrefs.SetFloat("MusicVol", myMusicVolSlider.value);
        PlayerPrefs.Save();
    }



    // Update is called once per frame
    void Update () {
		
	}
}
