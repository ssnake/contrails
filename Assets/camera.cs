using UnityEngine;
using System.Collections;
using System;

public class Camera : MonoBehaviour {
    public int filter = 3;
    public float tiltAngle = 90.0F;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Quaternion target = Quaternion.Euler(getX()* -1, getY(), getZ()*-1);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime);
        Debug.Log("rotation: " + transform.rotation.ToString());
        
       
    }
    float getX()
    {
        float value = (float)Math.Round(Input.acceleration.z, filter);

        return value * tiltAngle;
    }
    float getY()
    {
        float value = (float)Math.Round(Input.acceleration.y, filter);
       
        return value * tiltAngle * 0f;
    }
    float getZ()
    {
        float value = (float)Math.Round(Input.acceleration.x, filter);
        if (value == 0)
        {
            value = Input.GetAxis("Horizontal");

        }
        Debug.Log("GetZ: " + value);
        return value * tiltAngle;
    }
    float getW()
    {
        return 0.0f * tiltAngle;
    }

}
