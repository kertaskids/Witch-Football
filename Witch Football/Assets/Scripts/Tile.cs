using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public enum TileType{
        None, 
        Normal,
        Spiky, 
        Exploding,
        Shooting, 
        MysteryBox,
        FallingRock
    }
    public TileType tileType;
    public enum TileEffect{
        None, 
        RisingUP,
        Invisible 
    } 
    public TileEffect tileEffect;
    public enum TriggerType{
        None,
        Collide,
        Distance
    }
    public TriggerType triggerType;
    public GridPosition gridPos;
    // <Edit later> make a CharacterStat variable rather than these. Need a custom inspector or 
    // just simply pass this variable's value to the characterstat variable.
    public float triggerDistance;
    public float effectMaxDuration;
    public float effectMaxDelay;
    private float _effectDuration; 
    private float _effectDelay;
    public float typeMaxDuration;
    public float typeMaxDelay;
    private float _typeDuration;
    private float _typeDelay;
    private bool _effectPerformed; 
    private bool _typePerformed;
    private List<GameObject> _collidedWitches;
    private List<GameObject> _onRangeWitches;

    void Start() {
        //MaxDuration = 2f;
        //MaxDelay = 5f;
        _collidedWitches    = new List<GameObject>();
        _onRangeWitches     = new List<GameObject>();
    }
    public Tile(){
        tileType    = TileType.Normal;
        tileEffect  = TileEffect.None;
        triggerType = TriggerType.None;
        gridPos     = new GridPosition(0,0,0);
        triggerDistance     = 0f;
        _effectDuration     = 0f;
        _effectDelay        = 0f;
        _effectPerformed    = false;
        _typePerformed      = false;
        effectMaxDelay      = 0f;
        effectMaxDuration   = 0f;
        _typeDuration       = 0f;
        _typeDelay          = 0f;
        typeMaxDelay        = 0f;
        typeMaxDuration     = 0f;
    }
    public Tile(TileType tileType, TileEffect tileEffect, TriggerType triggerType, GridPosition gridPos, 
                float triggerDistance, float maxDuration, float maxDelay, float typeDuration, float typeMaxDuration, float typeDelay, float typeMaxDelay){
        this.tileType    = tileType;
        this.tileEffect  = tileEffect;
        this.triggerType = triggerType;
        this.gridPos     = new GridPosition(gridPos);
        this.triggerDistance    = triggerDistance;
        this._effectDuration    = 0f;
        this._effectDelay       = 0f;
        this._effectPerformed   = false;
        this._typePerformed     = false;
        this.effectMaxDelay     = _effectDelay;
        this.effectMaxDuration  = _effectDuration;
        this._typeDuration      = typeDuration;
        this._typeDelay         = typeDelay;
        this.typeMaxDelay       = typeMaxDelay;
        this.typeMaxDuration    = typeMaxDuration;
    }
    //<Edit later>
    public void Randomized(){
        // Inclusive parameters. Ignore the None type.
        int r       = Random.Range(2,6);
        tileType    = (TileType) r; 
        // Adding the children here
    }
    //<Edit later> perform animation
    public void PerformType(){
        if(tileType == TileType.None){}
        else if(tileType == TileType.Normal){}
        else if(tileType == TileType.Exploding){
            if(_onRangeWitches.Count > 0 && triggerType == TriggerType.Distance) {
               foreach (GameObject cw in _onRangeWitches){
                    cw.GetComponent<WitchController>().Damaged(3f, 3f); // Make a variable to hold this
                    cw.transform.rotation = Quaternion.LookRotation(transform.position - cw.transform.position);
                    cw.GetComponent<Rigidbody>().AddExplosionForce(5f, transform.position, 2f, 2f, ForceMode.Impulse);
                    Debug.Log("Collide with:" + cw.name + " direction:" + cw.transform.rotation);
                    
                    // Need to check the ball possession 
                    // Uncomment this if we want to make the ball unpossessed by the player after explosion
                    //if(_collideWith.GetComponent<WitchController>()._possessingBall){}

                    // <Edit later> move to witch
                    GameObject ball = GameObject.Find("Ball");
                    if(ball.GetComponent<Ball>().ballState == Ball.BallState.Free){
                        ball.transform.rotation = Quaternion.LookRotation(transform.position - ball.transform.position);
                        ball.GetComponent<Rigidbody>().AddExplosionForce(5f, transform.position, 2f, 2f, ForceMode.Impulse);
                    }
               } 
            }
            if(_collidedWitches.Count > 0 && triggerType == TriggerType.Collide){
                // <Edit later> move explosion damage to witch
                // <Doing> Change the single object with the all collided objects
                foreach (GameObject cw in _collidedWitches){
                    cw.GetComponent<WitchController>().Damaged(3f, 3f); // Make a variable to hold this
                    cw.transform.rotation = Quaternion.LookRotation(transform.position - cw.transform.position);
                    cw.GetComponent<Rigidbody>().AddExplosionForce(5f, transform.position, 2f, 2f, ForceMode.Impulse);
                    Debug.Log("Collide with:" + cw.name + " direction:" + cw.transform.rotation);
                
                    // Need to check the ball possession 
                    // Uncomment this if we want to make the ball unpossessed by the player after explosion
                    //if(_collideWith.GetComponent<WitchController>()._possessingBall){}

                    // <Edit later> move to witch
                    GameObject ball = GameObject.Find("Ball");
                    if(ball.GetComponent<Ball>().ballState == Ball.BallState.Free){
                        ball.transform.rotation = Quaternion.LookRotation(transform.position - ball.transform.position);
                        ball.GetComponent<Rigidbody>().AddExplosionForce(5f, transform.position, 2f, 2f, ForceMode.Impulse);
                    }
                }
            }
            Destroy(this.gameObject);
        }
        else if(tileType == TileType.MysteryBox){
            // <Edit later> Change this with mysterybox prefabs
            if(_typeDelay <= 0f) {
                // Spawn mystery box prefabs based on random
                int r = Random.Range(1, 5);
                GameObject mysteryBox = GameObject.CreatePrimitive(PrimitiveType.Quad);
                if(r == 1) mysteryBox = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                if(r == 2) mysteryBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
                if(r == 3) mysteryBox = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                if(r == 4) mysteryBox = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                mysteryBox.transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
                mysteryBox.AddComponent<Rigidbody>();
                mysteryBox.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                _typeDelay = typeMaxDelay;
            }
            _typeDelay -= Time.deltaTime;
        }
        else if(tileType == TileType.Shooting){
            // Shoot random nearby witch; ignoring the trigger type (distance and collide can work)
            // <Edit later> Shooting to random witch but the closest witch get 50% higher chance of getting shot
            if(_typeDelay <= 0){
                if(_onRangeWitches.Count > 0) {
                    // Prepare shooter
                    GameObject canon = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    canon.AddComponent<Rigidbody>();
                    canon.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                    // <Edit later> moveup the shooter if the duration is available

                    // Random nearby target
                    int r = Random.Range(0, _onRangeWitches.Count - 1);
                    canon.transform.LookAt(_onRangeWitches[r].transform);
                    canon.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                    // <Delete later> just to check the shooting
                    //canon.GetComponent<BoxCollider>().enabled = false;
                    canon.GetComponent<Rigidbody>().AddForce(canon.transform.forward * 10f, ForceMode.Impulse);
                    _typeDelay = typeMaxDelay;
                }
            }
            _typeDelay -= Time.deltaTime;
        }
        else if(tileType == TileType.Spiky){
            // Only impact the CollidedWitches
            Transform trap = gameObject.transform.Find("Trap");
            if(_typePerformed){
                // <Edit later> if trap != null
                if(trap.GetComponent<MeshRenderer>().enabled){
                    trap.GetComponent<MeshRenderer>().enabled = false;  
                    //trap.GetComponent<BoxCollider>().enabled  = false;   
                } else {
                    _typeDelay = _typeDelay + 1 * Time.deltaTime;
                    if(_typeDelay > typeMaxDelay){
                        _typePerformed = false;
                        _typeDelay = 0;
                        trap.GetComponent<MeshRenderer>().enabled = true;
                        //trap.GetComponent<BoxCollider>().enabled  = true; // <Delete> this, only need the Renderer

                        if(_collidedWitches.Count > 0){
                            foreach (GameObject w in _collidedWitches){
                                // Need to check the ball possesion too 
                                w.GetComponent<WitchController>().Damaged(1f, 1f); 
                                w.transform.rotation = Quaternion.LookRotation(transform.position - w.transform.position);
                                w.GetComponent<Rigidbody>().AddExplosionForce(5f, transform.position, 2f, 2f, ForceMode.Impulse);
                                Debug.Log("Collide with:" + w.name + " Direction:" + w.transform.rotation);
                            }
                        } 
                    }
                }
            }
            if(!_typePerformed){
                if(trap.GetComponent<MeshRenderer>().enabled){
                    _typeDuration = _typeDuration - 1 * Time.deltaTime;
                    if(_typeDuration < 0){
                        _typePerformed = true;
                        _typeDuration  = typeMaxDuration;
                        trap.GetComponent<MeshRenderer>().enabled = false;
                        //trap.GetComponent<BoxCollider>().enabled  = false;
                    }
                } else {
                    trap.GetComponent<MeshRenderer>().enabled = true;
                    //trap.GetComponent<BoxCollider>().enabled  = true; // <Delete> this, only need the Renderer
                    
                    // <Edit later> move these to witch?, collidewitch should be an array
                    if(_collidedWitches.Count > 0) {
                        // <Edit later> Need to check the ball possesion too 
                        foreach (GameObject w in _collidedWitches)
                        {
                            w.GetComponent<WitchController>().Damaged(1f, 1f); 
                            w.transform.rotation = Quaternion.LookRotation(transform.position - w.transform.position);
                            w.GetComponent<Rigidbody>().AddExplosionForce(5f,transform.position, 2f, 2f, ForceMode.Impulse);
                            Debug.Log("Collide with:" + w.name + " Direction:" + w.transform.rotation);
                        }
                    }
                }
            }  
            // <Edit later> if not collided, animation off immediatelly. 
        }
        else if (tileType == TileType.FallingRock) {
            if(_typeDelay <= 0f) {
                // Spawn rock randomly, <1> Small rock, <2> Big rock
                int r = Random.Range(1, 3);
                GameObject rock = GameObject.CreatePrimitive(PrimitiveType.Quad);
                if(r == 1) rock = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                if(r == 2) rock = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rock.transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
                rock.AddComponent<Rigidbody>();
                rock.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                _typeDelay = typeMaxDelay;
            }
            _typeDelay -= Time.deltaTime;
        }
    }
    
    public void PerformEffect(){
        if(tileEffect == TileEffect.None){}
        else if(tileEffect == TileEffect.RisingUP){
            //<Edit later>  Change the static Y value. Add effect speed. 
            if(_effectPerformed){
                if(transform.position.y >= 1){
                    transform.position = new Vector3 (transform.position.x, 1, transform.position.z);
                    _effectDuration = _effectDuration + 1 * Time.deltaTime;
                    if(_effectDuration > effectMaxDuration){
                        _effectPerformed = false;
                        _effectDuration = 0;
                    }
                } else {
                    transform.position = new Vector3(transform.position.x, 
                                                        transform.position.y + 1 * Time.deltaTime, 
                                                        transform.position.z);
                }
            } 
            if(!_effectPerformed){
                if(transform.position.y <= 0){
                    transform.position = new Vector3 (transform.position.x, 0, transform.position.z);
                    _effectDelay = _effectDelay + 1 * Time.deltaTime;
                    if(_effectDelay > effectMaxDelay){
                        _effectPerformed = true;
                        _effectDelay    = 0f;
                    }
                } else {
                    transform.position = new Vector3(transform.position.x, 
                                                        transform.position.y - 1 * Time.deltaTime, 
                                                        transform.position.z);
                }
            }    
        }
        else if(tileEffect == TileEffect.Invisible){
            if(_effectPerformed){
                if(gameObject.GetComponent<MeshRenderer>().enabled){
                    gameObject.GetComponent<MeshRenderer>().enabled = false;  
                    gameObject.GetComponent<BoxCollider>().enabled  = false;   
                } else {
                    _effectDuration = _effectDuration + 1 * Time.deltaTime;
                    if(_effectDuration > effectMaxDuration){
                        _effectPerformed = false;
                        _effectDuration = 0;
                        gameObject.GetComponent<MeshRenderer>().enabled = true;
                        gameObject.GetComponent<BoxCollider>().enabled  = true;
                    }
                }
            }
            if(!_effectPerformed){
                if(gameObject.GetComponent<MeshRenderer>().enabled){
                    _effectDelay = _effectDelay + 1 * Time.deltaTime;
                    if(_effectDelay > effectMaxDelay){
                        _effectPerformed = true;
                        _effectDelay    = 0f;
                        gameObject.GetComponent<MeshRenderer>().enabled = false;
                        gameObject.GetComponent<BoxCollider>().enabled  = false;
                    }
                } else {
                    gameObject.GetComponent<MeshRenderer>().enabled = true;
                    gameObject.GetComponent<BoxCollider>().enabled  = true;
                }
            }       
        }
    }
    public bool ValidateTrigger(){
        if(triggerType == TriggerType.None) {
            return true;
        } else if (triggerType == TriggerType.Distance) {
            return (_onRangeWitches.Count >= 1);
        } else if (triggerType == TriggerType.Collide) {
            return (_collidedWitches.Count >= 1);
        }
        return false;
    }
    void Update(){
        if(ValidateTrigger()){
            PerformEffect();
            PerformType();
        }
        OnRangeTrigger();
    }

    void OnCollisionEnter(Collision other) {        
        if(other.gameObject.tag == "Witch"){
            if(_collidedWitches == null || _collidedWitches.Count < 1) {
                _collidedWitches.Clear();
            }
            if(!_collidedWitches.Contains(other.gameObject)){ // Need to refine this, because the collision object might be different. 
                _collidedWitches.Add(other.gameObject);
            }
        }
    }
    void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "Witch"){
            if(_collidedWitches.Contains(other.gameObject)){
                _collidedWitches.Remove(other.gameObject);
            }
        }
        if(_collidedWitches == null || _collidedWitches.Count < 1){
            _collidedWitches.Clear();
        }
    }

    void OnRangeTrigger() {
        List<GameObject> allWitches = GameObject.FindGameObjectsWithTag("Witch").ToList();
        foreach (GameObject w in allWitches)
        {
            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), 
                                                new Vector2(w.transform.position.x, w.transform.position.z));
            
            // Enter 
            if(distance <= triggerDistance && !_onRangeWitches.Contains(w)){
                _onRangeWitches.Add(w);
            }

            // Exit 
            if(distance > triggerDistance && _onRangeWitches.Contains(w)){
                _onRangeWitches.Remove(w);
            }
        }
    }
}

public class GridPosition
{
        public int Row;
        public int Column;
        public int Height;

        public GridPosition(int row, int column, int height) {
            Row     = row;
            Column  = column;
            Height  = height;
        }
        public GridPosition(GridPosition source) {
            Row     = source.Row;
            Column  = source.Column;
            Height  = source.Height;
        }
}
