using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollerBlind : MonoBehaviour
{

    public float myMinY;
    public float myMaxY;
    public float mySpeed;
    public ButtonAnimating myUpButton;
    public ButtonAnimating myDownButton;
    public AudioClip myMotorSound;


    private AudioSource mySFX;
    private bool isMoving;


    // Use this for initialization
    void Start()
    {
        mySFX = GetComponent<AudioSource>();
        mySFX.clip = myMotorSound;
        mySFX.loop = true;
        mySFX.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        isMoving = false;
        if (myUpButton.isDepressed && myUpButton.hasPower)
        {
            if (this.transform.position.y < myMaxY)
            {
                isMoving = true;
                this.transform.Translate(Vector3.up * mySpeed * Time.deltaTime, Space.World);
            }
            else
            {
                mySFX.Stop();
            }
        }

        if (myDownButton.isDepressed && myDownButton.hasPower)
        {
            if (this.transform.position.y > myMinY)
            {
                this.transform.Translate(Vector3.down * mySpeed * Time.deltaTime, Space.World);
                isMoving = true;
            }
        }
        if (isMoving)
        {
            if (!mySFX.isPlaying)
            {
                mySFX.Play();
            }
        } else
        {
            if (mySFX.isPlaying)
            {
                mySFX.Stop();
            }

        }



    }
}
