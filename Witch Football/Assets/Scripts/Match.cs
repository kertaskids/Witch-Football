using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
    private float _mysteryBoxDelay;
    private float _stateMaxDelay = 3f;
    private float _mysteryBoxDelayMaxDelay = 5f;
    private bool _initiated;

    void Start(){
        _initiated = false;
        _isPaused   = false;
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
            // Uncomment if we want when the celebration the time is still running. 
            //_stateDelay -= Time.deltaTime;
            if(gamestate == GameState.PostGoal) PostGoal();
        }
    }

    public void PreMatch(){
        if(!_initiated){
            // <Edit later> Add these features
            // Set up for the match: ball position, player position, goal position, player's skill
            Timer = 180f; 
            TeamA = new Team(Team.TeamParty.TeamA);
            TeamB = new Team(Team.TeamParty.TeamB);
            SetupMatch();
            ShowPlayersStats();
            _isPaused   = false;
            _stateDelay = _stateMaxDelay;
            _initiated  = true;
            _mysteryBoxDelay = _mysteryBoxDelayMaxDelay;
        }
        if(_stateDelay <= 0) {
            gamestate   = GameState.MatchPlaying;
            _stateDelay = _stateMaxDelay;
        }
        // Comment this to test the gamestate
        _stateDelay -= Time.deltaTime;
    }
    public void MatchPlaying(){
        Timer -= Time.deltaTime;
        if(Timer <= 0){
            gamestate = GameState.TimeOver;
        }
        SetCharacterControl(true);
        // <Edit later> 
        // Time is limited. Timer is slower when playing a cutscene for casting heavy magic. 
        // Tiles can be manipulated and mutated
        if(_mysteryBoxDelay <= 0){
            SpawnMysteryBox();
            Debug.Log("Spawn Mystery Box");
            _mysteryBoxDelay = _mysteryBoxDelayMaxDelay;
        }
        _mysteryBoxDelay -= Time.deltaTime;

        _oneSecondTimer -= Time.deltaTime;
        if(_oneSecondTimer<=0){
            //Debug.Log("Timer: " + (int)(Timer/60) + "Minutes " + (int)(Timer%60) + "Seconds.");
            _oneSecondTimer = 1f;
        }
    }
    public void Goal(){ 
        // <Edit later>
        // Do this once right after ball trigger the goal line. 
        if(_stateDelay <= 0) {
            gamestate = GameState.PostGoal;
            _isPaused = true;
            _stateDelay = _stateMaxDelay;
        }
        _stateDelay -= Time.deltaTime;

        // Add Celebration 
    }
    public void GoalScored(WitchController scorer){
        // <Edit later> call this function in goal script. 
        if(scorer.teamParty == Team.TeamParty.TeamA) {
            TeamA.Score += 1;
        } else {
            TeamB.Score +=1 ;
        }
        
        List<WitchController> witchesTemp = new List<WitchController>();
        foreach (WitchController w in Scorers){
            witchesTemp.Add(w);
        }
        witchesTemp.Add(scorer);
        Scorers = witchesTemp.ToArray();
        gamestate = GameState.Goal;
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
            _stateDelay = _stateMaxDelay;
            if(TeamA != null && TeamB!=null){
                Debug.Log("Score A: "+TeamA.Score + ". Score B:"+TeamB.Score);
            }

            SetupMatch();
        }
        _stateDelay -= Time.deltaTime;
        Debug.Log("StateDelay:"+_stateDelay);
    }
    public void TimeOver(){
        // The time of match is over
        if(_stateDelay <= 0) {
            gamestate = GameState.PostMatch;
            _stateDelay = _stateMaxDelay;
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
        Debug.Log(TeamA.teamParty.ToString() + " Statistic:");
        Debug.Log(TeamA.witches.Length);
        foreach (WitchController w in TeamA.witches)
        {   
            // <Edit later> If the script is disabled, it is null reference somehow
            // <EDIT LATER> Null reference at the character, because the character is not a monobehaviour script
            Debug.Log(w.character == null);
            
            if(w.enabled){
                //Character Stat
                Debug.Log(w.name + ", HP: " + w.character.healthPoint.current + 
                            ", DamageToHP: " + w.character.tackledDamageToHealth.current + 
                            ", PassPower" + w.character.passPower.current + 
                            ", Speed: " + w.character.moveSpeed.current);
                //MagicSkill
                Debug.Log("LightSkill: " + w.character.lightMagicSkill.name + 
                            ", HeavySkill: " + w.character.heavyMagicSkill.name);
            }
        }
        Debug.Log(TeamB.teamParty.ToString() + " Statistic:");
        foreach (WitchController w in TeamB.witches)
        {
            if(w.enabled){
                //Character Stat
                Debug.Log(w.name + ", HP: " + w.character.healthPoint.current + 
                            ", DamageToHP: " + w.character.tackledDamageToHealth.current + 
                            ", PassPower" + w.character.passPower.current + 
                            ", Speed: " + w.character.moveSpeed.current);
                //MagicSkill
                Debug.Log("LightSkill: " + w.character.lightMagicSkill.name + 
                            ", HeavySkill: " + w.character.heavyMagicSkill.name);
            }
        }
    }
    void SetupMatch(){
        // Ball Position
        GameObject ball = GameObject.Find("Ball");
        // <Edit later> Starting ball position
        ball.transform.position = new Vector3(4f, 2f, 1f);
        ball.transform.rotation = Quaternion.identity; 
        // Witches Position
        for(int i = 0; i < TeamA.witches.Length; i++){
            TeamA.witches[i].transform.position = new Vector3(ball.transform.position.x - (i+1), 
                                                                ball.transform.position.y, 
                                                                ball.transform.position.z);
            TeamB.witches[i].transform.position = new Vector3(ball.transform.position.x + (i+1), 
                                                                ball.transform.position.y, 
                                                                ball.transform.position.z);                                                        
        }
    }
    void SetCharacterControl(bool enable){
        foreach (WitchController w in TeamA.witches)
        {
            w.enabled = enable;
        }
        foreach (WitchController w in TeamB.witches)
        {
            w.enabled = enable;
        }
        // <Edit later> Better using boolean in the update function to allow player control. 
    }
    void SpawnMysteryBox(){
        // Prepare the mysterybox objects
        // select mysterybox randomly
        // prepare the tiles then select one randomly
        // instantiate based on tile's position

        GameObject mysteryBox = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mysteryBox.AddComponent<Rigidbody>();
        //<Edit later> Add the physic material
        mysteryBox.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        
        Transform rootTiles = GameObject.Find("Tiles").transform;
        int r = Random.Range(0, rootTiles.transform.childCount-1);
        Transform selectedTile = rootTiles.transform.GetChild(r);
        mysteryBox.transform.position = new Vector3(selectedTile.position.x, selectedTile.position.y+4, selectedTile.position.z);
    }

}
