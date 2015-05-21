using UnityEngine;
using System.Collections;
 
public class GameManager : MonoBehaviour {

    public enum GameState
    {
        Play,
        Pause
    };

    //======================================
    // Variable Declarations
    
    // static variables
	
    // handles
	
    // private variables
    private GameState _state;



    // public variables
 
 
    //======================================
    // Function Definitions
 
    // getters & setters
    public GameState State
    {
        get { return _state; }
    }

    // unity functions
	void Awake ()
	{
	
	}
	
	void Start ()
	{
        // game starts in playing mode
	    _state = GameState.Play;
	}
	
	void Update () 
    {
	
	}

    // member functions
    public void PauseGame()
    {
        _state = GameState.Pause;
    }

    public void ResumeGame()
    {
        _state = GameState.Play;
    }

  
}