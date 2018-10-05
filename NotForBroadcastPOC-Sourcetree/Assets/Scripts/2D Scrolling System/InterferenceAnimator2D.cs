using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterferenceAnimator2D : MonoBehaviour {

    [Header("Bounce Settings:")]
    public bool isBouncing;
    [Range(0.05f,1f)]
    public float bounceUpMax=0.5f;
    [Range(0.05f, 1f)]
    public float bounceDownMax = 0.5f;
    [Tooltip("Number of seconds to go from Max to Min.")]
    public float bounceSpeed=5;
    public bool bounceUpFirst;

    [Space(5)]
    [Header("Spin Settings:")]
    public bool isSpinning;
    [Tooltip("Number of seconds to complete one full rotation.")]
    public float spinSpeed=10;
    public bool goAnticlockwise;

    [Space(5)]
    [Header("Scale Settings:")]
    public bool isScaling;
    [Range(0.05f, 10f)]
    public float maxScale=1.5f;
    [Range(0.05f, 10f)]
    public float minScale=0.5f;
    [Tooltip("Number of seconds to go from Max to Min.")]
    public float scaleSpeed = 10;
    public bool shrinkFirst;

    [Space(5)]
    [Header("Tilt Settings:")]
    public bool isTilting;
    [Range(0f, 360f)]
    public float tiltUpDegrees=90;
    [Range(0f, 360f)]
    public float tiltDownDegrees=90;
    [Tooltip("Number of seconds to go from Max to Min.")]
    public float tiltSpeed=10f;
    public bool startAnticlockwise;

    [Space(5)]
    [Header("Drift Settings:")]
    public bool isDrifting;
    [Range(0.05f, 5f)]
    public float driftLeftMax=0.5f;
    [Range(0.05f, 5f)]
    public float driftRightMax = 0.5f;
    [Range(0.05f, 1f)]
    public float driftUpMax = 0.5f;
    [Range(0.05f, 1f)]
    public float driftDownMax = 0.5f;
    [Tooltip("Number of seconds to go from Max to Min on X.")]
    public float driftSpeedX = 5f;
    [Tooltip("Number of seconds to go from Max to Min on Y.")]
    public float driftSpeedY = 5f;
    public bool driftRandomly;
    [Tooltip("Avarage number of seconds between direction changes.")]
    [Range (0.25f,3f)]
    public float driftRandomness=1.75f;

    private Vector3 startPosition, startRotation;
    private float bounceMaxY, bounceMinY, bouncePerSecond;
    private int bounceDirection;
    private float spinPerSecond;
    private int spinDirection;
    private float scalePerSecond;
    private int scaleDirection;
    private float tiltMaxRot, tiltMinRot, tiltPerSecond, currentTilt, tiltMonitor;
    private int tiltDirection;
    private float driftMaxX, driftMinX, driftMaxY, driftMinY, driftPerSecondX, driftPerSecondY;
    private int driftDirectionX, driftDirectionY;



    // Use this for initialization
    void Start () {

        startPosition = transform.position;
        startRotation = transform.rotation.eulerAngles;

        DoPreCalculations();
        if (isBouncing)
        {
            bounceDirection = -1;
            if (bounceUpFirst) { bounceDirection = 1; }
        }

        if (isSpinning)
        {
            spinDirection = -1;
            if (goAnticlockwise) { spinDirection = 1; }
        }

        if (isScaling)
        {
            scaleDirection = 1;
            if (shrinkFirst) { scaleDirection = -1; }
        }

        if (isTilting)
            tiltMonitor = 0;
        {
            tiltDirection = 1;
            if (startAnticlockwise) { tiltDirection = -1; }
        }

        if (isDrifting)
        {
            driftDirectionX = 1;
            if (Random.value > 0.5) { driftDirectionX = -1; }
            driftDirectionY = 1;
            if (Random.value > 0.5) { driftDirectionY = -1; }

        }



    }

    // Update is called once per frame
    void Update () {

        // Remove this to save cycles but dynamic adjustment will be disabled as a result:
        DoPreCalculations();
        //////////////////////////////////////////////////////////////////////////////////



        // Animate as Appropriate
        if (isBouncing)
        {
            transform.Translate(new Vector3 (0, bounceDirection * Time.deltaTime * bouncePerSecond, 0), Space.World);
            if (transform.position.y >= bounceMaxY)
            {
                transform.position = new Vector3(transform.position.x, bounceMaxY, transform.position.z);
                bounceDirection *= -1;
            }
            if (transform.position.y <= bounceMinY)
            {
                transform.position = new Vector3(transform.position.x, bounceMinY, transform.position.z);
                bounceDirection *= -1;
            }


        }

        if (isSpinning)
        {
            transform.Rotate(0,0, spinPerSecond * Time.deltaTime * spinDirection, Space.World);
        }

        if (isScaling)
        {
            float currentScale = transform.localScale.x;
            float newScale = currentScale + scalePerSecond * scaleDirection * Time.deltaTime;
            //Debug.Log("Current Scale: " + currentScale + "   New Scale: " + newScale + "   Max/Min: "+maxScale+"/"+minScale);
            if (newScale >= maxScale)
            {
                newScale = maxScale;
                scaleDirection *= -1;
            }
            if (newScale <= minScale)
            {
                newScale = minScale;
                scaleDirection *= -1;
            }
            transform.localScale+=new Vector3 (scalePerSecond * scaleDirection * Time.deltaTime, scalePerSecond * scaleDirection * Time.deltaTime, 0);
        }

        if (isTilting)
        {
            transform.Rotate(0, 0, tiltPerSecond * tiltDirection * Time.deltaTime, Space.World);
            tiltMonitor += tiltPerSecond * tiltDirection * Time.deltaTime;
            //float thisTilt = transform.rotation.eulerAngles.z;
            //Debug.Log("Current Tilt: " + thisTilt + "   Max/Min Tilt: " + tiltMaxRot + "/" + tiltMinRot + "  Monitor: "+tiltMonitor);
            if (tiltMonitor >= tiltUpDegrees)
            {
                Quaternion newRotation = Quaternion.Euler(0, 0, tiltMaxRot);
                tiltMonitor = tiltUpDegrees;
                transform.rotation = newRotation;
                tiltDirection *= -1;
            }
            if (tiltMonitor <= -tiltDownDegrees)
            {
                Quaternion newRotation = Quaternion.Euler(0, 0, tiltMinRot);
                tiltMonitor = -tiltDownDegrees;
                transform.rotation = newRotation;
                tiltDirection *= -1;
            }


        }

        if (isDrifting)
        {
            transform.Translate(new Vector3(driftDirectionX * Time.deltaTime * driftPerSecondX, driftDirectionY * Time.deltaTime * driftPerSecondY, 0), Space.World);

            if (transform.position.y >= driftMaxY)
            {
                transform.position = new Vector3(transform.position.x, driftMaxY, transform.position.z);
                driftDirectionY *= -1;
            }
            if (transform.position.y <= driftMinY)
            {
                transform.position = new Vector3(transform.position.x, driftMinY, transform.position.z);
                driftDirectionY *= -1;
            }
            if (transform.position.x >= driftMaxX)
            {
                transform.position = new Vector3(driftMaxX, transform.position.y, transform.position.z);
                driftDirectionX *= -1;
            }
            if (transform.position.x <= driftMinX)
            {
                transform.position = new Vector3(driftMinX, transform.position.y, transform.position.z);
                driftDirectionX *= -1;
            }

            // Randomly Change Drift Direction based on driftRandomness
            if (driftRandomly)
            {
                if (Time.deltaTime > Random.Range(0f, driftRandomness))
                {
                    //Debug.Log("Random Drift Change X");
                    driftDirectionX *= -1;
                }
                if (Time.deltaTime > Random.Range(0f, driftRandomness))
                {
                    //Debug.Log("Random Drift Change Y");
                    driftDirectionY *= -1;
                }
            }

        }

    }

    private void DoPreCalculations()
    {
        // Calculate Actual Parameters
        if (isBouncing)
        {
            bounceMaxY = startPosition.y + bounceUpMax;
            bounceMinY = startPosition.y - bounceDownMax;
            bouncePerSecond = (bounceMaxY - bounceMinY) / bounceSpeed;
            //Debug.Log("Bounce - Current Y: " + transform.position.y + "   Max Y: " + bounceMaxY + "   Min Y: "+bounceMinY+"   Per Scond Movement:" + bouncePerSecond);
        }

        if (isSpinning)
        {
            spinPerSecond = 360 / spinSpeed;
        }

        if (isScaling)
        {
            scalePerSecond = (maxScale - minScale) / scaleSpeed;
        }

        if (isTilting)
        {
            currentTilt = startRotation.z;
            tiltMaxRot = currentTilt + tiltUpDegrees;
            if (tiltMaxRot >= 360) { tiltMaxRot -= 360; }
            tiltMinRot = currentTilt - tiltDownDegrees;
            if (tiltMinRot < 0) { tiltMinRot += 360; }
            tiltPerSecond = (tiltMaxRot - tiltMinRot) / tiltSpeed;
        }

        if (isDrifting)
        {
            driftMaxX = startPosition.x + driftRightMax;
            driftMinX = startPosition.x - driftLeftMax;
            driftMaxY = startPosition.y + driftUpMax;
            driftMinY = startPosition.y - driftDownMax;
            driftPerSecondX = (driftMaxX - driftMinX) / driftSpeedX;
            driftPerSecondY = (driftMaxY - driftMinY) / driftSpeedY;

        }

    }
}
