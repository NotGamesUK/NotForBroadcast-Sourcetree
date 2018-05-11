﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterferenceLow : MonoBehaviour {

    [Range (1f,100f)]
    public float myInterferenceLevel=50;
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