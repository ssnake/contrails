using UnityEngine;
using System.Collections;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleJSON;

public class FlightRadar24Importer : AirtcraftImporter {
    DistPoint distPoint;

	public override IEnumerable Import()
    {//50.907154, long 34.820437
        var result = GetAircraftsFromJson ("{\"97f6906\":[\"8963E5\",50.907154, 34.820437,134,1000,490,\"2540\",\"F-ESSD2\",\"A388\",\"A6-EOH\",1461402984,\"SFO\",\"DXB\",\"EK226\",0,64,\"UAE226\",0]}");
		return result;
	}

	/// <summary>
	/// Gets the aircrafts from json.
	/// </summary>
	/// <returns>The aircrafts from json.</returns>
	/// <param name="json">Json as web response from flight radar</param>
	private List<Aircraft> GetAircraftsFromJson(string json)
	{
		JSONClass response = (JSONClass)JSON.Parse (json);

		var planes = new List<Aircraft> ();

		foreach (var responseEntry in response) {
			var entry = (KeyValuePair<string, JSONNode>)responseEntry;
			if (Regex.IsMatch (entry.Key, @"^\d")) {
				JSONArray mydata = (JSONArray)entry.Value;

				var lat = float.Parse(mydata [1].ToString ().Replace("\"", ""));
				var lon = float.Parse(mydata [2].ToString ().Replace("\"", ""));
				var alt = float.Parse(mydata [4].ToString ().Replace("\"", ""));
				var pointId = entry.Key.Replace("\"", " ");

				//international flight identificator
				//var flightId = mydata [13].ToString ().Replace("\"", "");

				var plane = new Aircraft (pointId, lon, lat, alt);

				planes.Add (plane);
			}
		}

		return planes;
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
