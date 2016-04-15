using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class AircraftController : MonoBehaviour {
    AirtcraftImporter importer;
    BuildingImporter buildingImporter;

    public GameObject aircraft;
    public GameObject route;
    List<AircraftImported> aircraftList;
    List<AircraftImported> buildingList;




    // Use this for initialization
    void Start () {
        MainController.aircraftController = this;
        importer = new AircrafImporterEmulate(10, 10 );
        buildingImporter = new BuildingImporter();
        aircraftList = new List<AircraftImported>();
        buildingList = new List<AircraftImported>();

        InvokeRepeating("Import", 0.0f, 1.0f);
        //Import();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public List<AircraftImported> AircraftList
    {
        get
        {
            return aircraftList;
        }
    }
    public List<AircraftImported> BuildingList
    {
        get
        {
            return buildingList;
        }
    }

    void Import()
    {

        aircraftList.Clear();
        buildingList.Clear();


        var deleteList2 = new List<GameObject>(GameObject.FindGameObjectsWithTag("route"));
        foreach (Aircraft a in importer.Import())
        {

            var imported = new AircraftImported(a);
            Apply2(a, imported);
            aircraftList.Add(imported);
            
            var routeObject = GameObject.Find("route_" + a.id);
            
            if (routeObject == null)
            {
                routeObject = Instantiate(route);
                routeObject.name = "route_" + a.id;
            } else
            {
                deleteList2.Remove(routeObject);
            }

       
            ApplyRoute(a, routeObject);

        }
        foreach(Aircraft a in buildingImporter.Import())
        {
            var imported = new AircraftImported(a);
            Apply2(a, imported);
            buildingList.Add(imported);
        }

        //clear remaining objects from scene
       
        foreach (GameObject go in deleteList2)
        {
            Destroy(go);
        }

    }

    void Apply2(Aircraft craft, AircraftImported imported)
    {
        var x = imported.position.x;
        var y = imported.position.y;
        var lat = craft.latitude;
        var lng = craft.longitude;
        var alt = craft.altitude;


        lat = lat - MainController.gpsController.GetLatitude();
        lng = lng - MainController.gpsController.GetLongitude();
        alt = alt - MainController.gpsController.GetAlt();

        MainController.mapController.LatLong2XY(lat, lng, alt, out x, out y, out alt);

        imported.position = new Vector3(x, alt, y);
    }
    void Apply(Aircraft craft, GameObject obj)
    {
 

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
