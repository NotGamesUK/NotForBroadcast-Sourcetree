using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterferenceLow2D : MonoBehaviour {

    [Range(1f, 100f)]
    public float myInterferenceLevel = 50;
    public Sprite[] mySprites;
    [HideInInspector]
    public Interference2D myParent;
    private SpriteRenderer myRenderer;


    // Use this for initialization
    void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
        myParent = GetComponentInParent<Interference2D>();

    }

    // Update is called once per frame

    // Update is called once per frame
    void Update()
    {

    }

    public void SkinYourself(int thisSprite)
    {
        myRenderer.sprite = mySprites[thisSprite];
    }
}
