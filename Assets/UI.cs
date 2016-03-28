using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public bool showCameras = true;
    public bool showGyro = true;
    public Text accel;
    public Text gps;
	// Use this for initialization
	void Start () {

        MainController.ui = this;
    }
	
	// Update is called once per frame
	void Update () {

        accel.text = "";
        if (showCameras) showCams();
        if (showGyro) showGyroInfo();

    }
    void showCams()
    {
        accel.text += "Camera Count: " + WebCamTexture.devices.Length +"\n";

    }
    void showGyroInfo()
    {
        accel.text += "Input x: " + MainController.camControl.getX() + "\n";
        accel.text += "Input y: " + MainController.camControl.getY() + "\n";
        accel.text += "Input z: " + MainController.camControl.getZ() + "\n";


    }
    void showLocation()
    {

    }
}
