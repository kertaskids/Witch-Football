﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

// ------<FOR TESTING ONLY>------
#if UNITY_EDITOR
using UnityEditor;
#endif
// ------<FOR TESTING ONLY>------

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
    public Text TimerText;
    public Text TeamAScore;
    public Text TeamBScore;
    public WitchController[] Scorers;
    public GameObject GoalA;
    public GameObject GoalB;
    public GameObject[] mysteryBoxes;
    public Team TeamA;
    public Team TeamB;
    private float _stateDelay;
    public float stateMaxDelay = 3f;
    private float _mysteryBoxDelay;
    public float mysteryBoxMaxDelay = 30f;
    private float _oneSecondTimer = 1;

    private bool _isPaused;
    private bool _initiated;

    private Vector3 _initialBallPos;
    private GameObject ball;

    private TitlePopUp _titlePopUp;

    void Start(){
        _initiated = false;
        _isPaused  = false;
        gamestate  = GameState.PreMatch;
        _initialBallPos = new Vector3(4.5f, 2, 2);
        _mysteryBoxDelay = mysteryBoxMaxDelay;
        if(ball == null) {
            ball = GameObject.Find("Ball");
        }
        _titlePopUp =  Resources.FindObjectsOfTypeAll<TitlePopUp>().FirstOrDefault();
        //GameObject.FindObjectOfType<TitlePopUp>().gameObject; 
        
    }
    void Update(){
        if(!_isPaused){
            if(gamestate == GameState.PreMatch) PreMatch();
            if(gamestate == GameState.MatchPlaying) MatchPlaying();
            if(gamestate == GameState.Goal) Goal();
            if(gamestate == GameState.PostGoal) PostGoal();
            if(gamestate == GameState.TimeOver) TimeOver();
            if(gamestate == GameState.PostMatch) PostMatch();
        } else {
            // Uncomment if we want when the celebration, time still running. 
            //_stateDelay -= Time.deltaTime;
            if(gamestate == GameState.PostGoal) PostGoal();
        }
    }
    void LateUpdate() {
        if(PlayerAndBallFallen()){
            // <Edit later> Simply respawn the player or the ball in the middle of arena, with stunned duration. 
            SetupMatch();
        }
    }
    public void PreMatch(){
        if(!_initiated){
            Timer = 180f; 
            TeamA = new Team(Team.TeamParty.TeamA);
            TeamB = new Team(Team.TeamParty.TeamB);
            SetPlayersControl(false); 
            SetupMatch();
            ShowPlayersStats();
            _isPaused   = false;
            _stateDelay = stateMaxDelay;
            _initiated  = true;
            _mysteryBoxDelay = mysteryBoxMaxDelay;
            // <Edit later> Init all the UIs (including PinUp and HUD) here
            ShowTitle("READY?", 3f);
            
        }

        if(_stateDelay <= 0) {
            gamestate   = GameState.MatchPlaying;
            _stateDelay = stateMaxDelay;
            SetPlayersControl(true);
            ShowTitle("GO!", 1f, 128f);
        }
        _stateDelay -= Time.deltaTime;
    }
    public void MatchPlaying(){
        Timer -= Time.deltaTime;
        if(Timer <= 0){
            gamestate = GameState.TimeOver;
            SetPlayersControl(false);
        }

        // <Edit later> 
        // Time is limited. Timer is slower when playing a cutscene for casting heavy magic. 
        // Tiles can be manipulated and mutated

        if(_mysteryBoxDelay <= 0){
            SpawnMysteryBox();
            _mysteryBoxDelay = mysteryBoxMaxDelay;
        }
        _mysteryBoxDelay -= Time.deltaTime;

        _oneSecondTimer -= Time.deltaTime;
        if(_oneSecondTimer <= 0){
            TimerText.text = ((int)(Timer/60)).ToString() + ":" + ((int)(Timer % 60)).ToString();
            _oneSecondTimer = 1f;
        } 
    }
    public void Goal(){ 
        // <Edit later>
        // Do this once right after ball trigger the goal line. 
        if(_stateDelay <= 0) {
            gamestate = GameState.PostGoal;
            _isPaused = true;
            _stateDelay = stateMaxDelay;
            SetPlayersControl(false); // <Creates a bug on ballposession>
            //Debug.Log("Ball State:" + ball.GetComponent<Ball>().ballState + " ");
            //Debug.Log("GameState: " + gamestate.ToString());
            //Debug.Break();
            ShowTitle("GOAL!", 3f, 128f);
            //GameObject title = GameObject.Instantiate("Assests/Prefabs/UI/TitlePopUp", Vector3.zero, Quaternion.identity) as GameObject;
        }
        _stateDelay -= Time.deltaTime;

        // Add Celebration 
    }
    public void GoalScored(WitchController scorer, Goal goal){ 
        if(scorer.teamParty == Team.TeamParty.TeamA) {
            if(goal.teamParty == Team.TeamParty.TeamB){
                // Normal Goal
                TeamA.Score += 1;
            } else if (goal.teamParty == Team.TeamParty.TeamA){
                // Own Goal
                TeamB.Score += 1;
            }
        } else if (scorer.teamParty == Team.TeamParty.TeamB){
            if(goal.teamParty == Team.TeamParty.TeamA){
                TeamB.Score += 1;
            } else if (goal.teamParty == Team.TeamParty.TeamB){
                TeamA.Score += 1;
            }
        }
        TeamAScore.text= TeamA.Score.ToString();
        TeamBScore.text= TeamB.Score.ToString();
        
        List<WitchController> witchesTemp = new List<WitchController>();
        foreach (WitchController w in Scorers){
            witchesTemp.Add(w);
        }
        witchesTemp.Add(scorer);
        Scorers = witchesTemp.ToArray();
        gamestate = GameState.Goal;
        // <Edit later> If we want to let player attack in postGoal to get manna, just uncomment it. 
        //SetPlayersControl(true);
        Debug.Log("Score A: "+TeamA.Score + ". Score B:"+TeamB.Score);
        
        _oneSecondTimer -= Time.deltaTime;
        if(_oneSecondTimer<=0){
            //Debug.Log("Timer: " + (int)(Timer/60) + "Minutes " + (int)(Timer%60) + "Seconds.");
            _oneSecondTimer = 1f;
        }
    }
    public void PostGoal(){
        if(_stateDelay <= 0) {
            gamestate = GameState.MatchPlaying;
            _isPaused = false;
            _stateDelay = stateMaxDelay;
            SetPlayersControl(true);
            if(TeamA != null && TeamB != null){
                Debug.Log("Score A: "+TeamA.Score + ". Score B:"+TeamB.Score);
            }
            SetupMatch();
        }
        _stateDelay -= Time.deltaTime;
        //Debug.Log("StateDelay:"+_stateDelay);
        Debug.Log("Ball State:" + ball.GetComponent<Ball>().ballState + " ");
            
        // <Edit later> Refresh the stun duration of the player
    }
    public void TimeOver(){
        // The time of match is over
        if(_stateDelay <= 0) {
            gamestate = GameState.PostMatch;
            _stateDelay = stateMaxDelay;
            SetPlayersControl(false);
            if(TeamA != null && TeamB!=null){
                Debug.Log("Score A: "+TeamA.Score + ". Score B:"+TeamB.Score);
            }
        }
        _stateDelay -= Time.deltaTime;
        // <Edit later> 
        // Play winning Animation 
    }
    public void PostMatch(){
        // Show the winner and stats
        // Might have MVP feature
        if(_stateDelay <= 0) {
            gamestate = GameState.PostMatch;
            //_stateDelay = _stateMaxDelay;
            if(TeamA != null && TeamB!=null){
                Debug.Log("The match is over. Score A: "+TeamA.Score + ". Score B:"+TeamB.Score);
            }
            // Show list of scorers
            Debug.Log("Scorers: " );
            foreach (WitchController s in Scorers)
            {
                Debug.Log(s.gameObject.name);
            }
            // or just simply Debug.break();
            #if UNITY_EDITOR
            EditorApplication.isPaused = true;
            #endif

            // <Edit later> Go back to menu screen
            
        } else {
            _stateDelay -= Time.deltaTime;
        }
    }

    // <Edit later> Change in the UI
    void ShowPlayersStats(){
        Debug.Log(TeamA.teamParty.ToString() + "'s statistic:");
        foreach (WitchController w in TeamA.witches)
        {   
            //Debug.Log("Character null on " + w.name + "? " + (w.character == null));
            //Debug.Log("Name " + w.name);
            //Debug.Log(w.gameObject.name + " on " + TeamA.teamParty.ToString());
            if(w.enabled){
                //Character Stat
                Debug.Log(w.name + ", HP: " + w.witch.character.healthPoint.current + 
                            ", DamageToHP: " + w.witch.character.damageHealth.current + 
                            ", PassPower" + w.witch.character.passPower.current + 
                            ", Speed: " + w.witch.character.moveSpeed.current +
                            ", LightSkill: " + w.witch.character.lightMagicSkill.name + 
                            ", HeavySkill: " + w.witch.character.heavyMagicSkill.name);
            }
        }
        Debug.Log(TeamB.teamParty.ToString() + " Statistic:");
        foreach (WitchController w in TeamB.witches)
        {
            if(w.enabled){
                //Character Stat
                Debug.Log(w.name + ", HP: " + w.witch.character.healthPoint.current + 
                            ", DamageToHP: " + w.witch.character.damageHealth.current + 
                            ", PassPower" + w.witch.character.passPower.current + 
                            ", Speed: " + w.witch.character.moveSpeed.current +
                            ", LightSkill: " + w.witch.character.lightMagicSkill.name + 
                            ", HeavySkill: " + w.witch.character.heavyMagicSkill.name);
            }
        }
    }
    void SetupMatch(){
        // Nullifies the ball first 
        GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
        foreach (GameObject w in allWitches)
        {
            if(w.GetComponent<WitchController>()._possessingBall) {
                w.GetComponent<WitchController>().BallReleasing(_initialBallPos, Quaternion.identity, ball.transform.forward, Vector3.zero, Vector3.zero, Vector3.zero);
            }
        }
        // Ball Position
        ball.transform.position = _initialBallPos;
        ball.transform.rotation = Quaternion.identity; 
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        // Witches Position
        for(int i = 0; i < TeamA.witches.Length; i++){
            TeamA.witches[i].gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            TeamA.witches[i].transform.position = new Vector3(ball.transform.position.x - (i+1), 
                                                                ball.transform.position.y, 
                                                                ball.transform.position.z);                             
            
        }
        for(int i = 0; i < TeamB.witches.Length; i++){
            TeamB.witches[i].gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            TeamB.witches[i].transform.position = new Vector3(ball.transform.position.x + (i+1), 
                                                                ball.transform.position.y, 
                                                                ball.transform.position.z);
        }
    }
    void SetPlayersControl(bool allowed){
        GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
        foreach (GameObject w in allWitches){
            WitchController wc = w.GetComponent<WitchController>();
            wc.ControlAllowed = allowed;
            // Remove the stun duration
            wc.witch.character.stunnedDuration.current = 0f;
        }
    }
    void SpawnMysteryBox(){
        if(mysteryBoxes != null || mysteryBoxes.Length >= 0){
            int r = Random.Range(0, mysteryBoxes.Length);
            GameObject mysteryBox = GameObject.Instantiate(mysteryBoxes[r]);
            Transform rootTiles = GameObject.Find("Tiles").transform;
            
            int rt = Random.Range(0, rootTiles.transform.childCount);
            Transform selectedTile = rootTiles.transform.GetChild(rt);
            mysteryBox.transform.position = new Vector3(selectedTile.position.x, selectedTile.position.y+4, selectedTile.position.z);

            Debug.Log("Spawn " + mysteryBox.name);
        }   
    }
    bool PlayerAndBallFallen(){
        bool fallen = false;
        float minPosY = -50;
        if(ball.transform.position.y < minPosY){
            fallen = true;
        }
        foreach(WitchController w in TeamA.witches){
            if(w.transform.position.y < minPosY) {
                w.witch.character.stunnedDuration.current += 3f;
                fallen = true;
            } 
        }
        foreach(WitchController w in TeamB.witches){
            if(w.transform.position.y < minPosY){
                w.witch.character.stunnedDuration.current += 3f;
                fallen = true;
            }
        }
        return fallen;
    }

    void ShowTitle(string text, float duration, float fontSize = 96){
        // Refresh FX
        _titlePopUp.gameObject.SetActive(true);
        _titlePopUp.duration = duration;
        _titlePopUp.GetComponent<TextMeshProUGUI>().text = text;
        _titlePopUp.GetComponent<TextMeshProUGUI>().fontSize = fontSize;
    }
}
