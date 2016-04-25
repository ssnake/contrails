using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class DataHolder : MonoBehaviour
{
    public AircraftImported aircraftImported;
}
public class UIHighlighter : MonoBehaviour {
    public Canvas canvas;
    public GameObject selectUI;
    public GameObject selectUIBuilding;
    public Camera camera;
    
    // Use this for initialization
    void Start () {
        
 	}
	
	// Update is called once per frame
	void Update () {
        var list = MainController.aircraftController.AircraftList;
        var listBuilding = MainController.aircraftController.BuildingList;
        List2UI(list, "aircraft_", "aircraft", selectUI);
       // List2UI(listBuilding, "building_", "building", selectUIBuilding);


    }
    void List2UI(List<AircraftImported> list, string namePrefix, string tagName, GameObject selectObject)
    {
        
        var deleteList = new List<GameObject>(GameObject.FindGameObjectsWithTag(tagName));
        if (list != null)
        {
            foreach (var obj in list)
            {
                if (!IsVisible(obj)) continue; 
                    
                var v = WorldToCanvasPosition(canvas.GetComponent<RectTransform>(), camera, obj.position);
                var select = GameObject.Find(namePrefix + obj.origin.id);

                if (select == null)
                {
                    select = Instantiate(selectObject);
                    select.name = namePrefix + obj.origin.id;
                    select.transform.SetParent(canvas.transform);
                    select.transform.localEulerAngles = Vector3.zero;
                    select.AddComponent<DataHolder>();
                } else
                    deleteList.Remove(select);

                var holder = select.GetComponent<DataHolder>();
                holder.aircraftImported = obj;
                select.transform.localPosition = new Vector3(v.x, v.y, 0.0f);
                
                var scale = 1.0f / Vector3.Distance(Vector3.zero, obj.position) * 40;

                select.transform.localScale = Vector3.one * System.Math.Max(0.1f, scale);

            }
        }
        foreach (var o in deleteList)
        {
            Destroy(o);
        }

    }
    bool IsVisible(AircraftImported craft)
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(camera);
        Bounds b = new Bounds(craft.position, Vector3.one);


        return GeometryUtility.TestPlanesAABB(planes, b);
    }
    static public Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
    {
        //Vector position (percentage from 0 to 1) considering camera size.
        //For example (0,0) is lower left, middle is (0.5,0.5)
        Vector2 temp = camera.WorldToViewportPoint(position);


        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        temp.x *= canvas.sizeDelta.x;
        temp.y *= canvas.sizeDelta.y;

        //The result is ready, but, this result is correct if canvas recttransform pivot is 0,0 - left lower corner.
        //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
        //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
        //returned value will still be correct.

        temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return temp;
    }
}
