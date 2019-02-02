using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanExtra : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void KillMeIn(int thisSeconds)
    {
        Invoke("KillMeNow", thisSeconds);
    }

    void KillMeNow()
    {
        Destroy(this.gameObject);
    }
}
