using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrossHairCollider : MonoBehaviour
{
    public Color selectColor;
    List<GameObject> selectedRoutes;
    // Use this for initialization
    void Start()
    {
        selectedRoutes = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        float minDist, maxDist;
        if (MainController.contrailsController.GetDistance(out minDist, out maxDist))
        { 
            var col = gameObject.GetComponent<SphereCollider>();
            col.radius = (maxDist - minDist) / 2.0f;
            gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, minDist + col.radius);

        }
    }
    void OnCollisionEnter(Collision col)
    {
     /*   if (col.gameObject.tag == "route")
        {
            selectedRoutes.Add(col.gameObject);
            //var lr = col.gameObject.GetComponent<LineRenderer>();
            //lr.SetColors(selectColor, selectColor);
        }
       */ Debug.Log("Collide enter");
    }
    void OnCollisionExit(Collision col)
    {
        /*if (col.gameObject.tag == "route")
        {
            
            selectedRoutes.Remove(col.gameObject);
            //var lr = col.gameObject.GetComponent<LineRenderer>();
            //lr.SetColors(Color.white, Color.white);
        }*/
        Debug.Log("Collide stay");
    }
}
