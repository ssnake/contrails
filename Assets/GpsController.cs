using UnityEngine;
using System.Collections;

public class GpsController
{
    bool emulateGPS;
    public GpsController()
    {
        emulateGPS = Application.isEditor;
       
        if (!emulateGPS) Input.location.Start();
    }
    //my place lat 50.907154, long 34.820437
    // home 50.937207, 34.768769
    public float GetLatitude()
    {
        if (emulateGPS)
        {
            return 50.90756f;
            //return 50.93724f;
        }
        else {
            return Input.location.lastData.latitude;
        }
    }
    public float GetLongitude()
    {
        if (emulateGPS)
        {
            return 34.82222f;
            //return 34.76892f;
        }
        else {
            return Input.location.lastData.longitude;
        }
    }
    public string GetStatus()
    {
        if (emulateGPS)
            return "Emulated";
        else
            return Input.location.status.ToString();
    }
    public float GetAlt()
    {
        if (emulateGPS)
            return 0.0f;
        else
            return Input.location.lastData.altitude;
           
    }
}
