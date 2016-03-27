using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI : MonoBehaviour {
    public bool showCameras = true;
    public bool showGyro = true;
    Text text;
	// Use this for initialization
	void Start () {
	    text = GetComponent<Text>(); 
    }
	
	// Update is called once per frame
	void Update () {

        text.text = "";
        if (showCameras) showCams();
        if (showGyro) showGyroInfo();

    }
    void showCams()
    {
        text.text += "Camera Count: " + WebCamTexture.devices.Length +"\n";

    }
    void showGyroInfo()
    {
        text.text += "Gyro x: " + Input.acceleration.x + "\n";
        text.text += "Gyro y: " + Input.acceleration.y + "\n";
        text.text += "Gyro z: " + Input.acceleration.z + "\n";


    }
}
