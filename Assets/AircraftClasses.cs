
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
    AircrafImporterEmulate(float baseLong, float baseLat, int radiusKM=50, int amoutOfAircrafts = 10)
    {
        this.list = new List<Aircraft>();
        for(int i=0; i < amoutOfAircrafts; i++)
        {
            list.Add(generateAircraft(i, baseLong, baseLat, radiusKM));

        }
        //my place lat 50.907154, long 34.820437
        list.Add(generateAircraft(11, 34.820437f, 50.907154f, 0));
    }
    Aircraft generateAircraft(int id, float baseLong, float baseLat, int radiusKM)
    {
        Aircraft craft = new Aircraft(id, baseLong, baseLat, Random.Range(1000, 12000));
        int radius = Random.Range(1000, radiusKM * 1000);//random radius in meters
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
    public void Apply(GameObject go)
    {
        go.transform.Translate(new Vector3(longitude, altitude, latitude));
        go.name = "aircraft_" + id;
    }
}
