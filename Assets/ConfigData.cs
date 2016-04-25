using UnityEngine;
using System.Collections;

public class ConfigData : MonoBehaviour {

    public float visibleDistance = 60000.0f;
    
   // Use this for initialization
    void Awake () {
        MainController.config = this;
        Camera.main.farClipPlane = visibleDistance * 2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
