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
		var result = GetAircraftsFromJson ("{\"97f6906\":[\"8963E5\",60.9580,17.6808,134,35000,490,\"2540\",\"F-ESSD2\",\"A388\",\"A6-EOH\",1461402984,\"SFO\",\"DXB\",\"EK226\",0,64,\"UAE226\",0]}");
		return null;
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
}
