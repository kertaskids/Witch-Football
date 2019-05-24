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
    }
    public GameState gamestate;
    public float Timer;
    public Team TeamA;
    public Team TeamB;
    public WitchController[] Scorers;
    private bool _isPaused;
    private float _stateDelay;
    private float _oneSecondTimer = 1;
    private bool _initiated;

    void Start(){
        _initiated = false;
        _isPaused   = false;
        gamestate = GameState.PreMatch;
        // Need to make sure the assignment of the team & witches
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
            // Uncomment if we want when the celebration, the time is still running. 
            //_stateDelay -= Time.deltaTime;
            if(gamestate == GameState.PostGoal) PostGoal();
        }
    }

    public void PreMatch(){
        if(!_initiated){
            Timer = 180f; 
            TeamA = new Team(Team.TeamParty.TeamA);
            TeamB = new Team(Team.TeamParty.TeamB);
            _isPaused   = false;
            _stateDelay = 3f;
            _initiated  = true;

            // <Edit later> Add these features
            // Init Scorers; 
            // Show players and stats
            ShowPlayersStats();
            // Set up for the match: ball position, player position, goal, player's skill
        }
        if(_stateDelay <= 0) {
            gamestate   = GameState.MatchPlaying;
            _stateDelay = 3f;
        }
        // Comment this to test the gamestate
        _stateDelay -= Time.deltaTime;
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
        _oneSecondTimer -= Time.deltaTime;
        if(_oneSecondTimer<=0){
            //Debug.Log("Timer: " + (int)(Timer/60) + "Minutes " + (int)(Timer%60) + "Seconds.");
            _oneSecondTimer = 1f;
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
    // <Edit later> Change in the UI
    void ShowPlayersStats(){
        Debug.Log(TeamA.teamParty.ToString() + " Statistic:");
        foreach (WitchController w in TeamA.witches)
        {
            //Character Stat
            Debug.Log(w.name + ", HP: " + w.character.healthPoint.current + 
                        ", DamageToHP: " + w.character.tackledDamageToHealth.current + 
                        ", PassPower" + w.character.passPower.current + 
                        ", Speed: " + w.character.moveSpeed.current);
            //MagicSkill
            Debug.Log("LightSkill: " + w.character.lightMagicSkill.name + 
                        ", HeavySkill: " + w.character.heavyMagicSkill.name);
        }

        Debug.Log(TeamB.teamParty.ToString() + " Statistic:");
        foreach (WitchController w in TeamB.witches)
        {
            Debug.Log(w.name + ", HP: " + w.character.healthPoint.current + 
                        ", DamageToHP: " + w.character.tackledDamageToHealth.current + 
                        ", PassPower" + w.character.passPower.current + 
                        ", Speed: " + w.character.moveSpeed.current);
            Debug.Log("LightSkill: " + w.character.lightMagicSkill.name + 
                        ", HeavySkill: " + w.character.heavyMagicSkill.name);
        }
    }

}
