using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWallLight : MonoBehaviour {

    public Material myOffMaterial;
    public Material myOnMaterial;
    public MeshRenderer myRenderer;

	// Use this for initialization
	void Start () {
        myRenderer.material = myOffMaterial;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LightOn()
    {
        Debug.Log("Light On Called");
        myRenderer.material = myOnMaterial;
    }

    public void LightOff()
    {
        Debug.Log("Light Off Called");
        myRenderer.material = myOffMaterial;
    }

}
