using UnityEngine;
using System.Collections;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleJSON;

public class FlightRadar24Importer : AirtcraftImporter
{
    DistPoint distPoint;

    public override IEnumerable Import()
    {
        float lat1, lat2, lng1, lng2;
        GetRectangle(MainController.gpsController.GetLatitude(), MainController.gpsController.GetLongitude(), 10000, out lat1, out lng1, out lat2, out lng2);
        var result = GetIdFromJson("{\"97f6906\":[\"8963E5\",60.9580,17.6808,134,35000,490,\"2540\",\"F-ESSD2\",\"A388\",\"A6-EOH\",1461402984,\"SFO\",\"DXB\",\"EK226\",0,64,\"UAE226\",0]}");
        return null;
    }

    //	private IEnumerable<string> GetIdFromJson(string json)
    //	{
    //		JObject flightRadardata = JObject.Parse (json);
    //
    //		var keys = flightRadardata.Properties ().Where (x => Regex.IsMatch(x.Name, @"^\d"))
    //			.Select(x => x.Name)
    //			.ToList ();
    //
    //		foreach (var key in keys)
    //			yield return key;
    //	}

    private IEnumerable<string> GetIdFromJson(string json)
    {
        JSONArray arr = JSON.Parse(json) as JSONArray;
        yield return arr[0].ToString();
    }
    bool GetData(string url, out string data)
    {
        var net = new NetworkController();
        data = net.SendRequest("https://data-live.flightradar24.com/zones/fcgi/feed.js?bounds=51.19,50.11,28.35,33.12&faa=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=7200&gliders=1&stats=1");
        return true;


    }
    void GetRectangle(float lat, float lng, float dist, out float lat1, out float lng1, out float lat2, out float lng2)
    {
        distPoint = new DistPointConv();
        distPoint.GetPoint(lat, lng, 360.0f - 45.0f, dist, out lat1, out lng1);
        distPoint.GetPoint(lat, lng, 90.0f + 45.0f, dist, out lat2, out lng2);


    }

}
