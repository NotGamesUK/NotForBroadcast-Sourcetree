using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    public Material switchOnMaterial;
    public Material mouseOverMaterialOn;
    public Material switchOffMaterial;
    public Material mouseOverMaterialOff;
    public Vector3 myRotation;
    public bool isOn = false;

    private MeshRenderer myRenderer;
    private bool isSelected;
    private Quaternion onRotation;
    private Quaternion offRotation;

    // Use this for initialization
    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        offRotation = this.transform.rotation;
        this.transform.Rotate(myRotation);
        onRotation = this.transform.rotation;
        this.transform.rotation = offRotation;
    }

    private void OnMouseEnter()
    {
        if (isOn)
        {
            myRenderer.material = mouseOverMaterialOn;
        } else
        {
            myRenderer.material = mouseOverMaterialOff;
        }
        isSelected = true;
    }

    private void OnMouseExit()
    {
        if (isOn)
        {
            myRenderer.material = switchOnMaterial;
        }
        else
        {
            myRenderer.material = switchOffMaterial;
        }
        isSelected = false;
    }

    private void OnMouseDown()
    {
        if (isSelected && isOn)
        {
            SwitchOff();
        } else
        {
            SwitchOn();
        }
    }

    private void OnMouseUp()
    {
        // Might be needed if controls slippy/over sensitive
    }

    public void SwitchOn()
    {
        isOn = true;
        this.transform.rotation = onRotation;
        if (isSelected)
        {
            myRenderer.material = mouseOverMaterialOn;
        } else
        {
            myRenderer.material = switchOnMaterial;
        }
    }

    public void SwitchOff()
    {
        isOn = false;
        this.transform.rotation = offRotation;
        if (isSelected)
        {
            myRenderer.material = mouseOverMaterialOff;
        }
        else
        {
            myRenderer.material = switchOffMaterial;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if (myDirection != 0)
        //{
        //    this.transform.Translate(myTranslation * (myDirection / speed));
        //    count++;
        //    if (count == speed)
        //    {
        //        if (myDirection == 1)
        //        {
        //            this.transform.position = startPosition;
        //            this.transform.Translate(myTranslation);
        //            if (isSelected)
        //            {
        //                myRenderer.material = mouseOverMaterialOff;
        //            }
        //            else
        //            {
        //                myRenderer.material = switchOffMaterial;
        //            }
        //        }
        //        else
        //        {
        //            this.transform.position = startPosition;
        //            if (isSelected)
        //            {
        //                myRenderer.material = mouseOverMaterialOn;
        //            }
        //            else
        //            {
        //                myRenderer.material = switchOnMaterial;
        //            }
        //        }
        //        count = 0;
        //        myDirection = 0;
        //    }
        //}
    }
}
