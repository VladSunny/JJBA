using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoControls : MonoBehaviour 
{

    public ParticleManipulator PM;

    public Toggle enableToggle;
    public Slider gravitySlider;
    public Toggle colorToggle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
        PM.enabled = enableToggle.isOn;
        PM.GravityForceSettings.Force = gravitySlider.value;
        PM.ColorSettings.enable = colorToggle.isOn;
	}
}
