using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System.Globalization;
using System;
using UnityEngine.UI;
using System.Threading.Tasks;
#if NETFX_CORE
using Windows.Web.Http;
#endif

public class LoadCameraPosition : MonoBehaviour {
    
    public string hostname;
    //public Text textObject;
    float longitude, latitude;

	// Use this for initialization
	void Start () {

#if NETFX_CORE
        getLongLat();
        convertToScenePosition();
#else
        longitude = -105.2679f;
        latitude = 40.0062f;
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}

    private async void getLongLat()
    {
#if NETFX_CORE
        var result = "";
        var uri = new System.Uri(hostname + "/fieldview/getLongitude");
        Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
        using (var httpClient = new Windows.Web.Http.HttpClient())
        {
            try
            {
                httpResponse = await httpClient.GetAsync(uri);
                httpResponse.EnsureSuccessStatusCode();
                result = await httpResponse.Content.ReadAsStringAsync();
                longitude = float.Parse(result, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        uri = new System.Uri(hostname + "/fieldview/getLatitude");
        httpResponse = new Windows.Web.Http.HttpResponseMessage();
        using (var httpClient = new Windows.Web.Http.HttpClient())
        {
            try
            {
                httpResponse = await httpClient.GetAsync(uri);
                httpResponse.EnsureSuccessStatusCode();
                result = await httpResponse.Content.ReadAsStringAsync();
                latitude = float.Parse(result, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
#endif
    }

    // Convert Latitude/Longitude to Scene Coordinates
    private void convertToScenePosition()
    {
        // Assuming that Cell 0 begins at 40.00565,-105.26800, which maps to (0,0) in scene space
        // TODO: Add link to the map.
        float meter_to_degree = .000008983f;
        float worldspace_latitude_degree_offset = latitude - 40.00565f;
        float worldspace_longitude_degree_offset = longitude + 105.26800f;

        Debug.Log(worldspace_latitude_degree_offset);
        Debug.Log(worldspace_longitude_degree_offset);

        float pos_z = worldspace_latitude_degree_offset / meter_to_degree;
        float pos_x = worldspace_longitude_degree_offset / meter_to_degree;
        
        transform.localPosition = new Vector3(-pos_x, 0, -pos_z);
    }
}
