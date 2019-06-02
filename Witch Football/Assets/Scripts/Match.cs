using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    void Start(){
        _initiated = false;
        _isPaused  = false;
        gamestate  = GameState.PreMatch;
        _initialBallPos = new Vector3(4.5f, 2, 2);
        _mysteryBoxDelay = mysteryBoxMaxDelay;

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

    public void PreMatch(){
        if(!_initiated){
            // <Edit later> Initialize Timer, Team, Ball position, Player's skill
            Timer = 180f; 
            TeamA = new Team(Team.TeamParty.TeamA);
            TeamB = new Team(Team.TeamParty.TeamB);
            // <Edit later> Need to check with OnEnabled
            SetCharacterControl(true); 
            SetupMatch();
            ShowPlayersStats();
            _isPaused   = false;
            _stateDelay = stateMaxDelay;
            _initiated  = true;
            _mysteryBoxDelay = mysteryBoxMaxDelay;
        }

        if(_stateDelay <= 0) {
            gamestate   = GameState.MatchPlaying;
            _stateDelay = stateMaxDelay;
        }
        _stateDelay -= Time.deltaTime;
    }
    public void MatchPlaying(){
        Timer -= Time.deltaTime;
        if(Timer <= 0){
            gamestate = GameState.TimeOver;
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
            //Debug.Log("Timer: " + (int)(Timer/60) + "Minutes " + (int)(Timer%60) + "Seconds.");
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
            if(TeamA != null && TeamB!=null){
                Debug.Log("Score A: "+TeamA.Score + ". Score B:"+TeamB.Score);
            }
            //<Edit later> already implemented on SetupMatch
            GameObject ball = GameObject.Find("Ball");
            ball.transform.position = _initialBallPos;
            ball.transform.rotation = Quaternion.identity; 
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            // Make the possessing player nullifies the ball
            GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
            foreach (GameObject w in allWitches)
            {
                if(w.GetComponent<WitchController>()._possessingBall) {
                    w.GetComponent<WitchController>().BallReleasing();
                }
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
            _stateDelay = stateMaxDelay;
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
                Debug.Log(w.name + ", HP: " + w.character.healthPoint.current + 
                            ", DamageToHP: " + w.character.tackledDamageToHealth.current + 
                            ", PassPower" + w.character.passPower.current + 
                            ", Speed: " + w.character.moveSpeed.current);
                //MagicSkill
                //Debug.Log("LightSkill: " + w.character.lightMagicSkill.name + 
                            //", HeavySkill: " + w.character.heavyMagicSkill.name);
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
                //Debug.Log("LightSkill: " + w.character.lightMagicSkill.name + 
                            //", HeavySkill: " + w.character.heavyMagicSkill.name);
            }
        }
    }
    void SetupMatch(){
        // Ball Position
        GameObject ball = GameObject.Find("Ball");
        ball.transform.position = _initialBallPos;
        ball.transform.rotation = Quaternion.identity; 
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        // Witches Position
        for(int i = 0; i < TeamA.witches.Length; i++){
            TeamA.witches[i].transform.position = new Vector3(ball.transform.position.x - (i+1), 
                                                                ball.transform.position.y, 
                                                                ball.transform.position.z);                                                        
        }
        for(int i = 0; i < TeamB.witches.Length; i++){
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
        if(mysteryBoxes != null || mysteryBoxes.Length >= 0){
            int r = Random.Range(0, mysteryBoxes.Length-1);
            GameObject mysteryBox = GameObject.Instantiate(mysteryBoxes[r]);
            Transform rootTiles = GameObject.Find("Tiles").transform;
            
            int rt = Random.Range(0, rootTiles.transform.childCount-1);
            Transform selectedTile = rootTiles.transform.GetChild(rt);
            mysteryBox.transform.position = new Vector3(selectedTile.position.x, selectedTile.position.y+4, selectedTile.position.z);

            Debug.Log("Spawn " + mysteryBox.name);
        }   
    }
}
