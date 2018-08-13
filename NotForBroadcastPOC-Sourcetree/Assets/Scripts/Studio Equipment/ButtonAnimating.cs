using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimating : MonoBehaviour {

    public Material buttonUpMaterial;
    public Material mouseOverMaterialUp;
    public Material buttonDownMaterial;
    public Material mouseOverMaterialDown;
    public Material buttonLockedMaterial;
    public Material mouseOverMaterialLocked;
    private Material defaultMaterial;
    public enum type { Lock, Hold };
    public type buttonType;
    public Vector3 myTranslation;
    public float speed=7;
    public bool hasPower;
    private bool lastPower;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private MeshRenderer myRenderer;
    private bool isSelected = false;
    public bool isDepressed = false;
    public bool isLocked = false;
    public bool oneWay = false;
    private bool canRelease = false;
    private bool keyHeld = false;
    private float myDirection;
    private int count = 0;

	// Use this for initialization
	void Start () {
        myRenderer = GetComponent<MeshRenderer>();
        startPosition = this.transform.position;
        defaultMaterial = GetComponent<MeshRenderer>().material;
        if (hasPower)
        {
            if (isDepressed)
            {
                myRenderer.material = buttonDownMaterial;
            }
            else
            {
                myRenderer.material = buttonUpMaterial;
            }
        }

    }

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
        if (isSelected)
        {
            if (isDepressed)
            {
                myRenderer.material = mouseOverMaterialDown;
            } else
            {
                myRenderer.material = buttonDownMaterial;
            }
        } else
        {
            if (isDepressed)
            {
                myRenderer.material = mouseOverMaterialUp;
            }
            else
            {
                myRenderer.material = buttonUpMaterial;
            }

        }
    }

    private void OnMouseEnter()
    {
        if (!(buttonType == type.Hold && keyHeld))
        {
            myRenderer.material = mouseOverMaterialUp;
            if (buttonType == type.Lock && isDepressed)
            {
                myRenderer.material = mouseOverMaterialDown;
            }
            isSelected = true;
        }
    }

    private void OnMouseExit()
    {
        if (!(buttonType == type.Hold && keyHeld))
        {

            myRenderer.material = buttonUpMaterial;
            if (buttonType == type.Lock && isDepressed)
            {
                myRenderer.material = buttonDownMaterial;
                canRelease = true;
            }

            isSelected = false;
            if (buttonType == type.Hold && isDepressed)
            {
                MoveUp();
            }
        }
    }

    private void OnMouseDown()
    {
        if (!(buttonType == type.Hold && keyHeld))
        {

            if (isSelected && !isDepressed && !isLocked)
            {
                MoveDown();
                if (buttonType == type.Lock)
                {
                    canRelease = false;
                }
            }
        }
    }

    public void KeyDown()
    {
        if (!isDepressed && !isLocked)
        {
            MoveDown();
            if (buttonType == type.Lock)
            {
                canRelease = false;
            }
        }
        if (buttonType == type.Hold)
        {
            keyHeld = true;
        }

    }

    public void KeyUp()
    {
        if (buttonType == type.Hold && isDepressed && !isLocked)
        {
            myRenderer.material = mouseOverMaterialUp;
            MoveUp();
        }
        if (buttonType == type.Lock && !isLocked && !oneWay)
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
        if (buttonType == type.Hold)
        {
            keyHeld = false;
        }

    }

    private void OnMouseUp()
    {
        if (!(buttonType == type.Hold && keyHeld))
        {

            if (buttonType == type.Hold && isSelected && isDepressed && !isLocked)
            {
                myRenderer.material = mouseOverMaterialUp;
                MoveUp();
            }
            if (buttonType == type.Lock && !isLocked && !oneWay)
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

        if (lastPower != hasPower)
        {
            if (hasPower)
            {
                if (isDepressed)
                {
                    myRenderer.material = buttonDownMaterial;
                }
                else
                {
                    myRenderer.material = buttonUpMaterial;
                }
            }
        }
        lastPower = hasPower;

    }

    private void LateUpdate()
    {
        if (!hasPower)
        {
            myRenderer.material = defaultMaterial;

        } else if (isLocked)
        {
            if (isSelected)
            {
                myRenderer.material = mouseOverMaterialLocked;
            } else
            {
                myRenderer.material = buttonLockedMaterial;
            }

        }
        
    }

}
