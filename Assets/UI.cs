using System;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public bool showCameras = true;
    public bool showGyro = true;
    public bool showGps = true;
    public Text accel;
    public Text gps;
    public GameObject compassArrow;
	// Use this for initialization
	void Start () {

        MainController.ui = this;
        Input.location.Start();
    }
	
	// Update is called once per frame
	void Update () {

        accel.text = "";
        gps.text = "";
        if (showCameras) showCams();
        if (showGyro) showGyroInfo();
        if (showGps) showLocation();
        if (compassArrow != null) UpdateCompass();
    }

    private void UpdateCompass()
    {
        float fix = 90.0f;
        
        compassArrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, (MainController.camControl.getY() + fix));
    }

    void showCams()
    {
        accel.text += "Camera Count: " + WebCamTexture.devices.Length +"\n";

    }
    void showGyroInfo()
    {
        accel.text += "Input x: " + MainController.camControl.getX() + "\n";
        accel.text += "Input y: " + MainController.camControl.getY() + "\n";
        accel.text += "Input z: " + MainController.camControl.getZ() + "\n";


    }
    void showLocation()
    {
        gps.text += "Gps Status: " + MainController.gpsController.GetStatus() + "\n";
        gps.text += "Lat: " + MainController.gpsController.GetLatitude() + "\n";
        gps.text += "Long: " + MainController.gpsController.GetLongitude() + "\n";
        gps.text += "Alt: " + MainController.gpsController.GetAlt() + "\n";
        gps.text += "LastUpdate: " + MainController.gpsController.GetGPSDateTime() + "\n";
    }
}
