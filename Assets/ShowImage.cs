using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using System;

public class ShowImage : MonoBehaviour, IInputClickHandler
{
    private bool imageShown = false;
    public void OnInputClicked(InputClickedEventData eventData)
    {
        imageShown = !imageShown;
        transform.Find("Image").gameObject.SetActive(imageShown);
    }

    // Use this for initialization
    void Start () {
        transform.Find("Image").gameObject.SetActive(imageShown);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
