using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuTitleJudder : MonoBehaviour {

    public Image titleStill;
    public Image titleJudder;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void JudderOn()
    {
        titleJudder.enabled = true;
        titleStill.enabled = false;

    }

    public void JudderOff()
    {
        titleJudder.enabled = false;
        titleStill.enabled = true;

    }
}
