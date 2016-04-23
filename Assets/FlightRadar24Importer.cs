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
        //var result = GetAircraftsFromJson ("{\"97f6906\":[\"8963E5\",50.907154, 34.820437,134,1000,490,\"2540\",\"F-ESSD2\",\"A388\",\"A6-EOH\",1461402984,\"SFO\",\"DXB\",\"EK226\",0,64,\"UAE226\",0]}");
        var data = "";
        GetData(GetUrl(), out data);
        var result = GetAircraftsFromJson(data);
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
				var alt = float.Parse(mydata [4].ToString ().Replace("\"", "")) * 0.3048f;
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
        data = net.SendRequest(url);
        return true;


    }
    void GetRectangle(float lat, float lng, float dist, out float lat1, out float lng1, out float lat2, out float lng2)
    {
        distPoint = new DistPointConv();
        distPoint.GetPoint(lat, lng, 360.0f - 45.0f, dist, out lat1, out lng1);
        distPoint.GetPoint(lat, lng, 90.0f + 45.0f, dist, out lat2, out lng2);


    }
    string GetUrl()
    {
        
        float lat1, lng1, lat2, lng2;
        GetRectangle(
            MainController.gpsController.GetLatitude(),
            MainController.gpsController.GetLongitude(),
            30000.0f,
            out lat1,
            out lng1,
            out lat2,
            out lng2

            );
        lat1 = (float) System.Math.Round(lat1, 2);
        lat2 = (float)System.Math.Round(lat2, 2);
        lng1 = (float)System.Math.Round(lng1, 2);
        lng2 = (float)System.Math.Round(lng2, 2);

        //https://data-live.flightradar24.com/zones/fcgi/feed.js?bounds=51.19,50.11,28.35,33.12&faa=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=7200&gliders=1&stats=1
        var bounds = "" + lat1 + "," + lat2 + ","+ lng1 + ","+lng2;
        
        return "https://data-live.flightradar24.com/zones/fcgi/feed.js?bounds=" + bounds + "&faa=1&mlat=1&flarm=1&adsb=1&gnd=1&air=1&vehicles=1&estimated=1&maxage=7200&gliders=1&stats=1";

    }
}
