using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {
    
    public static UI ui;
    public static CameraControl camControl;
    public static MapController mapController;
    public static GpsController gpsController;


    // Use this for initialization
    void Awake () {
        MainController.mapController = new MapController();
        MainController.gpsController = new GpsController(true);


    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit(); }

    }
}
