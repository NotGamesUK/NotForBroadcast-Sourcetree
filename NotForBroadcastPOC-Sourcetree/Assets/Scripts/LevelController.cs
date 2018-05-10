using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    [Range(0,0.3f)]
    public float cameraSpeed;

    private SignalControlCamera myCamera;
    public bool levelHasStarted = false;

	// Use this for initialization
	void Start () {
        myCamera = FindObjectOfType<SignalControlCamera>();

	}
	
	// Update is called once per frame
	void Update () {
		if (levelHasStarted)
        {
            myCamera.transform.Translate(Vector3.right * cameraSpeed * Time.deltaTime);
        }
	}
}
