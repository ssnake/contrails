using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class AircraftController : MonoBehaviour {
    AirtcraftImporter importer;
    public GameObject aircraft;

	// Use this for initialization
	void Start () {
        importer = new AirtcraftImporter();
	}
	
	// Update is called once per frame
	void Update () {
        var deleteList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));

        foreach (Aircraft a in importer.Import())
        {
            var airObject = GameObject.FindWithTag("aircraft_" + a.id);
            //if new aircraft
            if (airObject == null)
            {
                Instantiate(aircraft);

            } else
            {
                deleteList.Remove(airObject);
            }
            Apply(a, airObject);


        }
        //clear remaining objects from scene
        foreach(GameObject go in deleteList)
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
        MainController.mapController.LatLong2XY(lat, lng, out x, out y);
        obj.transform.Translate(x, y, craft.altitude);

    }

}
