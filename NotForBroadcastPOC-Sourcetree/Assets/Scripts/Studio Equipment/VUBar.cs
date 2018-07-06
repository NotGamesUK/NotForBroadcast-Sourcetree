using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VUBar : MonoBehaviour {

    public Material myOffMaterial;
    public Material myOnMaterial;
    private MeshRenderer myRenderer;
    [HideInInspector]
    public bool isOn;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.material = myOffMaterial;
        isOn = false;
	}

    // Update is called once per frame
    public void LightOn()
    {
        myRenderer.material = myOnMaterial;
        isOn = true;
    }

    public void LightOff()
    {
        myRenderer.material = myOffMaterial;
        isOn = false;
    }

}
