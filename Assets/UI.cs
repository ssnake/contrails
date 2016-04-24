using System;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public bool showCameras = true;
    public bool showGyro = true;
    public bool showGps = true;
    public Text accel;
    public Text gps;
    public Text contrails;
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
        contrails.text = "";

        if (showCameras) showCams();
        if (showGyro) showGyroInfo();
        if (showGps) showLocation();
        showContrailsCoord();
        if (compassArrow != null) UpdateCompass();
    }

    private void UpdateCompass()
    {
        float fix = 90.0f;
        
        compassArrow.transform.rotation = Quaternion.Euler(0.0f, 0.0f, ( - MainController.camControl.getY() + fix));
    }

    void showCams()
    {
        accel.text += "Camera Count: " + WebCamTexture.devices.Length +"\n";

    }

    void showContrailsCoord()
    {
        if (contrails != null)
        {
            float minDist, maxDist, lat, lng;
            contrails.text += "Contrails: \n";
            if (MainController.contrailsController.GetContrailsCoord(out minDist, out maxDist, out lat, out lng))
            {
                
                contrails.text += "Apx Long: " + lng + "\n";
                contrails.text += "Apx Lat: " + lat + "\n";
                contrails.text += "Min Dist: " + minDist + "\n";
                contrails.text += "Max Dist: " + maxDist + "\n";
            } else
            {
                contrails.text += "Apx Long: --\n";
                contrails.text += "Apx Lat: --\n";
                contrails.text += "Min Dist: --\n";
                contrails.text += "Max Dist: --\n";
            }
        }
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
