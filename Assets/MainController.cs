using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {
    
    public static UI ui;
    public static CameraControl camControl;
    public static MapController mapController;
    public static GpsController gpsController;
    public static AircraftController aircraftController;
    public static ContrailsController contrailsController;
    public static ConfigData config;

    // Use this for initialization
    void Awake () {
        MainController.mapController = new MapController();
        MainController.gpsController = new GpsController();
        MainController.contrailsController = new ContrailsController();


    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

    }
}
