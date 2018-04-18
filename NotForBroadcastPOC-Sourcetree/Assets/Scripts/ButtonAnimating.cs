using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimating : MonoBehaviour {

    public Material buttonUpMaterial;
    public Material mouseOverMaterialUp;
    public Material buttonDownMaterial;
    public Material mouseOverMaterialDown;
    public enum type { Lock, Hold };
    public type buttonType;
    public Vector3 myTranslation;
    public float speed=7;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private MeshRenderer myRenderer;
    private bool isSelected = false;
    private bool isDepressed = false;
    private bool canRelease = false;
    private float myDirection;
    private int count = 0;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        startPosition = this.transform.position;
    }

    private void OnMouseEnter()
    {
        myRenderer.material = mouseOverMaterialUp;
        if (buttonType==type.Lock && isDepressed)
        {
            myRenderer.material = mouseOverMaterialDown;
        }
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

        isSelected = false;
        if (buttonType==type.Hold && isDepressed)
        {
            MoveUp();
        }
    }

    private void OnMouseDown()
    {
        if (isSelected && !isDepressed)
        {
            MoveDown();
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
            myRenderer.material = mouseOverMaterialUp;
            MoveUp();
        }
        if (buttonType == type.Lock)
        {
            if (canRelease)
            {
                MoveUp();
            }
            else
            {
                canRelease = true;
            }

        }

    }

    public void MoveUp()
    {
        myDirection = -1;
        isDepressed = false;
    }

    public void MoveDown()
    {
        myDirection = 1;
        isDepressed = true;
    }

    // Update is called once per frame
    void Update () {
		if (myDirection != 0)
        {
            this.transform.Translate(myTranslation*(myDirection/speed));
            count++;
            if (count == speed)
            {
                if (myDirection == 1) {
                    this.transform.position = startPosition;
                    this.transform.Translate(myTranslation);
                    if (isSelected)
                    {
                        myRenderer.material = mouseOverMaterialDown;
                    }
                    else
                    {
                        myRenderer.material = buttonDownMaterial;
                    }
                } else
                {
                    this.transform.position = startPosition;
                    if (isSelected)
                    {
                        myRenderer.material = mouseOverMaterialUp;
                    }
                    else
                    {
                        myRenderer.material = buttonUpMaterial;
                    }
                }
                count = 0;
                myDirection = 0;
            }
        }
	}
}
