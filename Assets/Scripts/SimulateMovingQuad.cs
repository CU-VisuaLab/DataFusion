using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulateMovingQuad : MonoBehaviour {
    private System.Random rnd;
    
    public float linear_velocity = 0.5f; // velocity in meters per second
    private Vector3 velocity;

    public Vector3[] waypointLocations;
    public GameObject waypointPrefab;
    private GameObject[] waypoints;
    private int waypointsHit;

    public Material hitMaterial;
    public Material unhitMaterial;

    public GameObject pathCylinder;
    private GameObject waypointPath;

    public Sprite[] collectedImages;

    private bool finishedNavigation;
    
	// Use this for initialization
	void Start () {
        rnd = new System.Random();

        // Add unhit waypoints to the scene
        waypoints = new GameObject[waypointLocations.Length];
        for (var i = 0; i < waypointLocations.Length; i++)
        {
            waypoints[i] = Instantiate(waypointPrefab);
            foreach (Transform child in waypoints[i].transform)
            {
                if (child.name != "Image") child.gameObject.GetComponent<Renderer>().material = unhitMaterial;
            }
            waypoints[i].transform.position = new Vector3(waypointLocations[i].x, transform.position.y, waypointLocations[i].z);
        }

        // Initialize position and path of drone
        transform.position = new Vector3(waypointLocations[0].x, transform.position.y, waypointLocations[0].z);
        waypointsHit = 0;
        planPath();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Find("VelocityCanvas").transform.localEulerAngles = new Vector3(0, -transform.localEulerAngles.y, 0);
        if (finishedNavigation)
        {
            if (transform.position.y > 0.02f)
            {
                velocity = new Vector3(0.0f, -linear_velocity / 3, 0.0f);
                transform.position += velocity * Time.fixedDeltaTime;
            }
            return;
        }
        // Random variance in Y direction
        float variance = (rnd.Next(300) - 150) / 10000f;
        velocity.y = variance;
        transform.position += velocity * Time.fixedDeltaTime;
        if (Vector3.Distance(transform.position, waypointLocations[waypointsHit + 1]) - (Mathf.Abs(transform.position.y - waypointLocations[waypointsHit + 1].y)) < 0.02f)
        {
            waypointsHit++;
            planPath();
        }
	}

    // Set up the path to the next waypoint
    private void planPath() {
        GameObject hitWaypoint = waypoints[waypointsHit];
        Destroy(waypointPath);
        
        hitWaypoint.transform.Find("Image").GetComponent<Image>().sprite = collectedImages[waypointsHit];
        foreach (Transform child in hitWaypoint.transform)
        {
            if (child.name != "Image") child.gameObject.GetComponent<Renderer>().material = hitMaterial;
        }
        if (waypointsHit + 1 >= waypointLocations.Length)
        {
            finishedNavigation = true;
            return;
        }
        Vector3 start = transform.position;
        Vector3 finish = waypointLocations[waypointsHit + 1];

        float theta = Mathf.Atan((finish.z - start.z) / (finish.x - start.x));
        waypointPath = Instantiate(pathCylinder);
        waypointPath.transform.position = new Vector3((start.x + finish.x) / 2, transform.position.y, (start.z + finish.z) / 2);
        waypointPath.transform.localScale = new Vector3(0.01f, Mathf.Sqrt(Mathf.Pow(finish.z - start.z, 2) + Mathf.Pow(finish.x - start.x, 2)) / 2, 0.01f);
        waypointPath.transform.localEulerAngles = new Vector3(0, -Mathf.Rad2Deg * theta, 90f);
        
        velocity = new Vector3(linear_velocity * Mathf.Cos(theta), 0, linear_velocity * Mathf.Sin(theta));
        if (finish.x < start.x && theta == 0) velocity.x *= -1; // Seems that ACos can't differentiate between 0 degrees and 180
        transform.localEulerAngles = new Vector3(0, Mathf.Rad2Deg * theta + 45, 0);
    }
}
