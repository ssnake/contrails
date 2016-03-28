using UnityEngine;
using System.Collections;
using System;

public class CameraControl : MonoBehaviour {
    public int filter = 3;
    public float tiltAngle = 90.0F;
    // Use this for initialization
    void Start () {
        MainController.camControl = this;
        Input.compass.enabled = true;

    }
	
	// Update is called once per frame
	void Update () {

        Quaternion target = Quaternion.Euler(getX()* -1, getY(), getZ()*-1);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
       
        //Debug.Log("rotation: " + transform.rotation.ToString());
        
       
    }
    public float getX()
    {
        float value = (float)Math.Round(Input.acceleration.z, filter);

        return value * tiltAngle;
    }
    public float getY()
    {
        float value = (float)Input.compass.magneticHeading;
        if (value == 0)
        {   
            
            value = transform.rotation.eulerAngles.y  + Input.GetAxis("Mouse X") * tiltAngle * 2.0f;
            Debug.Log("getY: " + value);

        }
        return value ;
    }
    public float getZ()
    {
        float value = (float)Math.Round(Input.acceleration.x, filter);
        Debug.Log("GetZ: " + value);
        return value * tiltAngle;
    }
    float getW()
    {
        return 0.0f * tiltAngle;
    }

}
