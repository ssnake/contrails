using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Webcamera : MonoBehaviour
{

    WebCamDevice[] devices;
    WebCamDevice cam;
    WebCamTexture camTexture;
    // Image rotation
    Vector3 rotationVector = new Vector3(0f, 0f, 0f);

    // Use this for initialization

    void Start()
    {

        devices = WebCamTexture.devices;
        camTexture = null;
        for (var i = 0; i < devices.Length; i++)
            if (!devices[i].isFrontFacing)
            {
                cam = devices[i];
                camTexture = new WebCamTexture(cam.name);
                camTexture.Play();
                break;
            }
        GetComponent<RawImage>().texture = camTexture;


    }

    // Update is called once per frame
    void Update()
    {
        // Rotate image to show correct orientation 
        if (camTexture != null)
        {
            rotationVector.z = -camTexture.videoRotationAngle;
            GetComponent<RawImage>().rectTransform.localEulerAngles = rotationVector;
        }


    }
}
