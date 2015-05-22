using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeadControls : MonoBehaviour {

    public enum ViewMode { Planet, Inventory }

    //======================================
    // Variable Declarations
    
    // private variables
    private ViewMode _viewMode;

    // handles
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _viewCamera;

    [SerializeField] private GameManager _gameManager;
    [SerializeField] private Text _viewModeText;

    // editor variables
    [SerializeField] private float _sensivity;
    [SerializeField] private float _distance;
    //[SerializeField] private float _smooth;     // not used yet, can be used for polishing later on
 
    //======================================
    // Function Definitions
 
    // getters & setters
 
    // unity functions
	void Awake ()
	{
	
	}
	
	void Start () 
    {
	    _viewMode = ViewMode.Planet;
	}
	
	void LateUpdate ()
    {
	    if (_gameManager.State == GameManager.GameState.Play)
	    {
	        switch (_viewMode)
	        {
	            case ViewMode.Planet:
	                PlanetView();       // Orbit Around Planet
	                break;

                case ViewMode.Inventory:
	                InventoryView();    // Stand Still
	                break;
	        }
	    }   
    }
 
    // public member functions
    public void LookAtPlanet()
    {
        _viewCamera.LookAt(_target);
    }

    public void ToggleViewMode()
    {
        if (_viewMode == ViewMode.Planet)   _viewMode = ViewMode.Inventory;
        else                                _viewMode = ViewMode.Planet;;

        _viewModeText.text = "View Mode: " + (_viewMode == ViewMode.Planet ? "Planet" : "Inventory");
    }

    // private member functions
    private void PlanetView()
    {
        // Rotating parent object makes it look like the camera is orbiting the planet. 
        // (there might be issues?) This tutorial can be followed to make it work using 
        // just another way: https://www.youtube.com/watch?v=TdjoQB43EsQ
        float axisY = Input.GetAxis("Horizontal");
        float axisX = Input.GetAxis("Vertical");
        
        Vector3 rotation = new Vector3(axisX, axisY, 0);    // player input component of the rotation
        rotation *= Time.deltaTime * _sensivity;            // some fine tuning too

        _viewCamera.parent.transform.Rotate(rotation);      // action!
    }

    private void InventoryView()
    {
        // Still camera - interactions with objects on the planet - inventory
        // TODO: follow trello for implementation details
    }
}