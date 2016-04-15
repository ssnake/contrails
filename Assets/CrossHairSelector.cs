using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrossHairSelector : MonoBehaviour {
    public GameObject crosshair;
    public Camera camera;
    public Canvas canvas;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var crossRect = crosshair.GetComponent<RectTransform>();
        var canvasRect = canvas.GetComponent<RectTransform>();

        
        List<GameObject> list = new List<GameObject>();
        list.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("aircraft")));
        list.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("building")));

        foreach (var o in list)
        {
            var vect = o.transform.localPosition;
            var craftImported = o.GetComponent<DataHolder>().aircraftImported;
            // var r = ConvertRects(canvasRect, crossRect);
            var r = crossRect.rect;
            if (r.Contains(vect))
            {
                Debug.Log("Selected: " + craftImported.ToString());
            }
        }
	
	}
    Rect ConvertRects(RectTransform canvas, RectTransform obj)
    {
        Rect r = new Rect();
        r.x = canvas.sizeDelta.x * obj.anchorMin.x + obj.localPosition.x - obj.pivot.x * obj.sizeDelta.x;
        r.y = canvas.sizeDelta.y * obj.anchorMin.y + obj.localPosition.y - obj.pivot.y * obj.sizeDelta.y;
        r.width = obj.sizeDelta.x;
        r.height = obj.sizeDelta.y;
        return r;

    }
}
