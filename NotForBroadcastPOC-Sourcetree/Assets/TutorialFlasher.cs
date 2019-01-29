using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlasher : MonoBehaviour
{
    Outline myOutline;
    public float myMinFlash = 2f;
    public float myMaxFlash = 7f;
    private float myTimer, myFlashStep, myStepDirection;
    private bool isFlashing;

    // Start is called before the first frame update
    void Start()
    {
        myOutline = GetComponent<Outline>();
        myOutline.OutlineWidth = 0;
        isFlashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFlashing)
        {
            myOutline.OutlineWidth += myFlashStep * myStepDirection * Time.deltaTime;
            if (myOutline.OutlineWidth < myMinFlash)
            {
                myOutline.OutlineWidth = myMinFlash;
                myStepDirection = 1;
            } else if (myOutline.OutlineWidth>myMaxFlash)
            {
                myOutline.OutlineWidth = myMaxFlash;
                myStepDirection = -1;
            }
        }
        myTimer -= Time.deltaTime;
        if (myTimer < 0)
        {
            isFlashing = false;
            myOutline.OutlineWidth = 0;

        }
    }

    public void FlashForXSeconds(float thisTime, float thisSpeed)
    {
        myFlashStep = (myMaxFlash-myMinFlash) / thisSpeed;
        isFlashing = true;
        myTimer = thisTime;
        myOutline.OutlineWidth = myMinFlash;
        myStepDirection = 1;
    }
}
