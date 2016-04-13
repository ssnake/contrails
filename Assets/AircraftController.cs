using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class AircraftController : MonoBehaviour {
    AirtcraftImporter importer;
    public GameObject aircraft;
    public GameObject route;
    SphereMap sphere;
   

    // Use this for initialization
    void Start () {
        importer = new AircrafImporterEmulate(MainController.gpsController.GetLongitude(), MainController.gpsController.GetLatitude(), 50, 0 );
        sphere = new SphereMap(100.0f);
        InvokeRepeating("Import", 0.0f, 1.0f);
        //Import();
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    void Import()
    {
        var deleteList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        var deleteList2 = new List<GameObject>(GameObject.FindGameObjectsWithTag("route"));
        foreach (Aircraft a in importer.Import())
        {
            var airObject = GameObject.Find("aircraft_" + a.id);
            //if new aircraft
            if (airObject == null)
            {
                airObject = Instantiate(aircraft);
                airObject.name = "aircraft_" + a.id;


            }
            else
            {
                deleteList.Remove(airObject);
            }

            var routeObject = GameObject.Find("route_" + a.id);
            if (routeObject == null)
            {
                routeObject = Instantiate(route);
                routeObject.name = "route_" + a.id;
            } else
            {
                deleteList2.Remove(routeObject);
            }

            Apply(a, airObject);
            ApplyRoute(a, routeObject);

        }
        //clear remaining objects from scene
        foreach (GameObject go in deleteList)
        {
            Destroy(go);
        }
        foreach (GameObject go in deleteList2)
        {
            Destroy(go);
        }

    }
    void Apply(Aircraft craft, GameObject obj)
    {
        var x = obj.transform.position.x;
        var y = obj.transform.position.y;
        var lat = craft.latitude;
        var lng = craft.longitude;
        var alt = craft.altitude;
        float myX;
        float myY;
        float myAlt;
        
        lat = lat - MainController.gpsController.GetLatitude();
        lng = lng - MainController.gpsController.GetLongitude();
        alt = alt - MainController.gpsController.GetAlt();
        
        MainController.mapController.LatLong2XY(lat, lng, alt, out x, out y, out alt);
        /*
        MainController.mapController.LatLong2XY(MainController.gpsController.GetLatitude(), MainController.gpsController.GetLongitude(), MainController.gpsController.GetAlt(), out myX, out myY, out myAlt);
    
        x = x - myX;
        y = y - myY;
        alt = alt - myAlt;
        */
        //sphere.Adjust(ref x, ref y, ref alt);

        obj.transform.position = new Vector3(x, alt, y);
        

        //var dist = Vector3.Distance(obj.transform.position, new Vector3(0, 0, 0));
        //var scale = System.Math.Max(1.0f, dist/100.0f );
        //obj.transform.localScale = new Vector3(scale, scale, scale); 

    }
    void ApplyRoute(Aircraft craft, GameObject obj)
    {
        var lr = obj.GetComponent<LineRenderer>();
        List<Vector3> list = new List<Vector3>();
        for(var i = 0; i < craft.route.Count; i ++)
        {
            var lat = craft.route[i].latitude - MainController.gpsController.GetLatitude();
            var lng = craft.route[i].longitude - MainController.gpsController.GetLongitude();
            var alt = craft.route[i].altitude - MainController.gpsController.GetAlt();
            float x, y;

            MainController.mapController.LatLong2XY(lat, lng, alt, out x, out y, out alt);
            list.Add(new Vector3(x, alt, y));
        }
        lr.SetVertexCount(list.Count);
        lr.SetPositions(list.ToArray());

    }
}
