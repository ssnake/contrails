using UnityEngine;
using System.Collections;

public class GpsController
{
    bool emulateGPS;
    float emulatedLat;
    float emulatedLong;

    public GpsController()
    {
        emulateGPS = true || Application.isEditor;
        //50.466211, 30.508632
        //51.468679, -0.456624
        emulatedLat = 51.468679f;
		emulatedLong = -0.456624f;
//        emulatedLat = 40.631096f;
//        emulatedLong = -73.778282f;
        if (!emulateGPS) Input.location.Start();
    }
    //my place lat 50.907154, long 34.820437
    // home 50.937207, 34.768769
    //kiev 50.471110, 30.500214
    // brussel 50.844749, 4.370956
    // newyork 40.691154, -74.171536
    public float GetLatitude()
    {
        if (emulateGPS)
        {
            return emulatedLat;
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
            return emulatedLong;
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
            return 0.0f;
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
