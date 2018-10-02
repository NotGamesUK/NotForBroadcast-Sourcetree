using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InTrayCollisionDetection : MonoBehaviour {

    public int myID;
    public bool hasContents;
    public Material mouseoverMaterial;

    private Material defaultMaterial;
    private MeshRenderer myMeshRenderer;
    private GUIController myGUIGontroller;
    private bool isSelected, isTopTray;

	// Use this for initialization
	void Start () {
        myMeshRenderer = GetComponent<MeshRenderer>();
        myGUIGontroller = FindObjectOfType<GUIController>();
        defaultMaterial = myMeshRenderer.material;
        isTopTray = false;
        if (myID==1) { isTopTray = true; }
	}

    private void OnMouseEnter()
    {
        if (hasContents)
        {
            isSelected = true;
            myMeshRenderer.material = mouseoverMaterial;
        }
    }

    private void OnMouseExit()
    {
        if (isSelected)
        {
            myMeshRenderer.material = defaultMaterial;
            isSelected = false;
        }
    }

    private void OnMouseDown()
    {
        if (isSelected)
        {
            myGUIGontroller.ShowPaperwork(isTopTray);
            myMeshRenderer.material = defaultMaterial;
        }
    }
}
