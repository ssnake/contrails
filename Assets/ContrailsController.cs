using UnityEngine;
using System.Collections;
using System;

public abstract class DistPoint
{
    public abstract void GetPoint(float latitude, float longitude, float bearing, float distance, out float newLat, out float newLong);
}

//http://www.movable-type.co.uk/scripts/latlong.html
public class DistPointConv : DistPoint
{
    public override void GetPoint(float latitude, float longitude, float bearing, float distance, out float newLat, out float newLong)
    {
        float R = 6378137.0f;
        latitude = (float)((latitude) * Math.PI / 180.0f);
        longitude = (float)((longitude ) * Math.PI / 180.0f);
        bearing = (float)(bearing * Math.PI / 180.0f);

        newLat = (float)(Math.Asin(Math.Sin(latitude) * Math.Cos(distance / R) + Math.Cos(latitude) * Math.Sin(distance / R) * Math.Cos(bearing)));
        newLong = (float)(longitude + Math.Atan2(Math.Sin(bearing) * Math.Sin(distance / R) * Math.Cos(latitude), Math.Cos(distance / R) - Math.Sin(latitude) * Math.Sin(newLat)));
        newLat = (float)(newLat * 180.0f / Math.PI);
        newLong = (float)(newLong * 180.0f / Math.PI);
    }
}

public class ContrailsController  {
    DistPoint distPoint;

    // Use this for initialization
    public ContrailsController() {
        distPoint = new DistPointConv();
    }
	
    public bool GetContrailsCoord(out float minDist, out float maxDist, out float lat, out float lng)
    {
        //float minLong, minLat, maxLong, maxLat;
        //Altitudes where contrails can form from jet exhaust range from 25,000-35,000 ft

        var b = GetDistance(out minDist, out maxDist);
        
        //distPoint.GetPoint(MainController.gpsController.GetLatitude(), MainController.gpsController.GetLongitude(), MainController.camControl.getY(), minDist, out minLat, out minLong);
        distPoint.GetPoint(MainController.gpsController.GetLatitude(), MainController.gpsController.GetLongitude(), MainController.camControl.getY(), (maxDist - minDist)/2.0f, out lat, out lng);
        return b;
        
    }
    public bool GetDistance(out float minDist, out float maxDist)
    {
        minDist = (float)(((25000 * 0.3048f) - MainController.gpsController.GetAlt()) / Math.Tan(-MainController.camControl.getX() * Math.PI / 180.0f));
        maxDist = (float)(((35000 * 0.3048f) - MainController.gpsController.GetAlt()) / Math.Tan(-MainController.camControl.getX() * Math.PI / 180.0f));

        return 30000.0f > maxDist && maxDist > 0.0f;
    }
}
