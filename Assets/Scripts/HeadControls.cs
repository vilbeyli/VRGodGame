using UnityEngine;
using System.Collections;
 
public class HeadControls : MonoBehaviour {
 
    //======================================
    // Variable Declarations
    
    // static variables
    public static bool MouseLook = false;

    // handles
    public Transform target;

    // editor variables
    [SerializeField] private float _sensivity;
    [SerializeField] private float _distance;
    [SerializeField] private float _smooth;     // not used yet
 
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
	
	void LateUpdate ()
    {

        float dirY = Input.GetAxis("Horizontal");
        float dirX = Input.GetAxis("Vertical");

        Vector3 dir = MouseLook ? new Vector3(-dirX, dirY, 0) : new Vector3(dirX, dirY, 0);

        dir *= Time.deltaTime * _sensivity;

        // Mouse Look
	    if (MouseLook)
	    {
	        transform.Rotate(dir, Space.Self);
	    }

        // Orbit Around
	    else
	    {
            transform.parent.transform.Rotate(dir);
            //LookAtPlanet();
        }

    }
 
    // member functions
    public void LookAtPlanet()
    {
        transform.LookAt(target);
    }
}