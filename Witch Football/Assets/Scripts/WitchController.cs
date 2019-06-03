using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : MonoBehaviour
{
    public Character character;
    public Team.TeamParty teamParty;
    public bool _possessingBall;
    public GameObject ball; 
    public GameObject ballPosition; // <Edit later> get ball and its position by tag and transform at startup 
    public WitchController[] teamMatesWitches;
    private Rigidbody _rigidbody;
    // <Edit later> The WitchState
    public bool _isTackling = false;
    // Edit later
    public PlayerInput.ID playerID;
    private PlayerInput playerInput;

    void Start(){
        Init();
        playerInput = PlayerInput.GetPlayer((int)playerID);
    }

    void Init(){
        character       = new Character();
        _rigidbody      = GetComponent<Rigidbody>();
        //teamParty
        _possessingBall = false;
        ball            = GameObject.FindGameObjectWithTag("Ball");
        //ballposition

        if(teamMatesWitches == null || teamMatesWitches.Length < 1) {
            GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
            List<WitchController> witchesTemp = new List<WitchController>();
            foreach (GameObject w in allWitches) {
                WitchController witchController = w.GetComponent<WitchController>();
                if(witchController.teamParty == this.teamParty && witchController != this) {
                    witchesTemp.Add(witchController);
                    //Debug.Log("Add " + w.name + " on " + teamParty.ToString() + "as team mate of " + gameObject.name); 
                }
            }
            teamMatesWitches = witchesTemp.ToArray();
        }
        
        //<Edit later> assign the ball and ball position here locally
    }

    void Update(){
        // If not ispaused && not stunned
        if(character.stunnedDuration.current <= 0) {
            MoveControl();
            ActionControl();
            MagicControl();
            GuardControl();
            MysteryBoxControl();
        } else {
            character.stunnedDuration.current = UpdateDuration(character.stunnedDuration.current);
        }

        /* -------FOR TESTING ONLY ---------*/
        if(Input.GetButtonDown(playerInput.StartOrPause)){
            Debug.Log("Start1 is pressed");
        }
        if(Input.GetButton(playerInput.HorizontalMove)){
            Debug.Log("HMOVE");
        }
        if(Input.GetButton(playerInput.VerticalMove)){
            Debug.Log("VMOve");
        }
        if(Input.GetButtonDown(playerInput.PassOrFollow)){
            Debug.Log("pass");
        }
        if(Input.GetButtonDown(playerInput.ShootOrTackle)){
            //Debug.Log("tackle");
        }
        if(Input.GetButtonDown(playerInput.LightMagic)){
            Debug.Log("Light");
        }
        if(Input.GetButtonDown(playerInput.HeavyMagic)){
            Debug.Log("Heavy");
        }
        if(Input.GetButtonDown(playerInput.Jump)){
            //Debug.Log("Jump");
            Debug.Log(playerInput.Jump);
        }
        /* -------FOR TESTING ONLY ---------*/
    }

    void MoveControl(){
        float horizontal    = 0;
        float vertical      = 0;
        
        /*----------------FOR TESTING ONLY--------------------*/
        /* if(Input.GetKey(KeyCode.A)){
            transform.localEulerAngles = new Vector3 (0, -90, 0);
            horizontal = -1f;    
        } 
        if(Input.GetKey(KeyCode.D)){
            transform.localEulerAngles = new Vector3 (0, 90, 0);
            horizontal = 1f;    
        } 
        if(Input.GetKey(KeyCode.W)){
            transform.localEulerAngles = new Vector3 (0, 0, 0);
            vertical = 1f;    
        }
        if(Input.GetKey(KeyCode.S)){
            transform.localEulerAngles = new Vector3 (0, 180, 0);
            vertical = -1f;    
        }*/
        /*---------------TESTING ONLY --------------- */

        // Direction  
        if(Input.GetAxis(playerInput.HorizontalMove) < -0.2){
            transform.localEulerAngles = new Vector3 (0, -90, 0);
            horizontal = -1f;    
        } 
        if(Input.GetAxis(playerInput.HorizontalMove) > 0.2){
            transform.localEulerAngles = new Vector3 (0, 90, 0);
            horizontal = 1f;    
        } 
        if(Input.GetAxis(playerInput.VerticalMove) > 0.2){
            transform.localEulerAngles = new Vector3 (0, 0, 0);
            vertical = 1f;    
        }
        if(Input.GetAxis(playerInput.VerticalMove) < -0.2){
            transform.localEulerAngles = new Vector3 (0, 180, 0);
            vertical = -1f;    
        } 
        //Debug.Log("Axis X, Y: " + Input.GetAxis(playerInput.HorizontalMove) + ", " + Input.GetAxis(playerInput.VerticalMove));
        //Debug.Log("H, V: " + horizontal + ", " + vertical);
        _rigidbody.MovePosition(new Vector3(transform.position.x + character.moveSpeed.current * horizontal * Time.deltaTime,
                                            transform.position.y, 
                                            transform.position.z + character.moveSpeed.current * vertical * Time.deltaTime));

        // Jump
        if(Input.GetButtonDown(playerInput.Jump) && (_rigidbody.velocity.y <= 0.1f) && (character.jumpDelay.full)){
            _rigidbody.AddForce(character.jumpForce.current * Vector3.up, ForceMode.Impulse);
            character.jumpDelay.current = 0f;
        }
        character.jumpDelay.current = UpdateTimer(character.jumpDelay.current, character.jumpDelay.max);

        //Dribble
        if(ball != null){
            if(_possessingBall){
                // <Edit later> Refresh ball velocity and rotation or simply just make the ball as children 
                // <Edit this> in BallFollowing() 
                ball.transform.position = ballPosition.transform.position;

            }
        }
    }

    // <Edit later> Change to static
    float UpdateTimer(float curVal, float maxVal){
        return curVal >= maxVal ? maxVal : (curVal += 1f * Time.deltaTime); 
    }
    float UpdateDuration(float curVal){
        return curVal <= 0 ? 0 : (curVal -= 1f * Time.deltaTime);
    }

    void ActionControl(){
        // Offense & Defense
        if(_possessingBall){
            // Shoot
            if(Input.GetButtonDown(playerInput.ShootOrTackle) && (character.shootDelay.current >= character.shootDelay.max)){
                if(_possessingBall && ball != null){
                    BallReleasing();
                    // Check the rotation and the velocity before adding a force of shoot.
                    ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 3, 0);
                    ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    ball.transform.rotation = Quaternion.identity;
                    ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * character.shootPower.current, ForceMode.Impulse);
                    //_possessingBall = false;
                    character.shootDelay.current = 0f;
                    Debug.Log("Shoot! Power: " +character.shootPower.current + ", at Euler: " + transform.eulerAngles);
                }
            }
            // Pass
            if(Input.GetButtonDown(playerInput.PassOrFollow) && (character.passDelay.current >= character.passDelay.max)){
                if(_possessingBall && ball != null){
                    // Find the closest teammate then pass the ball toward it.
                    // Check closest teammate, and assign it.   
                    if(GetClosestTeamMate()!=null){
                        BallReleasing();
                        // Needto check, if the closest team mates if not active, just pass forward. 
                        Transform teamMate = GetClosestTeamMate().transform;
                        Vector3 teamMateDir = teamMate.position - transform.position;
                        // To make Y angle static, delete this.
                        //teamMateDir.y = 0;
                        transform.rotation = Quaternion.LookRotation(teamMateDir);
                        // If the ball is a passing ball, move the ball smoothly towards teammate except it is interrupted. 
                        // Check the rotation and the velocity before adding a force of pass.
                        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        ball.transform.rotation = Quaternion.identity;
                        ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * character.passPower.current, ForceMode.Impulse);
                        
                        //_possessingBall        = false;
                        character.passDelay.current    = 0f;
                        Debug.Log("Pass! Power: " + character.passPower.current + ", at Euler: " + transform.eulerAngles + " To: " + teamMate.name);
                    } else {
                        Debug.Log("You have no friend :'(");
                        BallReleasing();
                        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * character.passPower.current, ForceMode.Impulse);
                        
                        //_possessingBall        = false;
                        character.passDelay.current    = 0f;
                    }
                }
            }
        }else{
            // Tackle
            if(Input.GetButtonDown(playerInput.ShootOrTackle) && (character.tackleDelay.current >= character.tackleDelay.max)){
                character.tackleDelay.current = 0f;
                _isTackling = true;
                Debug.Log("Tackling");
                // 1. Check the enemy collider, if it is hit by this.collider reduce enemy health & get low manna
                // 2. If it posses
                //      if its guard 0 short stun
                //      if its health 0 long stun 
                // 3. Release the ball & add force OR if player collide with the ball, then possessing    
            } else {
                _isTackling = false;
            }
            // Follow 
            if(Input.GetButtonDown(playerInput.PassOrFollow) && (character.followDelay.current >= character.followDelay.max)){
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
            if(Input.GetButtonUp(playerInput.PassOrFollow)){
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
        if(Input.GetButtonDown(playerInput.LightMagic)){    
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
        if(Input.GetButtonDown(playerInput.LightMagic)){
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
    // <Edit Later> tackled(guard, hp, explosiontype, etc)
    void Tackled(float guardReduced, float healthReduced) {
        //Debug.Log("GetTackledDelayRemain: "+character.getTackledDelay.current);
        // Need to check first if its Still in tackled duration (invulnerable).
        // Code below is currently also implemented on tile.cs
        //if(character.getTackledDelay.current < character.getTackledDelay.max){ // or make this as condition too, in checking guard.available and healthpoint available.
        //    return;
        //}
        // Need to check first if its possessing
        if(_possessingBall){
            // Guard
            if(character.guard.available && (character.getTackledDelay.current >= character.getTackledDelay.max)){
                character.guard.current -= guardReduced;
            }
            if(character.guard.empty) {
                character.guard.current = 0;
                // Do short stun
                character.stunnedDuration.current = 5f;
                Debug.Log("Short stunned! Guard:" + character.guard.current + ", HP:" +character.healthPoint.current);
                BallReleasing();
            }
            Debug.Log("Tackled when possesses"+_possessingBall);
        } 
        // Move these codes into above block to not let player loss the HP when in guard. 
        // Health Point
        //Debug.Log(character.healthPoint.available + " " + (character.getTackledDelay.current >= character.getTackledDelay.max));
        if(character.healthPoint.available && (character.getTackledDelay.current >= character.getTackledDelay.max)){
            character.healthPoint.current -= healthReduced;
        }
        //Debug.Log("GetTackledDelay: "+character.getTackledDelay.current);
        if(character.healthPoint.empty) {
            character.healthPoint.current = 0;
            character.guard.current       = 0;
            // Do long stun
            character.stunnedDuration.current = 10f;
            Debug.Log("Long stunned! Guard:" + character.guard.current + ", HP:" +character.healthPoint.current);
            if(_possessingBall){
                BallReleasing();
                Debug.Log("Ball Released because Long Stun.");
            }
        }
        Debug.Log("Guard: "+character.guard.current +" .HP: "+character.healthPoint.current);
        
        character.getTackledDelay.current = 0f;
        Debug.Log("GetTackledDelay: "+character.getTackledDelay.current);
    }

    public void Damaged(float guardReduced, float healthReduced){
        Tackled(guardReduced, healthReduced);
    }

    void PlayerIndexControl(int index){
        
    }

    void BallPossessing(){
        // Need condition of When get the ball for the first time to avoid redundancy on guard = 3.  
        // Check if the ball is free
        // ball.transform.rotation = Quaternion.identity; 
        // ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        // If in previous state ball is free, refresh the guard
        if(ball.GetComponent<Ball>().ballState != Ball.BallState.Possessed){// <Edit later> Check in oncollisionenter
            _possessingBall = true;
            character.guard.current    = character.guard.max;
            Debug.Log("Guard:"+character.guard.current);
            ball.GetComponent<Ball>().Possessed(this);
        }
        // <Edit later> Refresh ball velocity and rotation or simply just make the ball as children 
        // <Edit this> in BallFollowing() 
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
    
    public void BallReleasing(bool addForce = false){
        // Need to check why the ball is releasing
        // character.guard.current = 0f; Already defined in GuardControl();
        _possessingBall = false;
        Ball b =  ball.GetComponent<Ball>();
        b.Released(Ball.BallState.Free);
        // Release the ball forward and little bit up
        Vector3 targetDir = transform.forward; //-ball.transform.forward;
        targetDir.y += 0.5f;
        if(!addForce) {
            targetDir.x = 0f;
        }
        ball.GetComponent<Rigidbody>().velocity = new Vector3(targetDir.x, 4, 0);  
        ball.GetComponent<Rigidbody>().AddForce(targetDir, ForceMode.Impulse);
        Debug.Log("ball direction:"+targetDir);

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

        if(teamMatesWitches != null && teamMatesWitches.Length > 0){
            closestTeamMate = teamMatesWitches[0].gameObject;
            float distance  = Vector3.Distance(transform.position, closestTeamMate.transform.position); 
            // <Edit later> Check first if TeamMates > 1
            foreach (WitchController tmw in teamMatesWitches){
               float distanceNext = Vector3.Distance(transform.position, tmw.transform.position);
               if(distance > distanceNext) {
                   distance = distanceNext;
                   closestTeamMate = tmw.gameObject;
               } 
            }
        }
        if(closestTeamMate != null) {
            Debug.Log("Closest Team Mate: "+closestTeamMate.name);
        }
        return closestTeamMate;
    }

    void OnCollisionStay(Collision other) {
        if(other.gameObject.tag == "Witch" && other.gameObject.GetComponent<WitchController>()._isTackling){
        //if(other.gameObject.name == "damageCollider") {
            Debug.Log("Collide with " + other.gameObject.name);
            //Debug.Log("tackledDelay: "+witchCharacter.tackledDelay + ". maxTackledDelay" + witchCharacter.maxTackledDelay);
            if(character.getTackledDelay.current >= character.getTackledDelay.max){
                Debug.Log(gameObject.name + " tackled by" + other.gameObject.name);
                Tackled(character.tackledDamageToGuard.current, character.tackledDamageToHealth.current);
                character.getTackledDelay.current = 0f;
                // <Edit later> Attacker get a manna. 
                // <Edit later> AddForce to the damaged witch
            }
        }   
    }
    void OnCollisionEnter(Collision other) {
         // <Edit later> Collide with witch should be in OnCollisionStay. Or, make temp bool to store elapsed tackle
         if(other.gameObject.tag == "Witch"){
             Debug.Log("Collide with " + other.gameObject.name);
         }
        // Tackled 
        if(other.gameObject.tag == "Witch" && other.gameObject.GetComponent<WitchController>()._isTackling){
        //if(other.gameObject.name == "damageCollider") {
            Debug.Log("Collide with " + other.gameObject.name);
            //Debug.Log("tackledDelay: "+witchCharacter.tackledDelay + ". maxTackledDelay" + witchCharacter.maxTackledDelay);
            if(character.getTackledDelay.current >= character.getTackledDelay.max){
                Debug.Log(gameObject.name + " tackled by" + other.gameObject.name);
                Tackled(character.tackledDamageToGuard.current, character.tackledDamageToHealth.current);
                character.getTackledDelay.current = 0f;
                // <Edit later> Attacker get a manna. 
                // <Edit later> AddForce to the damaged witch
            }
        }
        // MysteryBox
        if(other.gameObject.tag == "MysteryBox") { 
            if(character.usedMysteryBox == null) {
                other.gameObject.GetComponent<MysteryBox>().UseEffect(this);
                Debug.Log("Taking MysteryBox: " + other.gameObject.name);
            }
        }

        if(other.gameObject.tag == "Rock"){
            if(character.getTackledDelay.current >= character.getTackledDelay.max){
                // <Edit later> Need to change the force when the ball is released
                Rock rock = other.gameObject.GetComponent<Rock>();
                Debug.Log(gameObject.name + " damaged by" + rock.gameObject.name);
                Damaged(rock.damageGuard, rock.damageHealth);
                
                if(_possessingBall){
                    BallReleasing();
                    GameObject.Find("Ball").GetComponent<Rigidbody>().position = new Vector3(transform.position.x,
                                                                                         transform.position.y + 0.5f, transform.position.z);
                    //Debug.Break();
                    /* 
                    ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 3, 0);
                    ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    ball.transform.rotation = Quaternion.identity; //<Need to change apparently>
                    ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * character.shootPower.current, ForceMode.Impulse);
                    */
                }
                transform.rotation = Quaternion.LookRotation(rock.transform.position - transform.position);
                //gameObject.GetComponent<Rigidbody>().AddExplosionForce(5f, rock.transform.position, 2f, 2f, ForceMode.Impulse);
                //Debug.Log("Collide with:" + gameObject.name + " Direction:" + gameObject.transform.rotation);

                character.getTackledDelay.current = 0f;
            }
        }

        // <Edit later> Spiky & Explode & Rock and need to check the ball possession
        //if(other.gameObject.tag == "Tile") { && if typeperformed
            
            // spiky and exploding damage and addforce
        //}
        
        
        // <Edit later>
        // Possess the ball when touching it, later it can possessed when the ball is Shot and Passed too. and when the velocity is low. 
        // Ball 
        if(other.gameObject == ball && character.stunnedDuration.current <= 0) {
            if(ball.GetComponent<Ball>().ballState == Ball.BallState.Free) {
                // <Edit later> Must be Ballpossessing()
                BallPossessing();
                ball.GetComponent<Ball>().Possessed(this);
                Debug.Log("Possessed by "+gameObject.name);
                // <Edit later> Refresh ball velocity and rotation                 
            }
        }
    }

    void MysteryBoxControl(){
        if(character.usedMysteryBox != null){
            //Debug.Log(character.usedMysteryBox.duration);
            // If already casted
            if(character.usedMysteryBox.duration > 0 && character.usedMysteryBox.casted) {
                character.usedMysteryBox.duration -= Time.deltaTime;
            } else if (character.usedMysteryBox.duration <= 0 && character.usedMysteryBox.casted) {
                
                character.RevertMysteryBox(character.usedMysteryBox);
            }
            
        }
    }
} 