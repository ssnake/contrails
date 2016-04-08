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
        if (!Application.isEditor)
        {
            devices = WebCamTexture.devices;
            camTexture = null;
            for (var i = 0; i < devices.Length; i++)
                if (!devices[i].isFrontFacing || Application.isEditor)
                {
                    cam = devices[i];
                    camTexture = new WebCamTexture(cam.name);

                    camTexture.Play();
                    break;
                }
            RawImage ri = GetComponent<RawImage>();
            var arf = ri.GetComponent<AspectRatioFitter>();
            arf.aspectRatio = 1.0f * camTexture.width / camTexture.height;
            ri.texture = camTexture;
        }
        //var fov = GetFOV(3.54f);
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
    public float GetFOV(float focalLengthmm)
    {
        //standard film size
        int filmHeight = 24;
        int filmWidth = 36;

        //Formula to convert focalLength to field of view - In Unity they use Vertical FOV.
        //So we use the filmHeight to calculate Vertical FOV.
        double fovdub = Mathf.Rad2Deg * 2.0 * System.Math.Atan(filmHeight / (2.0 * focalLengthmm));
        return  (float)fovdub;
    }
}
