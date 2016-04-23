using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Aircraft
{
    public string id;
    public float longitude;
    public float latitude;
    public float altitude;
    public List<Waypoint> route;
    public Aircraft(string id, float longitude, float latitude, float altitude)
    {
        this.id = id;
        this.longitude = longitude;
        this.latitude = latitude;
        this.altitude = altitude;
        this.route = new List<Waypoint>();
    }
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("id: " + id);
        sb.Append("longitude: " + longitude);
        sb.Append("latitude: " + latitude);
        sb.Append("altitude: " + altitude);

        return sb.ToString();
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
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append(origin.ToString());
        return sb.ToString();
    }
}