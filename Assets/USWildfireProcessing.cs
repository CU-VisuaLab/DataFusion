using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Globalization;

public class WildfireDataPoint : MonoBehaviour
{
    public float longitude;
    public float latitude;
    public int year;
    public string fire_name;
    public string state;
    public string fire_size;
}
public class USWildfireProcessing : MonoBehaviour {
    public CSVReader CSV;
    public Material[] materials;
    public GameObject dataPoint;
    private float min_latitude = 25f;
    private float max_latitude = 50f;
    private float min_longitude = -125f;
    private float max_longitude = -65f;
    private List<string[]> rowData;
    private string[] headers;
    private List<WildfireDataPoint> datapoints;

    // Use this for initialization
    void Start() {
        CSV.ParseFirst1000();
        headers = CSV.GetColumnHeads();
        rowData = CSV.GetRowData();
        datapoints = new List<WildfireDataPoint>();
        loadVisualization();
        displayVisualization();
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    void loadVisualization()
    {
        for (var i = 1; i < rowData.Count; i++)
        {
            WildfireDataPoint point = new WildfireDataPoint();
            for (var j = 0; j < rowData[i].Length; j++)
            {
                try
                {
                    if (headers[j] == "FIRE_NAME" && rowData[i][j] != "") point.fire_name = rowData[i][j];
                    else if (headers[j] == "LONGITUDE" && rowData[i][j] != "") point.longitude = float.Parse(rowData[i][j], CultureInfo.InvariantCulture.NumberFormat);
                    else if (headers[j] == "LATITUDE" && rowData[i][j] != "") point.latitude = float.Parse(rowData[i][j], CultureInfo.InvariantCulture.NumberFormat);
                    else if (headers[j] == "FIRE_YEAR" && rowData[i][j] != "") point.year = int.Parse(rowData[i][j], CultureInfo.InvariantCulture.NumberFormat);
                    else if (headers[j] == "STATE" && rowData[i][j] != "") point.state = rowData[i][j];
                    else if (headers[j] == "FIRE_SIZE_CLASS" && rowData[i][j] != "") point.fire_size = rowData[i][j];
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            datapoints.Add(point);
        }
    }

    void displayVisualization()
    {
        float latitude_range = 111.2401f;
        float longitude_range = 267.169f;
        for (var i = 0; i < datapoints.Count; i++)
        {
            try
            {
                GameObject newPoint = Instantiate(dataPoint);
                newPoint.transform.localPosition = new Vector3((datapoints[i].longitude - min_longitude) * longitude_range / (max_longitude - min_longitude), 5f,
                    (datapoints[i].latitude - min_latitude) * latitude_range / (max_latitude - min_latitude));
                newPoint.transform.Find("Image").Find("Text").GetComponent<Text>().text = datapoints[i].fire_name + ": " + datapoints[i].state;

                int factor;
                if (datapoints[i].fire_size == "A") factor = 2;
                else if (datapoints[i].fire_size == "B") factor = 4;
                else if (datapoints[i].fire_size == "C") factor = 6;
                else if (datapoints[i].fire_size == "D") factor = 8;
                else if (datapoints[i].fire_size == "E") factor = 10;
                else if (datapoints[i].fire_size == "F") factor = 12;
                else factor = 14;
                newPoint.transform.localScale = new Vector3(factor, factor, factor);
                newPoint.transform.Find("Image").localScale = new Vector3(5f / factor, 5f / factor, 5f / factor);
                //newPoint.transform.Find("Image").gameObject.SetActive(false);
                int material_index = Mathf.Min((datapoints[i].year - 1992) / 3, 6);
                newPoint.GetComponent<Renderer>().material = materials[material_index];
            }
            catch(Exception e)
            {
                continue;
            }
        }
    }
}
