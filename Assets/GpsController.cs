using UnityEngine;
using System.Collections;

public class GpsController
{
    bool emulateGPS;
    public GpsController(bool emulate)
    {
        emulateGPS = emulate;
        if (!emulate) Input.location.Start();
    }
    //my place lat 50.907154, long 34.820437
    public float GetLatitude()
    {
        if (emulateGPS)
        {
            return 50.907154f;
        }
        else {
            return Input.location.lastData.latitude;
        }
    }
    public float GetLongitude()
    {
        if (emulateGPS)
        {
            return 34.820437f;
        }
        else {
            return Input.location.lastData.latitude;
        }
    }
    public string GetStatus()
    {
        if (emulateGPS)
            return "Emulated";
        else
            return Input.location.status.ToString();
    }
}
