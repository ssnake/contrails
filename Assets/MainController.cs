using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {
    public static MainController controller;
    public static UI ui;
    public static CameraControl camControl;


	// Use this for initialization
	void Awake () {
        if (MainController.controller == null) {
            MainController.controller = new MainController();
        };
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
