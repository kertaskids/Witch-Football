using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match : MonoBehaviour
{
    public enum GameState{
        PreMatch, 
        MatchPlaying,
        Goal,
        PostGoal, 
        TimeOver,
        PostMatch
    };
    public GameState gamestate;
    public float Timer;
    public Team TeamA;
    public Team TeamB;
    public WitchController[] Scorers;
    private bool _isPaused;
    private float _stateDelay;
    // <Delete later>
    private float _aSecondTimer=1;

    void Start(){
        gamestate = GameState.PreMatch;
    }
    void Update() {
        if(!_isPaused){
            // Do all the stuff here
            if(gamestate == GameState.PreMatch) PreMatch();
            if(gamestate == GameState.MatchPlaying) MatchPlaying();
            if(gamestate == GameState.Goal) Goal();
            if(gamestate == GameState.PostGoal) PostGoal();
            if(gamestate == GameState.TimeOver) TimeOver();
            if(gamestate == GameState.PostMatch) PostMatch();
        } else {
            //_stateDelay -= Time.deltaTime;
            if(gamestate == GameState.PostGoal) PostGoal();
            
        }
    }

    public void PreMatch(){
        Timer = 180f; 
        //TeamA.Initialize();
        //TeamB.Initialize();
        _isPaused = false;
        //_stateDelay = 3f;

        if(_stateDelay <= 0) {
            gamestate = GameState.MatchPlaying;
            _stateDelay = 3f;
        }
        //Init Scorers; 
        // Assign score and time 
        // Show players and stats
        // Set up for the match
    }
    public void MatchPlaying(){
        Timer -= Time.deltaTime;
        if(Timer <= 0){
            gamestate = GameState.TimeOver;
        }
        // Player can: action, cast magic, score a goal.
        // Time is limited. Time is paused when playing a cutscene for casting heavy magic. 
        // Spawn mystery box. 
        // Tiles can be manipulated and mutated
        _aSecondTimer -= Time.deltaTime;
        if(_aSecondTimer<=0){
            //Debug.Log("Timer: " + (int)(Timer/60) + "Minutes " + (int)(Timer%60) + "Seconds.");
            _aSecondTimer = 1f;
        }

    }
    public void Goal(){
        // A player score a goal by making the ball triggers the goal line. 
        if(_stateDelay <= 0) {
            gamestate = GameState.PostGoal;
            _isPaused = true;
            _stateDelay = 3f;
        }
        _stateDelay -= Time.deltaTime;
    }
    public void PostGoal(){
        if(_stateDelay <= 0) {
            gamestate = GameState.MatchPlaying;
            _isPaused = false;
            _stateDelay = 3f;
            if(TeamA != null && TeamB!=null){
                Debug.Log("Score A: "+TeamA.Score + ". Score B:"+TeamB.Score);
            }
        }
        _stateDelay -= Time.deltaTime;
        Debug.Log("StateDelay:"+_stateDelay);
        
        // <Delete later>
        // Show the player and historical stats
        // Preparing the after goal set up
        // Time of the match is paused
    }
    public void TimeOver(){
        // The time of match is over
        if(_stateDelay <= 0) {
            gamestate = GameState.PostMatch;
            _stateDelay = 3f;
            if(TeamA != null && TeamB!=null){
                Debug.Log("Score A: "+TeamA.Score + ". Score B:"+TeamB.Score);
            }
        }
        _stateDelay -= Time.deltaTime;
    }
    public void PostMatch(){
        // Show the winner and stats
        // Might have MVP feature
        if(_stateDelay <= 0) {
            gamestate = GameState.PostMatch;
            _stateDelay = 3f;
            if(TeamA != null && TeamB!=null){
                Debug.Log("The match is over. Score A: "+TeamA.Score + ". Score B:"+TeamB.Score);
            }
        }
        _stateDelay -= Time.deltaTime;
    }

}
