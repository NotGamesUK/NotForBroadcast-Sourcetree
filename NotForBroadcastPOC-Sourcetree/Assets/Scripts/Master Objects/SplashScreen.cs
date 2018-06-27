using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SplashScreen : MonoBehaviour {

    // Use this for initialization
    private void Start()
    {
        Invoke("SplashScreenOver", 3);
    }

    void SplashScreenOver()
    {

        if (SceneManager.GetActiveScene().name == "00 Splash Screen")
        {
            SceneManager.LoadScene("01 Control Room", LoadSceneMode.Single);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
