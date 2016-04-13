using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class Aircraft
{
    public int id;
    public float longitude;
    public float latitude;
    public float altitude;
    public List<Waypoint> route;
    public Aircraft(int id, float longitude, float latitude, float altitude)
    {
        this.id = id;
        this.longitude = longitude;
        this.latitude = latitude;
        this.altitude = altitude;
        this.route = new List<Waypoint>();
    }
   
}
public class Waypoint
{
    public float longitude;
    public float latitude;
    public float altitude;

    public Waypoint(float longitude, float latitude, float altitude)
    {
        this.longitude = longitude;
        this.latitude = latitude;
        this.altitude = altitude;
    }
    public Waypoint(Vector3 vector)
    {
        FromVector(vector);

    }
    public Vector3 ToVector()
    {
        return new Vector3(longitude, altitude, latitude);
    }
    public void FromVector(Vector3 vector)
    {
        this.longitude = vector.x;
        this.latitude = vector.z;
        this.altitude = vector.y;
    }
}

public class AircraftImported
{
    public Vector3 position;
    public Aircraft origin;
    public AircraftImported(Aircraft craft)
    {
        origin = craft;
        position = new Vector3();
        
    }
}