using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class camera : MonoBehaviour {
   
    WebCamDevice[] devices;
    WebCamDevice cam;
    WebCamTexture camTexture;
    // Use this for initialization

    void Start () {
   
        devices = WebCamTexture.devices;
   
        for (var i = 0; i < devices.Length; i++)
            if (!devices[i].isFrontFacing)
            {
                cam = devices[i];
                camTexture = new WebCamTexture(cam.name);
                camTexture.Play();
                break;
            }
            
    }
	
	// Update is called once per frame
	void Update () {
        var renderer = GetComponent<Renderer>();

        renderer.material.mainTexture = camTexture;
   
    }
}
