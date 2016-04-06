
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class AirtcraftImporter
{
    protected List<Aircraft> list;
    public IEnumerable Import()
    {
        foreach (Aircraft a in list)
        {
            yield return a;
        }
    }

};
class AircrafImporterEmulate : AirtcraftImporter
{
    public AircrafImporterEmulate(float baseLong, float baseLat, int radiusKM=50, int amoutOfAircrafts = 10)
    {
        this.list = new List<Aircraft>();
        for(int i=0; i < amoutOfAircrafts; i++)
        {
            list.Add(generateAircraft(i, baseLong, baseLat, radiusKM));

        }
        
        list.Add(generateAircraft(amoutOfAircrafts + 1, MainController.gpsController.GetLongitude(), MainController.gpsController.GetLatitude(), 0));
        //house
        list.Add(new Aircraft(amoutOfAircrafts + 2, 34.822387f, 50.908291f, 20.0f));
        //bus stop 50.909039, 34.822518
        list.Add(new Aircraft(amoutOfAircrafts + 3, 34.822518f, 50.909039f, 3.0f));
        //shop 50.908872, 34.821022
        list.Add(new Aircraft(amoutOfAircrafts + 4, 34.821022f, 50.908872f, 3.0f));
        //tec 50.948070, 34.780939
        list.Add(new Aircraft(amoutOfAircrafts + 5, 34.780939f, 50.948070f, 50.0f));
        //house 50.915215, 34.828533
        list.Add(new Aircraft(amoutOfAircrafts + 6, 34.828533f, 50.915215f, 50.0f));
        //house 50.915371, 34.829048
        list.Add(new Aircraft(amoutOfAircrafts + 7, 34.829048f, 50.915371f, 50.0f));
        //house 50.910667, 34.822622
        list.Add(new Aircraft(amoutOfAircrafts + 8, 34.822622f, 50.910667f, 50.0f));




    }
    Aircraft generateAircraft(int id, float baseLong, float baseLat, int radiusKM)
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

};


internal class Aircraft
{
    public int id;
    public float longitude;
    public float latitude;
    public float altitude;
    public Aircraft(int id, float longitude, float latitude, float altitude)
    {
        this.id = id;
        this.longitude = longitude;
        this.latitude = latitude;
        this.altitude = altitude;
    }
   
}
