using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class AirtcraftImporter
{
    protected List<Aircraft> list;
    public AirtcraftImporter()
    {
        this.list = new List<Aircraft>();
    }
    public virtual IEnumerable Import()
    {
        foreach (Aircraft a in list)
        {
            yield return a;
        }
    }
    public List<Aircraft> AircraftList
    {
        get
        {
            return list;
        }
    }

};
class BuildingImporter : AirtcraftImporter
{
    public BuildingImporter()
    {
        var amount = 1000;
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


  
        //50.912719, 34.825652
        //list.Add(new Aircraft(amount + 12, 34.825652f, 50.912719f, MainController.gpsController.GetAlt() + 50.0f));
        //50.911427, 34.817165
        //  list.Add(new Aircraft(amount + 13, 34.817165f, 50.911427f, MainController.gpsController.GetAlt() + 50.0f));
        //50.904382, 34.815856
        //  list.Add(new Aircraft(amount + 14, 34.815856f, 50.904382f, MainController.gpsController.GetAlt() + 50.0f));
        //50.904753, 34.830686
        //list.Add(new Aircraft(amount + 15, 34.830686f, 50.904753f, MainController.gpsController.GetAlt() + 50.0f));

    }

}
class AircrafImporterEmulate : AirtcraftImporter
{
    int amount;
    float baseLat;
    float baseLong;
    int radiusKM;

    public AircrafImporterEmulate(float baseLong, float baseLat, int radiusKM = 50, int amoutOfAircrafts = 10)
    {
        
        amount = amoutOfAircrafts;
        this.baseLat = baseLat;
        this.baseLong = baseLong;
        this.radiusKM = radiusKM;
        Populate();


    }
    void Populate()
    {
        list.Clear();
        for (int i = 0; i < amount; i++)
        {
            list.Add(GenerateRoute(GenerateAircraft(i, baseLong, baseLat, radiusKM), 100, 30000));

        }
        
        list.Add(GenerateRoute(new Aircraft(amount + 1, MainController.gpsController.GetLongitude(), MainController.gpsController.GetLatitude(), MainController.gpsController.GetAlt() + 100), 100, 1000));
       

    }
    Aircraft GenerateAircraft(int id, float baseLong, float baseLat, int radiusKM)
    {
        Aircraft craft = new Aircraft(id, baseLong, baseLat, Random.Range(1000, 12000));
        int radius = Random.Range(500, radiusKM * 1000);//random radius in meters
        float angle = (float) Random.Range(0, (3.14f) * 2);
        Offset(baseLong, baseLat, angle, radius, out craft.longitude, out craft.latitude);
        
        return craft;
    }

    void Offset(float baseLong, float baseLat, float angle, float stepM, out float newLong, out float newLat)
    {
        //rough approximation http://gis.stackexchange.com/questions/2951/algorithm-for-offsetting-a-latitude-longitude-by-some-amount-of-meters
        var dn = stepM * (float)System.Math.Sin(angle);
        var de = stepM * (float)System.Math.Cos(angle);
        //Earth’s radius, sphere
        var R = 6378137f;
        var dLat = dn / R;
        var dLon = de / (R * System.Math.Cos(System.Math.PI * baseLat / 180));

        newLat = baseLat + (float)(dLat * 180 / System.Math.PI);
        newLong = baseLong + (float)(dLon * 180 / System.Math.PI);
    }

    Aircraft GenerateRoute(Aircraft craft, float stepM, float lengthM)
    {
        var baseAngle = (float)Random.Range(0, (float) System.Math.PI * 2.0f);
        int count = (int)(lengthM / stepM);
        craft.route.Add(new Waypoint(craft.longitude, craft.latitude, craft.altitude));
        for (var i = 1; i < count; i++)
        {
            var prev = craft.route[i - 1];
            float lang, lng;
            Offset(prev.longitude, prev.latitude, baseAngle + Random.Range(-15.0f / 180.0f * (float) System.Math.PI, 15.0f / 180.0f * (float) System.Math.PI), stepM, out lng, out lang);
            craft.route.Add(
                new Waypoint(
                    new Vector3(
                        lng,
                        craft.altitude, 
                        lang)
                    )
                );

        }
        return craft;
    }
    public override IEnumerable Import()
    {
        //Populate();
        return base.Import();
    }
};

