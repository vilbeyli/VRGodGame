using UnityEngine;
using System.Collections;
 
public class InputManager : MonoBehaviour {
 
    //======================================
    // Variable Declarations
    
    // static variables
	
    // handles

    [SerializeField] private MainMenuScript _menuScript;
    [SerializeField] private GameManager _gameManager;

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
	
	}
	
	void Update ()
	{
    
	    ReadKeyboardInput();
	}

    private void ReadKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _menuScript.ToggleMainMenu();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<HeadControls>().ToggleViewMode();
        }
    }

    // member functions
	
}