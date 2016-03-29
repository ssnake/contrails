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
            a.Apply(airObject);


        }
        //clear remaining objects from scene
        foreach(GameObject go in deleteList)
        {
            Destroy(go);
        }
	
	}

}
