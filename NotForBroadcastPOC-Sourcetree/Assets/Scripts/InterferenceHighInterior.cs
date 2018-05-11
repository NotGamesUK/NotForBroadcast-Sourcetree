using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterferenceHighInterior : MonoBehaviour {

    private MeshRenderer myRenderer;
    // Use this for initialization
    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SkinYourself(Material thisMaterial)
    {
        myRenderer.material = thisMaterial;
    }
}
