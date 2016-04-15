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

            return 50.907703f;
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
            return 34.821918f;
            //return 34.76892f;
        }
        else {
            return Input.location.lastData.longitude;
        }
    }
    public string GetStatus()
    {
        string started = "started";
        if (!Started()) started = "searching for gps";
        if (!Input.location.isEnabledByUser && !Application.isEditor) started = "not enabled by user";
        if (emulateGPS)
            return "Emulated ("+ started+")";
        else
        {
            return Input.location.status.ToString() + " (" + started + ")";
        }
    }

    public float GetAlt()
    {
        if (emulateGPS)
            return 134.0f;
        else
            return Input.location.lastData.altitude;
           
    }

    public System.DateTime GetGPSDateTime()
    {
        var ts = System.TimeSpan.FromSeconds(Input.location.lastData.timestamp);
        return (new System.DateTime(1970, 1, 1, 0, 0, 0, 0) + ts).ToLocalTime();
    }

    public bool Started()
    {
        var diff = System.DateTime.Now - GetGPSDateTime();
        if (emulateGPS)
            return true;
        else
            return (diff.TotalMinutes < 5);
    }
}
