﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour {
 
    //======================================
    // Variable Declarations
    
    // static variables
	
    // handles
    public HeadControls _headControls;

    // private variables
 
    // public variables
 
 
    //======================================
    // Function Definitions
 
    // getters & setters
 
    // unity functions
	void Awake ()
	{
	    
	}
	
	void Start () 
    {
        HeadControls.MouseLook = GameObject.FindGameObjectWithTag("MouseLook Toggle").GetComponent<Toggle>().isOn;
	}
	
	void Update () 
    {
	
	}
 
    // member functions
    public void MouseLookToggle(bool On)
    {
        Debug.Log("Mouse Look Toggle: " + On);
        HeadControls.MouseLook = On;

        // 
        if (!On)
        {
            _headControls.LookAtPlanet();
        }
    }

}