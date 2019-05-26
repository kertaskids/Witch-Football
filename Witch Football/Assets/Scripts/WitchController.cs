using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : MonoBehaviour
{
    public Character character;
    public Team.TeamParty teamParty;
    public bool _possessingBall;
    public GameObject ball; 
    public GameObject ballPosition; 
    // <Edit later> delete this and change with GetTeamMates(); like in Team.cs
    public GameObject[] teamMates;
    public WitchController[] witchTeamMates;
    private Rigidbody _rigidbody;

    void Start(){
        // <Edit later> Assign Team here
        if(witchTeamMates == null || witchTeamMates.Length < 1) {
            WitchController[] allWitches = GameObject.FindObjectsOfType<WitchController>();
            //Debug.Log("allwitches length " + allWitches.Length);
            List<WitchController> witchesTemp = new List<WitchController>();
            foreach (WitchController w in allWitches) {
                //Debug.Log(w.name + ", " + w.teamParty.ToString());
                if(w.teamParty == this.teamParty && w != this) {
                    witchesTemp.Add(w);
                    Debug.Log("Add " + w.name + " on " + teamParty.ToString() + "as team mate"); 
                }
            }
            witchTeamMates = witchesTemp.ToArray();
        }

        character       = new Character();
        _rigidbody      = GetComponent<Rigidbody>();
        _possessingBall = false;
        ball            = GameObject.Find("Ball");

        // Exception
        character.guard.current     = 0; // <delete later>
        character.lightMagicSkill.magicCasted = false;
        character.heavyMagicSkill.magicCasted = false;
        
        //<Edit later> assign the ball and ball position here
    }
    void Update(){
        MoveControl();
        ActionControl();
        MagicControl();
        GuardControl();
        MysteryBoxControl();
    }

    void MoveControl(){
        float horizontal    = 0;
        float vertical      = 0;
        
        // Direction  
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            transform.localEulerAngles = new Vector3 (0, -90, 0);
            horizontal = -1f;    
        } 
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            transform.localEulerAngles = new Vector3 (0, 90, 0);
            horizontal = 1f;    
        } 
        if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
            transform.localEulerAngles = new Vector3 (0, 0, 0);
            vertical = 1f;    
        }
        if(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){
            transform.localEulerAngles = new Vector3 (0, 180, 0);
            vertical = -1f;    
        } 
        _rigidbody.MovePosition(new Vector3(transform.position.x + character.moveSpeed.current * horizontal * Time.deltaTime,
                                            transform.position.y, 
                                            transform.position.z + character.moveSpeed.current * vertical * Time.deltaTime));

        // Jump
        if(Input.GetKeyDown(KeyCode.Space) && (_rigidbody.velocity.y <= 0.1f) && (character.jumpDelay.full)){
            _rigidbody.AddForce(character.jumpForce.current * Vector3.up, ForceMode.Impulse);
            character.jumpDelay.current = 0f;
        }
        character.jumpDelay.current = UpdateTimer(character.jumpDelay.current, character.jumpDelay.max);

        //Dribble
        if(ball != null){
            if(_possessingBall){
                BallPossessing();
                // Need condition of When get the ball for the first time to avoid redundancy on guard = 3.  
                // Check if the ball is free
                // ball.transform.rotation = Quaternion.identity; 
                // ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
        }
    }

    // <Edit later> Change to static
    float UpdateTimer(float curVal, float maxVal){
        return curVal >= maxVal ? maxVal : (curVal += 1f * Time.deltaTime); 
    }

    void ActionControl(){
        // Offense & Defense
        if(_possessingBall){
            // Shoot
            if(Input.GetKeyDown(KeyCode.Z) && (character.shootDelay.current >= character.shootDelay.max)){
                if(_possessingBall && ball != null){
                    BallReleasing();
                    // Check the rotation and the velocity before adding a force of shoot.
                    ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    ball.transform.rotation = Quaternion.identity;
                    ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * character.shootPower.current, ForceMode.Impulse);
                    //_possessingBall = false;
                    character.shootDelay.current = 0f;
                    Debug.Log("Shoot! Power: " +character.shootPower.current + ", at Euler: " + transform.eulerAngles);
                }
            }
            // Pass
            if(Input.GetKeyDown(KeyCode.X) && (character.passDelay.current >= character.passDelay.max)){
                if(_possessingBall && ball != null){
                    // Find the closest teammate then pass the ball toward it.
                    // Check closest teammate, and assign it.   
                    if(GetClosestTeamMate()!=null){
                        BallReleasing();
                    
                        Transform teamMate = GetClosestTeamMate().transform;
                        // Need to check if it has teamMates. If not, can perform pass, or simply pass forward. 
                        //Transform teamMate = GameObject.FindGameObjectWithTag("TeamMate").transform;
                        Vector3 teamMateDir = teamMate.position - transform.position;
                        // To make Y angle static, delete this.
                        //teamMateDir.y = 0;
                        transform.rotation = Quaternion.LookRotation(teamMateDir);
                        // If the ball is a passing ball, move the ball smoothly towards teammate except it is interrupted. 
                        // Check the rotation and the velocity before adding a force of pass.
                        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        ball.transform.rotation = Quaternion.identity;
                        ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * character.passPower.current, ForceMode.Impulse);
                        
                        //_possessingBall        = false;
                        character.passDelay.current    = 0f;
                        Debug.Log("Pass! Power: " + character.passPower.current + ", at Euler: " + transform.eulerAngles + " To: " + teamMate.name);
                    } else {
                        Debug.Log("You have no friend :'(");
                        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * character.passPower.current, ForceMode.Impulse);
                        
                        _possessingBall        = false;
                        character.passDelay.current    = 0f;
                    }
                }
            }
        }else{
            // Tackle
            if(Input.GetKeyDown(KeyCode.Z) && (character.tackleDelay.current >= character.tackleDelay.max)){
                character.tackleDelay.current = 0f;
                Debug.Log("Tackle");

                // 1. Check the enemy collider, if it is hit by this.collider reduce enemy health & get low manna
                // 2. If it posses
                //      if its guard 0 short stun
                //      if its health 0 long stun 
                // 3. Release the ball & add force OR if player collide with the ball, then possessing    
            }
            // Follow 
            if(Input.GetKey(KeyCode.X) && (character.followDelay.current >= character.followDelay.max)){
                Debug.Log("Follow");
                // Check if there is no team possesing the ball  
                Vector3 ballDir     = ball.transform.position - transform.position;
                // Turn off Y angle before make it as rotation
                ballDir.y           = 0f;
                transform.rotation  = Quaternion.LookRotation(ballDir.normalized); 
                Vector3 tempMovePos = ballDir.normalized * character.moveSpeed.current * Time.deltaTime;
                _rigidbody.MovePosition(transform.position + tempMovePos);
            }
            // Un-Follow
            if(Input.GetKeyUp(KeyCode.X)){
                character.followDelay.current = 0f;
            }
        }
        character.shootDelay.current   = UpdateTimer(character.shootDelay.current, character.shootDelay.max);
        character.passDelay.current    = UpdateTimer(character.passDelay.current, character.passDelay.max);
        character.tackleDelay.current  = UpdateTimer(character.tackleDelay.current, character.tackleDelay.max);
        character.followDelay.current  = UpdateTimer(character.followDelay.current, character.followDelay.max);
    }

    void MagicControl(){
         // Light Magic 
        if(Input.GetKeyDown(KeyCode.C)){    
            // <Edit later> Check the TimeUse first
            if(character.lightMagicSkill.delay.current >= character.lightMagicSkill.delay.max && !character.lightMagicSkill.magicCasted){
                if(character.manna.current >= character.lightMagicSkill.mannaNeed){
                    character.manna.current -= character.lightMagicSkill.mannaNeed;
                    //witchCharacter.lightMagicSkill.Cast();
                    character.CastMagic(character.lightMagicSkill);
                    character.lightMagicSkill.delay.current = 0f;
                    character.lightMagicSkill.magicCasted = true;
                } else {
                    Debug.Log("Not enough Manna");
                }
            }
        }
        // Heavy Magic
        if(Input.GetKeyDown(KeyCode.V)){
            /*if(witchCharacter.manna >= witchCharacter.heavyMagicMannaNeed){
                witchCharacter.manna -= witchCharacter.heavyMagicMannaNeed;
                Debug.Log("Heavy Magic. Damage: " + witchCharacter.heavyMagicDamage + ". Manna: " + witchCharacter.manna);
            }else{
                Debug.Log("Not enough Manna");
            }*/
            // Check the TimeUse first
            if(character.heavyMagicSkill.delay.current >= character.heavyMagicSkill.delay.max && !character.heavyMagicSkill.magicCasted){
                if(character.manna.current >= character.heavyMagicSkill.mannaNeed){
                    character.manna.current -= character.heavyMagicSkill.mannaNeed;
                    //witchCharacter.heavyMagicSkill.Cast();
                    character.CastMagic(character.heavyMagicSkill);
                    character.heavyMagicSkill.delay.current = 0f;
                    character.heavyMagicSkill.magicCasted = true;
                } else {
                    Debug.Log("Not enough Manna");
                }
            }
        }

        // <Update> Back to original coding, duration --; 
        // UpdateTimer duration dan delay dari magicskill!
        // When the delay is over, back to original stats
        character.lightMagicSkill.delay.current = UpdateTimer(character.lightMagicSkill.delay.current, character.lightMagicSkill.delay.max);
        character.heavyMagicSkill.delay.current = UpdateTimer(character.heavyMagicSkill.delay.current, character.heavyMagicSkill.delay.max);
        
        character.lightMagicSkill.duration.current = character.UpdateDurationMagic(character.lightMagicSkill);
        character.heavyMagicSkill.duration.current = character.UpdateDurationMagic(character.heavyMagicSkill);

        // <Edit> Revert when the buff duration is over
        // if duration >= maxDuration
        if(character.lightMagicSkill.duration.current >=  character.lightMagicSkill.duration.max && character.lightMagicSkill.magicCasted){
            character.RevertMagic(character.lightMagicSkill);
            // <Edit> Pindahkan magic casted pada character
            //character.lightMagicSkill.magicCasted = false;
        }
        if(character.heavyMagicSkill.duration.current >=  character.heavyMagicSkill.duration.max && character.heavyMagicSkill.magicCasted){
            character.RevertMagic(character.heavyMagicSkill);
            // <Edit> Pindahkan magic casted pada character
            //character.heavyMagicSkill.magicCasted = false;
        }
    }

    void GuardControl(){
        if(!_possessingBall && (character.guard.available)){
            character.guard.current = 0f;
        }
        character.getTackledDelay.current = UpdateTimer(character.getTackledDelay.current, character.getTackledDelay.max);
        // Handle all the tackle and income damage here
        // if possessing && if(guard && health > 0)
    }

    void Tackled(float guardReduced, float healthReduced) {
        // Need to check first if its possessing
        if(_possessingBall){
            // Guard
            if(character.guard.available){
                character.guard.current -= guardReduced;
            }
            if(character.guard.empty) {
                character.guard.current = 0;
                // Do short stun
                Debug.Log("Short stunned! Guard:" + character.guard.current + ", HP:" +character.healthPoint.current);
                BallReleasing();
            }
            Debug.Log("Tackled when possesses"+_possessingBall);
        } 
        // Move these codes into above block to not let player loss the HP when in guard. 
        // Health Point
        if(character.healthPoint.available){
            character.healthPoint.current -= healthReduced;
        }
        if(character.healthPoint.empty) {
            character.healthPoint.current = 0;
            character.guard.current       = 0;
            // Do long stun
            Debug.Log("Long stunned! Guard:" + character.guard.current + ", HP:" +character.healthPoint.current);
            if(_possessingBall){
                BallReleasing();
                Debug.Log("Ball Released because Long Stun.");
            }
        }
        Debug.Log("Guard: "+character.guard.current +" .HP: "+character.healthPoint.current);
    }

    public void ExplosionDamaged(float guardReduced, float healthReduced){
        Tackled(guardReduced, healthReduced);
    }

    void PlayerIndexControl(int index){
        
    }

    void BallPossessing(){
        // If in previous state ball is free, refresh the guard
        if(ball.GetComponent<Ball>().ballState != Ball.BallState.Possessed){
            character.guard.current    = character.guard.max;
            Debug.Log("Guard:"+character.guard.current);
            ball.GetComponent<Ball>().Possessed(this);
        }
        // <Edit later> Refresh ball velocity and rotation or simply just make the ball as children
        ball.transform.position = ballPosition.transform.position;

        // Change Team State
        Match match = GameObject.FindObjectOfType<Match>();
        if(match != null){
            if(this.teamParty == match.TeamA.teamParty){
                match.TeamA.teamState = Team.TeamState.Offense;
                match.TeamB.teamState = Team.TeamState.Defense;
            } else if (this.teamParty == match.TeamB.teamParty){
                match.TeamA.teamState = Team.TeamState.Defense;
                match.TeamB.teamState = Team.TeamState.Offense;
            }
        }
    }

    void BallReleasing(){
        // Need to check why the ball is releasing
        // character.guard.current = 0f; Already defined in GuardControl();
        _possessingBall = false;
        ball.GetComponent<Ball>().Released(Ball.BallState.Free);

        // <Edit later> Change the team state based on the type of the release, free/ pass/ shot
         Match match = GameObject.FindObjectOfType<Match>();
         if(match != null){
            if(this.teamParty == match.TeamA.teamParty){
                match.TeamA.teamState = Team.TeamState.Defense;
            } else if (this.teamParty == match.TeamB.teamParty){
                match.TeamB.teamState = Team.TeamState.Defense;
            }
        }
    }

    GameObject GetClosestTeamMate(){
        GameObject closestTeamMate = null;
            
        if(teamMates != null && teamMates.Length > 0){
            //Debug.Log("Not Null");
            closestTeamMate = teamMates[0];
            float distance = Mathf.Abs(Vector3.Distance(transform.position, closestTeamMate.transform.position)); 

            foreach (GameObject tm in teamMates){
                float distanceNext = Mathf.Abs(Vector3.Distance(transform.position, tm.transform.position));
                if(distance > distanceNext){
                    distance = distanceNext;
                    closestTeamMate = tm;
                }
            }
        }

        if(closestTeamMate != null) {
            Debug.Log("Closest Team Mate: "+closestTeamMate.name);
        }
        return closestTeamMate;
    }

    void OnCollisionEnter(Collision other) {
        // Tackled
        if(other.gameObject.name == "damageCollider") {
            //Debug.Log("Collide with " + other.gameObject.name);
            //Debug.Log("tackledDelay: "+witchCharacter.tackledDelay + ". maxTackledDelay" + witchCharacter.maxTackledDelay);
            if(character.getTackledDelay.current >= character.getTackledDelay.max){
                Debug.Log("Tackled");
                Tackled(character.tackledDamageToGuard.current, character.tackledDamageToHealth.current);
                character.getTackledDelay.current = 0f;
            }
        }
        // MysteryBox
        if(other.gameObject.name == "MysteryBox") { 
            if(character.usedMysteryBox != null) {
                other.gameObject.GetComponent<MysteryBox>().UseEffect(this);
                Debug.Log("Taking MysteryBox: " + other.gameObject.name);
                Destroy(other.gameObject);
            }
        }

        // <Edit later> Spiky & Explode and need to check the ball possession
        //if(other.gameObject.tag == "Tile") {
            // spiky and exploding damage and addforce
        //}
        
        // <Edit later>
        // Possess the ball when touching it, later it can possessed when the ball is Shot and Passed too. and when the velocity is low. 
        // Ball 
        if(other.gameObject == ball) {
            if(ball.GetComponent<Ball>().ballState == Ball.BallState.Free) {
                // <Edit later> Must be
                _possessingBall = true;
                ball.GetComponent<Ball>().Possessed(this);
                Debug.Log("Possessed by "+gameObject.name);
                // <Edit later> Refresh ball velocity and rotation                 
            }
        }
    }

    void MysteryBoxControl(){
        if(character.usedMysteryBox != null){
            // If already casted
            if(character.usedMysteryBox.duration > 0 && character.usedMysteryBox.casted) {
                character.usedMysteryBox.duration -= Time.deltaTime;
            } else if (character.usedMysteryBox.duration <= 0 && character.usedMysteryBox.casted) {
                // <Edit later> Not needed to make it false before the nullifying
                character.usedMysteryBox.casted = false;
                character.usedMysteryBox = null;
            }
        }
    }
} 