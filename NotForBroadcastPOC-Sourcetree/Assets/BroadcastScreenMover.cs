using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastScreenMover : MonoBehaviour {

    public Camera myCamera;
    public float myTranslationFactor;

    private Vector3 homePosition;
    private Vector3 broadcastPosition;


	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {

	}

    public void MoveToBroadcastPosition()
    {
        transform.Translate(new Vector3(myTranslationFactor, -3.02f, 0f));
        //Debug.Log("My Broadcast Position: " + this.transform.position);

    }

    public void MoveToHomePosition()
    {
        transform.Translate(new Vector3(-myTranslationFactor, 3.02f, 0f));
        //Debug.Log("My Home Position: " + this.transform.position);

    }
}
