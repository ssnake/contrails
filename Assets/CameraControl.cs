using UnityEngine;
using System.Collections;
using System;


public class CameraControl : MonoBehaviour {
    public int filter = 3;
    public float tiltAngle = 90.0F;
    float speed = 100f;

    float filterDelta = 0.01f;
    Vector3 rotation;
   

    // Use this for initialization
    void Start () {
        MainController.camControl = this;
        Input.compass.enabled = true;
        rotation = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        
    }
	
	// Update is called once per frame
	void Update () {

        var valueX = getX();
        var valueY = getY();
        var valueZ = getZ();

        var dx = GetDelta(valueX, rotation.x);
        
        rotation.x = valueX;
        valueX += dx;

        var dy = GetDelta(valueY, rotation.y);
       
       
        rotation.y = valueY;
        valueY += dy;

        var dz = GetDelta(valueZ, rotation.z);
        
        rotation.z = valueZ;
        valueZ += dz;

        Quaternion target = Quaternion.Euler(valueX, valueY, valueZ);
        //Quaternion target = new Quaternion(dx, dy, dz, transform.rotation.w);

        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, target, Time.deltaTime);
        //transform.rotation = target;

        //transform.Rotate( -getX(), getY(), -getZ());
        //Debug.Log("rotation: " + transform.rotation.ToString());

    

    }
    

    public float getX()
    {
        //float value = (float)Math.Round(Input.acceleration.z * tiltAngle, filter) * -1.0f;
        float value = (float) Input.acceleration.z * tiltAngle * -1.0f;
        if (value == 0)
        {
            value = rotation.x + Input.GetAxis("Mouse Y")  * Time.deltaTime * speed * -1.0f;
            //Debug.Log("Mouse Y: " + Input.GetAxis("Mouse Y"));
        }
        return value;
    }

    public float getY()
    {
        float value = (float)Input.compass.magneticHeading;
        if (value == 0)
        {
            //transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
            value = rotation.y +  Input.GetAxis("Mouse X")  * Time.deltaTime * speed;
           // Debug.Log("Mouse X: " + Input.GetAxis("Mouse X"));
            
        }
        return value;
    }

    public float getZ()
    {
        float value = (float)Math.Round(Input.acceleration.x, filter) * -1.0f * tiltAngle;
        return value;
        
    }

    float getW()
    {
        return 0.0f * tiltAngle;
    }

    float GetDelta(float value, float prevValue)
    {
        if (Math.Abs(prevValue - value) > filterDelta)
        {
            //  Debug.Log("delta: " + (value - prevValue) * Time.deltaTime);
            return (value - prevValue) * Time.deltaTime;
            
        }
        else
        {
            return 0.0f;
        }
    }

}
