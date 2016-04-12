
using UnityEngine;
using System.Collections;
using System.Collections.Generic;





    }
    void Populate()
    {
        list.Clear();
        for (int i = 0; i < amount; i++)
        {
            list.Add(GenerateRoute(GenerateAircraft(i, baseLong, baseLat, radiusKM), 1000, 30000));

        }

        list.Add(new Aircraft(amount + 1, MainController.gpsController.GetLongitude(), MainController.gpsController.GetLatitude(), MainController.gpsController.GetAlt() + 100));
        //house 50.908213, 34.822319
        list.Add(new Aircraft(amount + 2, 34.822319f, 50.908213f, MainController.gpsController.GetAlt() + 50.0f));
        //bus stop 50.909039, 34.822518
        list.Add(new Aircraft(amount + 3, 34.822518f, 50.909039f, MainController.gpsController.GetAlt() + 3.0f));
        //shop 50.908872, 34.821022
        list.Add(new Aircraft(amount + 4, 34.821022f, 50.908872f, MainController.gpsController.GetAlt() + 3.0f));
        //tec 50.948070, 34.780939
        list.Add(new Aircraft(amount + 5, 34.780939f, 50.948070f, MainController.gpsController.GetAlt() + 50.0f));
        //house 50.915215, 34.828533
        list.Add(new Aircraft(amount + 6, 34.828533f, 50.915215f, MainController.gpsController.GetAlt() + 50.0f));
        //house 50.915371, 34.829048
        list.Add(new Aircraft(amount + 7, 34.829048f, 50.915371f, MainController.gpsController.GetAlt() + 50.0f));

        //house 50.910667, 34.822622
        list.Add(new Aircraft(amount + 8, 34.822622f, 50.910667f, MainController.gpsController.GetAlt() + 50.0f));
        //monument 50.895055, 34.789054
        list.Add(new Aircraft(amount + 9, 34.789054f, 50.895055f, MainController.gpsController.GetAlt() + 50.0f));
        //house 50.890367, 34.799896
        list.Add(new Aircraft(amount + 10, 34.799896f, 50.890367f, MainController.gpsController.GetAlt() + 50.0f));
        //cathedral 50.909251, 34.800637
        list.Add(new Aircraft(amount + 11, 34.800637f, 50.909251f, MainController.gpsController.GetAlt() + 50.0f));
        //bankovsk 50.893771, 34.802704
        list.Add(new Aircraft(amount + 11, 34.802704f, 50.893771f, MainController.gpsController.GetAlt() + 50.0f));
        //50.937464, 34.771593
        list.Add(new Aircraft(amount + 12, 34.771593f, 50.937464f, MainController.gpsController.GetAlt() + 50.0f));
        //50.939114, 34.769537
        list.Add(new Aircraft(amount + 13, 34.769537f, 50.939114f, MainController.gpsController.GetAlt() + 50.0f));


    }
    Aircraft GenerateAircraft(int id, float baseLong, float baseLat, int radiusKM)
    {
        Aircraft craft = new Aircraft(id, baseLong, baseLat, Random.Range(1000, 12000));
        int radius = Random.Range(0, radiusKM * 1000);//random radius in meters
        double angle = (double) Random.Range(0, (3.14f) * 2);
        //rough approximation http://gis.stackexchange.com/questions/2951/algorithm-for-offsetting-a-latitude-longitude-by-some-amount-of-meters
        var dn = radius * (float)System.Math.Sin(angle);
        var de = radius * (float)System.Math.Cos(angle);
        //Earth’s radius, sphere
        var R = 6378137f;
        var dLat = dn / R;
        var dLon = de / (R * System.Math.Cos(System.Math.PI * baseLat / 180));


        craft.latitude += (float)(dLat * 180 / System.Math.PI);
        craft.longitude += (float)(dLon * 180 / System.Math.PI);
        return craft;
    }
    Aircraft GenerateRoute(Aircraft craft, float stepM, float lengthM)
    {
        var mainDirection = Random.rotation;
        int count = (int)(lengthM / stepM);
        craft.route.Add(new Waypoint(craft.longitude, craft.latitude, craft.altitude));
        for (var i = 1; i < count; i++)
        {


        }
        return craft;
    }
    public override IEnumerable Import()
    {
        Populate();
        return base.Import();
    }
};


internal class Aircraft
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
internal class Waypoint
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