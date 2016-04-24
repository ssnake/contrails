using UnityEngine;
using System.Collections;

public class ConfigData : MonoBehaviour {

    public float visibleDistance = 60000.0f;
    
   // Use this for initialization
    void Start () {
        MainController.config = this;
        Camera.main.farClipPlane = visibleDistance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
