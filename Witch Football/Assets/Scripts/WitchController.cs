using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.ID playerID;
    public Witch.WitchClass baseClass;
    public Witch witch;
    public WitchController[] teamMatesWitches;
    public Team.TeamParty teamParty;
    private Rigidbody _rigidbody;
    public bool _possessingBall;
    public bool _isTackling; 
    public GameObject ball; 
    public GameObject ballPosition;
    
    void Start(){
        playerInput = PlayerInput.GetPlayer((int)playerID);
        Init();
    }

    void Init(){
        witch           = InitClass(baseClass); 
        _rigidbody      = GetComponent<Rigidbody>();
        _possessingBall = false;
        _isTackling     = false;
        ball            = GameObject.FindGameObjectWithTag("Ball");
        ballPosition    = transform.Find("BallPosition").gameObject; 

        if(teamMatesWitches == null || teamMatesWitches.Length < 1) {
            GameObject[] allWitches = GameObject.FindGameObjectsWithTag("Witch");
            List<WitchController> witchesTemp = new List<WitchController>();
            foreach (GameObject w in allWitches) {
                WitchController witchController = w.GetComponent<WitchController>();
                if(witchController.teamParty == this.teamParty && witchController != this) {
                    witchesTemp.Add(witchController);
                    Debug.Log("Add " + w.name + " on " + teamParty.ToString() + "as team mate of " + gameObject.name); 
                }
            }
            teamMatesWitches = witchesTemp.ToArray();
        }
    }
    Witch InitClass(Witch.WitchClass baseClass){
        this.baseClass = baseClass;
        if(baseClass == Witch.WitchClass.Sorcerer){
            return WitchBase.Sorcerer;
        }else if(baseClass == Witch.WitchClass.Cleric){
            return WitchBase.Cleric;
        }else if(baseClass == Witch.WitchClass.Wizard){
            return WitchBase.Wizard;
        }else if(baseClass == Witch.WitchClass.Druid){
            return WitchBase.Druid;
        }
        // Class Base
        return WitchBase.Base;
        // Magic skills are included here
    }

    void Update(){
        // If not ispaused && not stunned
        if(witch.character.stunnedDuration.current <= 0) {
            MoveControl();
            ActionControl();
            MagicControl();
            GuardControl();
            MysteryBoxControl();
        } else {
            witch.character.stunnedDuration.current = UpdateDuration(witch.character.stunnedDuration.current);
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
        /* if(Input.GetAxis(playerInput.HorizontalMove) < -0.2){
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
        } */
        horizontal = Input.GetAxis(playerInput.HorizontalMove);
        vertical = Input.GetAxis(playerInput.VerticalMove);
        float angle;
        angle = Mathf.Atan2(horizontal, vertical);
        angle = Mathf.Rad2Deg * angle;
        //angle += Camera.main.transform.eulerAngles.y;
        Quaternion targetdir = Quaternion.Euler(0, angle, 0);
        if(horizontal > -0.2 && horizontal < 0.2 && vertical < 0.2 && vertical > -0.2){
            //Idle
        } else {
            transform.localRotation = Quaternion.Slerp(transform.rotation, targetdir, 10*Time.deltaTime);
        }
        
        /* if(playerID == PlayerInput.ID.Player2){
            Debug.Log("Axis X, Y: " + Input.GetAxis(playerInput.HorizontalMove) + ", " + Input.GetAxis(playerInput.VerticalMove));
        }*/
        //Debug.Log("H, V: " + horizontal + ", " + vertical);
        _rigidbody.MovePosition(new Vector3(transform.position.x + witch.character.moveSpeed.current * horizontal * Time.deltaTime,
                                            transform.position.y, 
                                            transform.position.z + witch.character.moveSpeed.current * vertical * Time.deltaTime));

        // Jump
        if(Input.GetButtonDown(playerInput.Jump) && (_rigidbody.velocity.y <= 0.1f) && (witch.character.jumpDelay.full)){
            _rigidbody.AddForce(witch.character.jumpForce.current * Vector3.up, ForceMode.Impulse);
            witch.character.jumpDelay.current = 0f;
        }
        witch.character.jumpDelay.current = UpdateTimer(witch.character.jumpDelay.current, witch.character.jumpDelay.max);

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
            if(Input.GetButtonDown(playerInput.ShootOrTackle) && (witch.character.shootDelay.current >= witch.character.shootDelay.max)){
                if(_possessingBall && ball != null){
                    //Vector3 releasePos = 
                    //new Vector3(ball.transform.position.x, ball.transform.position.y, ball.transform.position.z + 0.25f);
                    //ball.transform.Translate(ball.transform.forward * 0.25f);
                    /*Debug.Log(ball.transform.position);
                    Debug.Log(ball.GetComponent<Rigidbody>().position);
                    ball.transform.position += transform.forward * 0.25f;
                    ball.GetComponent<Rigidbody>().position += transform.forward * 0.25f; 
                    Debug.Log(ball.transform.position);
                    Debug.Log(ball.GetComponent<Rigidbody>().position);
                    BallReleasing();
                    Debug.Log(ball.transform.position);
                    Debug.Log(ball.GetComponent<Rigidbody>().position);
                    ball.transform.rotation = Quaternion.identity;
                    ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * witch.character.shootPower.current, ForceMode.Impulse);
                    
                    witch.character.shootDelay.current = 0f;
                    Debug.Break();*/
                    
                    //ball.GetComponent<Rigidbody>().
                    //BallReleasing(new Vector3)
                     ball.transform.localPosition += new Vector3(0f, 0f, 0.3f); 
                     //ball.transform.position += ball.transform.forward * 0.25f;
                     Debug.Log(ball.transform.forward);
                     BallReleasing(ball.transform.position, ball.transform.localRotation, ball.transform.forward, new Vector3(0,0,0), Vector3.zero, 2 * transform.forward * witch.character.shootPower.current);
                    //Debug.Break();
                    //BallReleasing();
                    //ball.transform.position = ballPosition1.transform.position;
                    //Debug.Break();
                    // Check the rotation and the velocity before adding a force of shoot.
                    //ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                   // ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 3, 0);
                    //ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                   // ball.transform.rotation = Quaternion.identity;
                   // ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * witch.character.shootPower.current, ForceMode.Impulse);
                    //_possessingBall = false;
                    witch.character.shootDelay.current = 0f;
                    Debug.Log("Shoot! Power: " + witch.character.shootPower.current + ", at Euler: " + transform.eulerAngles);
                    
                }
            }
            // Pass
            if(Input.GetButtonDown(playerInput.PassOrFollow) && (witch.character.passDelay.current >= witch.character.passDelay.max)){
                if(_possessingBall && ball != null){
                    // Find the closest teammate then pass the ball toward it if any.  
                    if(GetClosestTeamMate()!=null){
                         // Needto check, if the closest team mates if not active, just pass forward. 
                        Transform teamMate = GetClosestTeamMate().transform;
                        Vector3 teamMateDir = teamMate.position - transform.position;
                        // To make Y angle static, delete this.
                        //teamMateDir.y = 0;
                        transform.rotation = Quaternion.LookRotation(teamMateDir);
                        
                        /* ball.transform.localPosition += new Vector3(0f, 0f, 0.3f);
                        BallReleasing();
                        //ball.transform.position = ballPosition1.transform.position;
                        
                        //Debug.Break();
                       
                        // If the ball is a passing ball, move the ball smoothly towards teammate except it is interrupted. 
                        // Check the rotation and the velocity before adding a force of pass.
                        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                        ball.transform.rotation = Quaternion.identity;
                        ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * witch.character.passPower.current, ForceMode.Impulse);
                        */
                        Debug.Log("Pass! Power: " + witch.character.passPower.current + ", at Euler: " + transform.eulerAngles + " To: " + teamMate.name);
                    } else {
                        Debug.Log("You have no friend :'(");
                        /* BallReleasing();
                        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * witch.character.passPower.current, ForceMode.Impulse);
                        */
                    }
                    //_possessingBall        = false;
                    //Vector3 startPos = new Vector3(ball.transform.position.x, ball.transform.position.y + 1.5f, ball.transform.position.z); 
                    //Vector3 vel = ball.transform.forward;
                    //vel += new Vector3(0,2,0);
                    //BallReleasing(startPos, ball.transform.localRotation, ball.transform.forward, vel, Vector3.zero, 1 * transform.up * witch.character.shootPower.current);
                    
                    ball.transform.localPosition += new Vector3(0f, 0f, 0.3f); 
                    BallReleasing(ball.transform.position, ball.transform.localRotation, ball.transform.forward, new Vector3(0,0,0), Vector3.zero, 2 * transform.forward * witch.character.passPower.current);
                    

                    witch.character.passDelay.current    = 0f;
                }
            }
        }else{
            // Tackle
            if(Input.GetButtonDown(playerInput.ShootOrTackle) && (witch.character.tackleDelay.current >= witch.character.tackleDelay.max)){
                witch.character.tackleDelay.current = 0f;
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
            if(Input.GetButton(playerInput.PassOrFollow) && (witch.character.followDelay.current >= witch.character.followDelay.max)){
                Debug.Log("Follow");
                // Check if there is no team possesing the ball  
                Vector3 ballDir     = ball.transform.position - transform.position;
                // Turn off Y angle before make it as rotation
                ballDir.y           = 0f;
                transform.rotation  = Quaternion.LookRotation(ballDir.normalized); 
                Vector3 tempMovePos = ballDir.normalized * witch.character.moveSpeed.current * Time.deltaTime;
                _rigidbody.MovePosition(transform.position + tempMovePos);
            }
            // Un-Follow
            if(Input.GetButtonUp(playerInput.PassOrFollow)){
                witch.character.followDelay.current = 0f;
            }
        }
        witch.character.shootDelay.current   = UpdateTimer(witch.character.shootDelay.current, witch.character.shootDelay.max);
        witch.character.passDelay.current    = UpdateTimer(witch.character.passDelay.current, witch.character.passDelay.max);
        witch.character.tackleDelay.current  = UpdateTimer(witch.character.tackleDelay.current, witch.character.tackleDelay.max);
        witch.character.followDelay.current  = UpdateTimer(witch.character.followDelay.current, witch.character.followDelay.max);
    }

    void MagicControl(){
         // Light Magic 
        if(Input.GetButtonDown(playerInput.LightMagic)){    
            // <Edit later> Check the TimeUse first
            if(witch.character.lightMagicSkill.delay.current >= witch.character.lightMagicSkill.delay.max && !witch.character.lightMagicSkill.casted){
                if(witch.character.manna.current >= witch.character.lightMagicSkill.mannaNeed){
                    witch.character.manna.current -= witch.character.lightMagicSkill.mannaNeed;
                    //witchCharacter.lightMagicSkill.Cast();
                    witch.character.CastMagic(witch.character.lightMagicSkill);
                    witch.character.lightMagicSkill.delay.current = 0f;
                    witch.character.lightMagicSkill.casted = true;
                } else {
                    Debug.Log("Not enough Manna");
                }
            }
        }
        // Heavy Magic
        if(Input.GetButtonDown(playerInput.HeavyMagic)){
            // Check the TimeUse first
            if(witch.character.heavyMagicSkill.delay.current >= witch.character.heavyMagicSkill.delay.max && !witch.character.heavyMagicSkill.casted){
                if(witch.character.manna.current >= witch.character.heavyMagicSkill.mannaNeed){
                    witch.character.manna.current -= witch.character.heavyMagicSkill.mannaNeed;
                    //witchCharacter.heavyMagicSkill.Cast();
                    witch.character.CastMagic(witch.character.heavyMagicSkill);
                    witch.character.heavyMagicSkill.delay.current = 0f;
                    witch.character.heavyMagicSkill.casted = true;
                } else {
                    Debug.Log("Not enough Manna");
                }
            }
        }

        // <Update> Back to original coding, duration --; 
        // UpdateTimer duration dan delay dari magicskill!
        // When the delay is over, back to original stats
        witch.character.lightMagicSkill.delay.current = UpdateTimer(witch.character.lightMagicSkill.delay.current, witch.character.lightMagicSkill.delay.max);
        witch.character.heavyMagicSkill.delay.current = UpdateTimer(witch.character.heavyMagicSkill.delay.current, witch.character.heavyMagicSkill.delay.max);
        
        witch.character.lightMagicSkill.duration.current = witch.character.UpdateDurationMagic(witch.character.lightMagicSkill);
        witch.character.heavyMagicSkill.duration.current = witch.character.UpdateDurationMagic(witch.character.heavyMagicSkill);

        // <Edit> Revert when the buff duration is over
        // if duration >= maxDuration
        if(witch.character.lightMagicSkill.duration.current >=  witch.character.lightMagicSkill.duration.max && witch.character.lightMagicSkill.casted){
            witch.character.RevertMagic(witch.character.lightMagicSkill);
            // <Edit> Pindahkan magic casted pada character
            //character.lightMagicSkill.magicCasted = false;
        }
        if(witch.character.heavyMagicSkill.duration.current >= witch.character.heavyMagicSkill.duration.max && witch.character.heavyMagicSkill.casted){
            witch.character.RevertMagic(witch.character.heavyMagicSkill);
            // <Edit> Pindahkan magic casted pada character
            //character.heavyMagicSkill.magicCasted = false;
        }
    }

    void GuardControl(){
        if(!_possessingBall && (witch.character.guard.available)){
            witch.character.guard.current = 0f;
        }
        witch.character.getTackledDelay.current = UpdateTimer(witch.character.getTackledDelay.current, witch.character.getTackledDelay.max);
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
            if(witch.character.guard.available && (witch.character.getTackledDelay.current >= witch.character.getTackledDelay.max)){
                witch.character.guard.current -= guardReduced;
            }
            if(witch.character.guard.empty) {
                witch.character.guard.current = 0;
                // Do short stun
                witch.character.stunnedDuration.current = 5f;
                Debug.Log("Short stunned! Guard:" + witch.character.guard.current + ", HP:" + witch.character.healthPoint.current);
                //BallReleasing();
                Vector3 startPos = new Vector3(ballPosition.transform.position.x, ballPosition.transform.position.y + 1.5f, ballPosition.transform.position.z); 
                Vector3 vel = ball.transform.forward;
                vel += new Vector3(0,1,0);
                BallReleasing(startPos, ball.transform.localRotation, ball.transform.forward, vel, Vector3.zero, 1 * transform.up * witch.character.shootPower.current);   

            }
            Debug.Log("Tackled when possesses"+_possessingBall);
        } 
        // Move these codes into above block to not let player loss the HP when in guard. 
        // Health Point
        //Debug.Log(character.healthPoint.available + " " + (character.getTackledDelay.current >= character.getTackledDelay.max));
        if(witch.character.healthPoint.available && (witch.character.getTackledDelay.current >= witch.character.getTackledDelay.max)){
            witch.character.healthPoint.current -= healthReduced;
        }
        //Debug.Log("GetTackledDelay: "+character.getTackledDelay.current);
        if(witch.character.healthPoint.empty) {
            witch.character.healthPoint.current = 0;
            witch.character.guard.current       = 0;
            // Do long stun
            witch.character.stunnedDuration.current = 10f;
            Debug.Log("Long stunned! Guard:" + witch.character.guard.current + ", HP:" + witch.character.healthPoint.current);
            if(_possessingBall){
                Vector3 startPos = new Vector3(ballPosition.transform.position.x, ballPosition.transform.position.y + 1.5f, ballPosition.transform.position.z); 
                Vector3 vel = ball.transform.forward;
                vel += new Vector3(0,1,0);
                BallReleasing(startPos, ball.transform.localRotation, ball.transform.forward, vel, Vector3.zero, 1 * transform.up * witch.character.shootPower.current);
                
                //BallReleasing();
                Debug.Log("Ball Released because Long Stun.");
            }
        }
        Debug.Log("Guard: "+witch.character.guard.current +" .HP: "+witch.character.healthPoint.current);
        //Debug.Break();
        witch.character.getTackledDelay.current = 0f;
        Debug.Log("GetTackledDelay: "+witch.character.getTackledDelay.current);
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
            witch.character.guard.current    = witch.character.guard.max;
            Debug.Log("Guard:"+witch.character.guard.current);
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
    //<Edit later> BallReleasing(direction, velocity, angularvelocity, force, startpos)
    public void BallReleasing(Vector3 startPosition, Quaternion rotation, Vector3 direction, Vector3 velocity, Vector3 angularVelocity, Vector3 force){
        
        _possessingBall = false;
        // stuff before set off parent
        //ball.transform.localPosition += new Vector3(0f, 0f, 0.3f);
        ball.transform.position = startPosition;
        ball.transform.localRotation = rotation;
        //Vector3 targetDir = transform.forward; //-ball.transform.forward;
        //targetDir.y += 0.5f;
        
        ball.GetComponent<Ball>().Released(Ball.BallState.Free);
        //ball.transform.rotation = rotation;
        Rigidbody ballRigidBody = ball.GetComponent<Rigidbody>(); 
        ballRigidBody.velocity  = velocity;
        ballRigidBody.angularVelocity = angularVelocity;
        ballRigidBody.AddForce(force, ForceMode.Impulse);
        //ball.GetComponent<Rigidbody>().velocity = direction;  
        //ball.GetComponent<Rigidbody>().AddForce(targetDir, ForceMode.Impulse);

        Match match = GameObject.FindObjectOfType<Match>();
         if(match != null){
            if(this.teamParty == match.TeamA.teamParty){
                match.TeamA.teamState = Team.TeamState.Defense;
            } else if (this.teamParty == match.TeamB.teamParty){
                match.TeamB.teamState = Team.TeamState.Defense;
            }
        }
    }
    public void BallReleasing(bool addForce = true){
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
            if(witch.character.getTackledDelay.current >= witch.character.getTackledDelay.max){
                Debug.Log(gameObject.name + " tackled by" + other.gameObject.name);
                // <Edit later> Check if later the HP is zero and player possessing the ball, then addforce to the ball. 
                Tackled(witch.character.tackledDamageToGuard.current, witch.character.tackledDamageToHealth.current);
                witch.character.getTackledDelay.current = 0f;
                Debug.Break();
                // <Edit later> Attacker get a manna. 
                // <Edit later> AddForce to the damaged witch
            }
        }   
    }
    void OnCollisionEnter(Collision other) {
         // <Edit later> Collide with witch should be in OnCollisionStay. Or, make temp bool to store elapsed tackle
         /* if(other.gameObject.tag == "Witch"){
             Debug.Log("Collide with " + other.gameObject.name);
         }*/
        // Tackled 
        /* if(other.gameObject.tag == "Witch" && other.gameObject.GetComponent<WitchController>()._isTackling){
        //if(other.gameObject.name == "damageCollider") {
            Debug.Log("Collide with " + other.gameObject.name);
            //Debug.Log("tackledDelay: "+witchCharacter.tackledDelay + ". maxTackledDelay" + witchCharacter.maxTackledDelay);
            if(witch.character.getTackledDelay.current >= witch.character.getTackledDelay.max){
                Debug.Log(gameObject.name + " tackled by" + other.gameObject.name);
                Tackled(witch.character.tackledDamageToGuard.current, witch.character.tackledDamageToHealth.current);
                witch.character.getTackledDelay.current = 0f;
                // <Edit later> Attacker get a manna. 
                // <Edit later> AddForce to the damaged witch & Set position
            }
        }*/
        // MysteryBox
        if(other.gameObject.tag == "MysteryBox") { 
            if(witch.character.usedMysteryBox == null) {
                other.gameObject.GetComponent<MysteryBox>().UseEffect(this);
                Debug.Log("Taking MysteryBox: " + other.gameObject.name);
            }
        }

        if(other.gameObject.tag == "Rock"){
            if(witch.character.getTackledDelay.current >= witch.character.getTackledDelay.max){
                // <Edit later> Need to change the force when the ball is released
                Rock rock = other.gameObject.GetComponent<Rock>();
                Debug.Log(gameObject.name + " damaged by" + rock.gameObject.name);
                Damaged(rock.damageGuard, rock.damageHealth);
                Physics.IgnoreCollision(ball.GetComponent<SphereCollider>(), rock.gameObject.GetComponent<BoxCollider>(), true);
                //transform.rotation = Quaternion.LookRotation(rock.transform.position - transform.position);
                
                if(_possessingBall){
                    Vector3 startPos = new Vector3(ball.transform.position.x, ball.transform.position.y + 1.5f, ball.transform.position.z); 
                    Vector3 vel = ball.transform.forward;
                    vel += new Vector3(0,2,0);
                    BallReleasing(startPos, ball.transform.localRotation, ball.transform.forward, vel, Vector3.zero, 1 * transform.up * witch.character.shootPower.current);
                    
                    
                    //BallReleasing();
                    //GameObject.Find("Ball").GetComponent<Rigidbody>().position = new Vector3(transform.position.x,
                    //                                                                     transform.position.y + 0.5f, transform.position.z);
                    
                    /* ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 3, 0);
                    ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    ball.transform.rotation = Quaternion.identity; //<Need to change apparently>
                    ball.GetComponent<Rigidbody>().AddForce(2 * transform.forward * witch.character.shootPower.current, ForceMode.Impulse);
                    */
                }
                
                //gameObject.GetComponent<Rigidbody>().AddExplosionForce(5f, rock.transform.position, 2f, 2f, ForceMode.Impulse);
                //Debug.Log("Collide with:" + gameObject.name + " Direction:" + gameObject.transform.rotation);

                witch.character.getTackledDelay.current = 0f;
            }
        }

        // <Edit later> Spiky & Explode & Rock and need to check the ball possession
        //if(other.gameObject.tag == "Tile") { && if typeperformed
            
            // spiky and exploding damage and addforce
        //}
        
        
        // <Edit later>
        // Possess the ball when touching it, later it can possessed when the ball is Shot and Passed too. and when the velocity is low. 
        // Ball 
        if(other.gameObject == ball && witch.character.stunnedDuration.current <= 0) {
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
        if(witch.character.usedMysteryBox != null){
            //Debug.Log(character.usedMysteryBox.duration);
            // If already casted
            if(witch.character.usedMysteryBox.duration > 0 && witch.character.usedMysteryBox.casted) {
                witch.character.usedMysteryBox.duration -= Time.deltaTime;
            } else if (witch.character.usedMysteryBox.duration <= 0 && witch.character.usedMysteryBox.casted) {
                
                witch.character.RevertMysteryBox(witch.character.usedMysteryBox);
            }
            
        }
    }
} 