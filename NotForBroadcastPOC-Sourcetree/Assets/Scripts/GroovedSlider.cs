﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroovedSlider : MonoBehaviour {

    public Material buttonUpMaterial;
    public Material mouseOverMaterialUp;
    public Material buttonDownMaterial;
    public Material mouseOverMaterialDown;
    public Vector3 myTranslation;
    public Slider mySlider;

    private Vector3 topPosition;
    private Vector3 bottomPosition;
    private float yChange;
    private float zChange;
    private MeshRenderer myRenderer;
    private bool isSelected = false;
    private bool isGrabbed = false;

    // Use this for initialization
    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        topPosition = this.transform.position;
        this.transform.Translate(myTranslation);
        bottomPosition = this.transform.position;
        yChange = topPosition.y - bottomPosition.y;
        zChange = topPosition.z - bottomPosition.z;
        // Move into alignment with slider
        float thisSliderPos = mySlider.value;
        float newX = bottomPosition.x;
        float newY = bottomPosition.y + (yChange * thisSliderPos);
        float newZ = bottomPosition.z + (zChange * thisSliderPos);
        this.transform.position = new Vector3(newX, newY, newZ);


    }

    private void OnMouseEnter()
    {
        if (!isGrabbed)
        {
            myRenderer.material = mouseOverMaterialUp;
        }
        isSelected = true;
    }

    private void OnMouseExit()
    {
        if (!isGrabbed)
        {
            myRenderer.material = buttonUpMaterial;
        }
        isSelected = false;
    }

    private void OnMouseDown()
    {
            myRenderer.material = mouseOverMaterialDown;
            isGrabbed = true;
    }

    private void OnMouseUp()
    {
        if (isSelected)
        {
            myRenderer.material = mouseOverMaterialUp;
        }
        else
        {
            myRenderer.material = buttonUpMaterial;
        }
        isGrabbed = false;
    }


    public void sliderMoved(float thisSliderPos)
    {
        // Adjust 3D slider
        float newX = bottomPosition.x;
        float newY = bottomPosition.y + (yChange * thisSliderPos);
        float newZ = bottomPosition.z + (zChange * thisSliderPos);
        this.transform.position = new Vector3(newX, newY, newZ);
        // Send new value upwards
        Debug.Log("Slider Moved to " + thisSliderPos);
    }
}
