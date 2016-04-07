﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class AircraftController : MonoBehaviour {
    AirtcraftImporter importer;
    public GameObject aircraft;
    SphereMap sphere;
    // Use this for initialization
    void Start () {
        importer = new AircrafImporterEmulate(MainController.gpsController.GetLongitude(), MainController.gpsController.GetLatitude(), 50, 0 );
        sphere = new SphereMap(1000.0f);
        InvokeRepeating("Import", 0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
        
	}
    void Import()
    {
        var deleteList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

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
            Apply(a, airObject);


        }
        //clear remaining objects from scene
        foreach (GameObject go in deleteList)
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
        
        MainController.mapController.LatLong2XY(lat, lng, alt, out x, out y, out alt);
      
        MainController.mapController.LatLong2XY(MainController.gpsController.GetLatitude(), MainController.gpsController.GetLongitude(), 0, out myX, out myY, out myAlt);
        x = x - myX;
        y = y - myY;
        sphere.Adjust(ref x, ref y, ref alt);

        obj.transform.position = new Vector3(x, alt, y);
        

        var dist = Vector3.Distance(obj.transform.position, new Vector3(myX, myY, 0));
        var scale = System.Math.Max(1.0f, dist/1000000.0f );
        obj.transform.localScale = new Vector3(scale, scale, scale); 

    }

}
