using UnityEngine;
using System.Collections;
 
public class Rotation : MonoBehaviour {
 
    //======================================
    // Variable Declarations
    
    // static variables
	
    // handles
	
    // private variables
 
    // public variables
    public float speed;
 
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
	
	void Update () 
    {

	    if (!HeadControls.MouseLook)
	    {
	        float dirY = Input.GetAxis("Horizontal");
	        float dirX = Input.GetAxis("Vertical");
	        Vector3 dir = new Vector3(dirX, dirY, 0);
	        dir *= Time.deltaTime*speed;

	        transform.Rotate(dir);
	    }
    }
 
    // member functions
	
}