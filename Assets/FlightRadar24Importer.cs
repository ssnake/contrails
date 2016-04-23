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

	private IEnumerable<string> GetIdFromJson(string json)
	{
		JSONArray arr = JSON.Parse (json) as JSONArray;
		yield return arr [0].ToString ();
	}
}
