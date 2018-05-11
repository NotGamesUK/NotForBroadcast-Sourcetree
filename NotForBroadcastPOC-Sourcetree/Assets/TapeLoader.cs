using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapeLoader : MonoBehaviour {

    private bool isLoading = false;
    private bool isEjecting = false;
    private VHSTape myTape;
    public float ejectDistance=0.07f;
    public float loadSpeed=2f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (isLoading)
        {
            Debug.Log("ME: "+this.transform.position.z+"   TAPE: "+myTape.transform.position.z);
            myTape.transform.Translate(-(ejectDistance / loadSpeed) * Time.deltaTime, 0f, 0f);
            if (myTape.transform.position.z > this.transform.position.z)
            {
                isLoading = false;
                myTape.transform.position = this.transform.position;

            }
        }
    }

    public void LoadTape(VHSTape thisTape)
    {
        myTape = thisTape;
        myTape.transform.position = this.transform.position;
        myTape.transform.Translate(ejectDistance, 0f, 0f);
        isLoading = true;
    }
}
