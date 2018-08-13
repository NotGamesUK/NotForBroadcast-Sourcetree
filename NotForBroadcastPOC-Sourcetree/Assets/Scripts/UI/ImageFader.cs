using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageFader : MonoBehaviour {

    public float myFadeSpeed;
    private Image image;
    private float targetAlpha;

    // Use this for initialization
    void Start()
    {
        this.image = this.GetComponent<Image>();
        if (this.image == null)
        {
            Debug.LogError("Error: No image on " + this.name);
        }
        this.targetAlpha = this.image.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        Color curColor = this.image.color;
        float alphaDiff = Mathf.Abs(curColor.a - this.targetAlpha);
        if (alphaDiff > 0.0001f)
        {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, this.myFadeSpeed * Time.deltaTime);
            this.image.color = curColor;
        }

    }

    public void FlashMe()
    {
        myFadeSpeed = 30;
        FadeIn();
        Invoke("FlashPartTwo", 0.1f);


    }

    void FlashPartTwo()
    {
        myFadeSpeed = 5;
        FadeOut();
    }

    public void FadeOut()
    {
        this.targetAlpha = 0.0f;
    }

    public void FadeIn()
    {
        this.targetAlpha = 1.0f;
    }
}