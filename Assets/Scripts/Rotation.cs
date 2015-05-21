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
    public Vector3 vect;
    public bool world;
 
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
	    Vector3 vec = vect*Time.deltaTime*speed;

	    if (world)
	    {
            transform.Rotate(vec, Space.World);
	    }
	    else
	    {
            transform.Rotate(vec, Space.Self);
	    }
	}
 
    // member functions
	
}