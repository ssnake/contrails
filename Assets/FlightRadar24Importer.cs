using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Text.RegularExpressions;

public class FlightRadar24Importer : AirtcraftImporter {
    DistPoint distPoint;
    NetworkController net;

    public FlightRadar24Importer()
    {
        net = new NetworkController();
        
        var data = GetData(GetUrl());
        list = GetAircraftsFromJson2(data);
        CalculateWaypoints(list);
    }
    public override IEnumerable Import()
    {//50.907154, long 34.820437
        //var result = GetAircraftsFromJson ("{\"97f6906\":[\"8963E5\",50.907154, 34.820437,134,1000,490,\"2540\",\"F-ESSD2\",\"A388\",\"A6-EOH\",1461402984,\"SFO\",\"DXB\",\"EK226\",0,64,\"UAE226\",0]}");
        
        
        return list;
	}

    private List<Aircraft> GetAircraftsFromJson2(string json)
    {
        var obj = new JSONObject(json);
        var list = new List<Aircraft>();

        for(var i = 0; i< obj.Count; i++)
        {
            if (Regex.IsMatch(obj.keys[i], @"^\d"))
            {
                
                var lat = obj.list[i].list[1].f;
                var lng = obj.list[i].list[2].f;
                var alt = obj.list[i].list[4].f * 0.3048f;
                var plane = new Aircraft(obj.keys[i], lng, lat, alt);
                list.Add(plane);
            }
        }
        return list;

    }
    
    private void CalculateWaypoints(List<Aircraft> planes)
    {
        foreach (var plane in planes)
        {
            var responseForPlane = GetDataWaypoints(plane);
            
            
            UpdateWaypoint2(plane, responseForPlane);
            
        }
    }
    private void UpdateWaypoint2(Aircraft plane, string json)
    {
        var obj = new JSONObject(json);
        var i = obj.keys.IndexOf("trail");
        if ( i >= 0)
        {
            var trailsObj = obj.list[i];
            foreach(var elem in trailsObj.list)
            {
                Waypoint wayPoint = new Waypoint(0, 0, 0);
                elem.GetField(out wayPoint.altitude, "alt", 0.0f);
                elem.GetField(out wayPoint.latitude, "lat", 0.0f);
                elem.GetField(out wayPoint.longitude, "lng", 0.0f);
                if (wayPoint.ToVector().magnitude != 0.0f)
                    plane.route.Add(wayPoint);

            }
            
            
        }
        

        
    }
    
    string GetData(string url)
    {
        
        return net.SendRequest(url);
      


    }
    string GetDataWaypoints(Aircraft craft)
    {
        var url = "https://data-live.flightradar24.com/clickhandler/?version=1.5&flight="+craft.id+"&altitude=0&equip_hint=E190";
        return net.SendRequest(url);
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
