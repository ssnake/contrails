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
        
    }
    public override IEnumerator Import()
    {
        IsImporting = true;
        try
        {
            var tempList = list;// new List<Aircraft>();
            

            var data = GetData(GetUrl());
            yield return null;
            foreach (var plane in GetAircraftsFromJson2(data))
            {

                tempList.Add(plane);

            }

            foreach (var plane in tempList)
            {
                var responseForPlane = GetDataWaypoints(plane);
                yield return null;
               
                    foreach (var wp in UpdateWaypoint2(responseForPlane))
                    {

                        if (wp.altitude > 7000)
                            plane.route.Add(wp);
                        yield return null;
                    };
                

            }
            list = tempList;

            

        } finally
        {
            IsImported = true;
            IsImporting = false;
        }
	}

    private IEnumerable<Aircraft>  GetAircraftsFromJson2(string json)
    {
        var obj = new JSONObject(json);
       

        for(var i = 0; i< obj.Count; i++)
        {
            if (Regex.IsMatch(obj.keys[i], @"^\d"))
            {
                
                var lat = obj.list[i].list[1].f;
                var lng = obj.list[i].list[2].f;
                var alt = obj.list[i].list[4].f * 0.3048f;
                var plane = new Aircraft(obj.keys[i], lng, lat, alt);
                yield return plane;
            }
        }
       

    }
    

    private IEnumerable<Waypoint> UpdateWaypoint2(string json)
    {
        JSONObject obj;
        try
        {
            obj = new JSONObject(json);
        } catch
        {
            yield break;
        }

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
                    yield return wayPoint;

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
            MainController.config.visibleDistance,
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
