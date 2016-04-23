using UnityEngine;
using System.Collections;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleJSON;

public class FlightRadar24Importer : AirtcraftImporter {
	
	public override IEnumerable Import()
	{
		var result = GetIdFromJson ("{\"97f6906\":[\"8963E5\",60.9580,17.6808,134,35000,490,\"2540\",\"F-ESSD2\",\"A388\",\"A6-EOH\",1461402984,\"SFO\",\"DXB\",\"EK226\",0,64,\"UAE226\",0]}");
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

	private List<Aircraft> GetIdFromJson(string json)
	{
		JSONClass parse = (JSONClass)JSON.Parse (json);

		List<Aircraft> planes = new List<Aircraft> ();

		foreach (var t in parse) {
			var value = (KeyValuePair<string, JSONNode>)t;
			if (Regex.IsMatch (value.Key, @"^\d")) {
				JSONArray mydata = (JSONArray)value.Value;

				var lat = mydata [1].ToString ().Replace("\"", "");
				var lon = mydata [2].ToString ().Replace("\"", "");
				var alt = mydata [4].ToString ().Replace("\"", "");
				var pointId = value.Key.Replace("\"", " ");
				var flightId = mydata [13].ToString ().Replace("\"", "");

				var plane = new Aircraft (pointId, float.Parse(lon),float.Parse (lat), float.Parse (alt) );

				planes.Add (plane);
			}
		}

		return planes;
	}
}
