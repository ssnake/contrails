using UnityEngine;
using System.Collections;

public class GpsController
{
    bool emulateGPS;
    public GpsController()
    {
        emulateGPS = true;// Application.isEditor;
       
        if (!emulateGPS) Input.location.Start();
    }
    //my place lat 50.907154, long 34.820437
    // home 50.937207, 34.768769
    public float GetLatitude()
    {
        if (emulateGPS)
        {
            //return 50.907703f;
            return 50.937207f;
        }
        else {
            return Input.location.lastData.latitude;
        }
    }
    public float GetLongitude()
    {
        if (emulateGPS)
        {
            //return 34.821918f;
            return 34.768769f;
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
