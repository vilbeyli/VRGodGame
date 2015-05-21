using UnityEngine;
using System.Collections;
 
public class MainMenuScript : MonoBehaviour {
 
    //======================================
    // Variable Declarations
    
    // static variables
	
    // handles
    [SerializeField] private Canvas _menuCanvas;

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

    // member functions
    public void ToggleMainMenu()
    {
        _menuCanvas.enabled = !_menuCanvas.enabled;

        // change the game state from game manager too
        if(_menuCanvas.enabled) _gameManager.PauseGame();
        else                    _gameManager.ResumeGame();;
    }

    public void StartNewGameButton()
    {
        Debug.Log("BUTTON: START NEW GAME");
    }

    public void SettingsButton()
    {
        Debug.Log("BUTTON: SETTINGS");
    }

    public void QuitButton()
    {
        Debug.Log("BUTTON: QUIT");
    }
}