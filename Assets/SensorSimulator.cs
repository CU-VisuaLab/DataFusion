using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HoloToolkit.Unity.InputModule;
using System;

public class SensorSimulator : MonoBehaviour, IInputClickHandler
{
    private string visShown = "TemperatureReadingImage";
    private System.Random rnd;
    private int secondCount;
    private float lastTemp;
    private float newPercentage;
    private float lastPercentage;

    public float[] temperatureRange;
    public GameObject lineGraphSegment;

    public void OnInputClicked(InputClickedEventData eventData)
    {
        transform.Find("VisCanvas").Find(visShown).gameObject.SetActive(false);
        if (visShown == "TemperatureReadingImage")
        {
            visShown = "LiveGraph";
            transform.Find("VisCanvas").Find("TemperatureReading").gameObject.SetActive(false);
        }
        else
        {
            visShown = "TemperatureReadingImage";
            transform.Find("VisCanvas").Find("TemperatureReading").gameObject.SetActive(true);
        }
        transform.Find("VisCanvas").Find(visShown).gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start () {
        rnd = new System.Random();
        InvokeRepeating("UpdateTemperature", 0.0f, 2.0f);
        secondCount = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void UpdateTemperature()
    {
        newPercentage = (rnd.Next(100) / 100f);
        float newTemperature = newPercentage * (temperatureRange[1] - temperatureRange[0]) + temperatureRange[0];
        transform.Find("VisCanvas").Find("TemperatureReading").GetComponent<Text>().text = "Temperature: " + string.Format("{0:0.00}", newTemperature) + "C";
        addSegment(newTemperature);
    }

    private void addSegment(float newTemp)
    {
        if (secondCount > 0)
        {
            GameObject newSegment = Instantiate(lineGraphSegment);
            newSegment.transform.SetParent(transform.Find("VisCanvas").Find("LiveGraph"));

            newSegment.transform.localPosition = new Vector3((secondCount - 0.5f) / 40f * 4 - 2f, newPercentage + lastPercentage, 0);
            float deltaX = 0.1f;
            float deltaY = 2 * (newPercentage - lastPercentage);
            if (transform.name == "WyomingSensor") Debug.Log(transform.name + ": " + deltaY);
            newSegment.transform.localEulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * -Mathf.Atan(deltaX / deltaY));
            newSegment.transform.localScale = new Vector3(newSegment.transform.localScale.x, Mathf.Sqrt(Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2)) / 2, newSegment.transform.localScale.z);
        }
        lastTemp = newTemp;
        lastPercentage = newPercentage;
        secondCount++;
    }
}
