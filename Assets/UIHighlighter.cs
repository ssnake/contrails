using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class SelectHolder : MonoBehaviour
{
    public GameObject select;
}
public class UIHighlighter : MonoBehaviour {
    public Canvas canvas;
    public GameObject selectUI;
    public Camera camera;
    
    // Use this for initialization
    void Start () {
        
 	}
	
	// Update is called once per frame
	void Update () {

        var list = MainController.aircraftController.AircraftList;
        var deleteList = new List<GameObject>(GameObject.FindGameObjectsWithTag("aircraft"));

        foreach(var obj in list)
        {
            var v = WorldToCanvasPosition(canvas.GetComponent<RectTransform>(), camera, obj.position);
            var select = GameObject.Find("aircraft_" + obj.origin.id);

            if (select == null)
            {
                select = Instantiate(selectUI);
                select.name = "aircraft_" + obj.origin.id;
                select.transform.SetParent(canvas.transform);
            }
            else deleteList.Remove(select);

            select.transform.localPosition = new Vector3(v.x, v.y, 0.0f);
            var scale = 1.0f / Vector3.Distance(Vector3.zero, obj.position) * 50;

            select.transform.localScale = Vector3.one * scale;
            Debug.Log("v: " + v.ToString());
            Debug.Log("pos: " + select.transform.localPosition.ToString());

        }
        foreach(var o in deleteList)
        {
            Destroy(o);
        }

    }
    
    private Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 position)
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
