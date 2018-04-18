using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

    public Material buttonUpMaterial;
    public Material mouseOverMaterialUp;
    public Material buttonDownMaterial;
    public Material mouseOverMaterialDown;
    public enum type { Lock, Hold };
    public type buttonType;


    private MeshRenderer myRenderer;
    private Animator myAnimator;
    private bool isSelected = false;
    private bool isDepressed = false;
    private bool canRelease = false;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        myAnimator = GetComponent<Animator>();
	}

    private void OnMouseEnter()
    {
        myRenderer.material = mouseOverMaterialUp;
        if (buttonType==type.Lock && isDepressed)
        {
            myRenderer.material = mouseOverMaterialDown;
        }
        Debug.Log("Mouse Enter");
        isSelected = true;
    }

    private void OnMouseExit()
    {
        myRenderer.material = buttonUpMaterial;
        if (buttonType == type.Lock && isDepressed)
        {
            myRenderer.material = buttonDownMaterial;
            canRelease = true;
        }

        Debug.Log("Mouse Leave");
        isSelected = false;
        if (buttonType==type.Hold)
        {
            myAnimator.SetTrigger("released");
            isDepressed = false;
        }
    }

    private void OnMouseDown()
    {
        if (isSelected && !isDepressed)
        {
            myRenderer.material = buttonDownMaterial;
            Debug.Log("Button Clicked");
            myAnimator.SetTrigger("pressed");
            myAnimator.ResetTrigger("released");
            isDepressed = true;
            if (buttonType == type.Lock)
            {
                canRelease = false;
            }
        }
    }

    private void OnMouseUp()
    {
        if (buttonType == type.Hold && isSelected && isDepressed)
        {
            myAnimator.SetTrigger("released");
            myAnimator.ResetTrigger("pressed");
            myRenderer.material = mouseOverMaterialUp;
            isDepressed = false;
        }
        if (buttonType == type.Lock)
        {
            if (canRelease)
            {
                myAnimator.SetTrigger("released");
                myAnimator.ResetTrigger("pressed");
                myRenderer.material = mouseOverMaterialUp;
                isDepressed = false;
            }
            else
            {
                canRelease = true;
            }

        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
