using UnityEngine;
using System.Collections;
 
public class HeadControls : MonoBehaviour {
 
    //======================================
    // Variable Declarations
    
    // static variables
    public static bool MouseLook = false;

    // handles
    public Transform Planet;

    // private variables
 
    // public variables
    public float Sensivity;
 
 
    //======================================
    // Function Definitions
 
    // getters & setters
 
    // unity functions
	void Awake ()
	{
	
	}
	
	void Start () 
    {
	
	}
	
	void FixedUpdate ()
    {

        float dirX = Input.GetAxis("Horizontal") * Sensivity;
        float dirY = Input.GetAxis("Vertical") * Sensivity;
        Vector3 dir = new Vector3(dirX, dirY, 0);
        //Debug.Log(dir);

        // Mouse Look
	    if (MouseLook)
	    {
	        transform.Rotate(dir * Time.deltaTime);
	    }

        // Orbit Around
	    else
	    {
	        transform.LookAt(Planet);
	        transform.Translate(dir*Time.deltaTime);
	    }

    }
 
    // member functions
    public void LookAtPlanet()
    {
        transform.LookAt(Planet);
    }
}