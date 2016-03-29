using UnityEngine;
using System.Collections;
using System;


public class CameraControl : MonoBehaviour {
    public int filter = 3;
    public float tiltAngle = 90.0F;
    float speed = 2000f;
    // Use this for initialization
    void Start () {
        MainController.camControl = this;
        Input.compass.enabled = true;
        

    }
	
	// Update is called once per frame
	void Update () {

        Quaternion target = Quaternion.Euler(getX(), getY(), getZ());
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
        //transform.Rotate( -getX(), getY(), -getZ());
        //Debug.Log("rotation: " + transform.rotation.ToString());
        
       
    }
    public float getX()
    {
        float value = (float)Math.Round(Input.acceleration.z * tiltAngle, filter) * -1.0f;
        if (value == 0)
        {
            value = transform.rotation.eulerAngles.x + Input.GetAxis("Mouse Y")  * Time.deltaTime * speed * -1.0f;
            Debug.Log("Mouse Y: " + Input.GetAxis("Mouse Y"));
        }

        return value ;
    }
    public float getY()
    {
        float value = (float)Input.compass.magneticHeading;
        if (value == 0)
        {
            //transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
            value =transform.rotation.eulerAngles.y +  Input.GetAxis("Mouse X")  * Time.deltaTime * speed;
            Debug.Log("Mouse X: " + Input.GetAxis("Mouse X"));

        }
        return value ;
    }
    public float getZ()
    {
        float value = (float)Math.Round(Input.acceleration.x, filter) * -1.0f;
        Debug.Log("GetZ: " + value);
        return value * tiltAngle;
    }
    float getW()
    {
        return 0.0f * tiltAngle;
    }

}
