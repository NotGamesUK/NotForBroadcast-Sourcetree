using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

    private Material defaultMaterial;
    public Material switchOnMaterial;
    public Material mouseOverMaterialOn;
    public Material switchOffMaterial;
    public Material mouseOverMaterialOff;
    public Vector3 myRotation;
    public AudioClip mySwitchSFX;
    private AudioSource mySFX;
    public bool isOn = false;
    public bool hasPower = true;

    private MeshRenderer myRenderer;
    private bool isSelected;
    private Quaternion onRotation;
    private Quaternion offRotation;
    private bool lastPower;
    private bool startHasPower;

    // Use this for initialization
    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        mySFX = GetComponent<AudioSource>();
        mySFX.clip = mySwitchSFX;
        mySFX.loop = false;
        offRotation = this.transform.rotation;
        this.transform.Rotate(myRotation);
        onRotation = this.transform.rotation;
        this.transform.rotation = offRotation;
        defaultMaterial = GetComponent<MeshRenderer>().material;
        lastPower = hasPower;
        if (hasPower)
        {
            if (isOn)
            {
                myRenderer.material = switchOnMaterial;
            }
            else
            {
                myRenderer.material = switchOffMaterial;
            }
        }
        startHasPower = hasPower;

    }

    public void ResetMe()
    {
        hasPower = startHasPower;
        isOn = false;
        this.transform.rotation = offRotation;
        myRenderer.material = defaultMaterial;
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
        }
        else
        {
            SwitchOn();
        }
    }

    public void KeyDown()
    {
        if (isOn)
        {
            SwitchOff();
        }
        else
        {
            SwitchOn();
        }
    }

    public void SwitchOn()
    {
        isOn = true;
        //Debug.Log("SwitchScript - Switch On");
        this.transform.rotation = onRotation;
        if (isSelected)
        {
            myRenderer.material = mouseOverMaterialOn;
        } else
        {
            myRenderer.material = switchOnMaterial;
        }
        if (mySFX.isActiveAndEnabled)
        {
            mySFX.Play();

        }

    }

    public void SwitchOff()
    {
        isOn = false;
        //Debug.Log("SwitchScript - Switch Off");
        this.transform.rotation = offRotation;
        if (isSelected)
        {
            myRenderer.material = mouseOverMaterialOff;
        }
        else
        {
            myRenderer.material = switchOffMaterial;
        }
        if (mySFX.isActiveAndEnabled)
        {
            mySFX.Play();

        }

    }

    public void Update()
    {
        if (lastPower != hasPower)
        {
            if (hasPower)
            {
                if (isOn)
                {
                    myRenderer.material = switchOnMaterial;
                }
                else
                {
                    myRenderer.material = switchOffMaterial;
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
        }
    }
}
