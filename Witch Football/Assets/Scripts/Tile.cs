using System.Collections;
using System.Collections.Generic;
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
    private bool _collided;
    // <Edit later> GameObject[] _collidedWitches; make GetCollidedWitches(); GetNearbyWitches()
    private GameObject _collideWith;
    private GameObject _collidedWitches;
    private GameObject _nearbyWitches;


    void Start() {
        //MaxDuration = 2f;
        //MaxDelay = 5f;
    }
    public Tile(){
        tileType    = TileType.Normal;
        tileEffect  = TileEffect.None;
        triggerType = TriggerType.None;
        gridPos     = new GridPosition(0,0,0);
        triggerDistance     = 0f;
        _effectDuration           = 0f;
        _effectDelay              = 0f;
        _effectPerformed    = false;
        _typePerformed      = false;
        _collided           = false;
        effectMaxDelay            = 0f;
        effectMaxDuration         = 0f;

        _typeDuration = 0f;
        _typeDelay = 0f;
        typeMaxDelay = 0f;
        typeMaxDuration = 0f;
    }
    public Tile(TileType tileType, TileEffect tileEffect, TriggerType triggerType, GridPosition gridPos, 
                float triggerDistance, float maxDuration, float maxDelay, float typeDuration, float typeMaxDuration, float typeDelay, float typeMaxDelay){
        this.tileType    = tileType;
        this.tileEffect  = tileEffect;
        this.triggerType = triggerType;
        this.gridPos     = new GridPosition(gridPos);
        this.triggerDistance    = triggerDistance;
        this._effectDuration          = 0f;
        this._effectDelay             = 0f;
        this._effectPerformed   = false;
        this._typePerformed     = false;
        this._collided          = false;
        this.effectMaxDelay           = _effectDelay;
        this.effectMaxDuration        = _effectDuration;
        this._typeDuration = typeDuration;
        this._typeDelay = typeDelay;
        this.typeMaxDelay = typeMaxDelay;
        this.typeMaxDuration = typeMaxDuration;
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
            // <Edit later> Damage the witch with trigger distance or just with what collide with
            // addforce to witch and ball
            // release ball
            
            /* 
            GameObject[] witches = GameObject.FindGameObjectsWithTag("Witch");
            if(witches !=null && witches.Length > 0){
                foreach (GameObject w in witches){
                    // Check the distance
                    w.GetComponent<WitchController>().ExplosionDamaged(3f, 5f);
                }
            }
            */
            if(_collided){
                // <Edit later> move explosion damage to witch
                _collideWith.GetComponent<WitchController>().ExplosionDamaged(3f, 5f); // Release the ball
                _collideWith.transform.rotation = Quaternion.LookRotation(transform.position - _collideWith.transform.position);
                _collideWith.GetComponent<Rigidbody>().AddExplosionForce(5f,transform.position, 2f, 2f, ForceMode.Impulse);
                Debug.Log("Collide with:"+_collideWith.name+" Direction:"+_collideWith.transform.rotation);
                // Need to check the ball possession
                //if(_collideWith.GetComponent<WitchController>()._possessingBall){}
                // <Edit later> move to witch
                GameObject ball = GameObject.Find("Ball");
                if(ball.GetComponent<Ball>().ballState == Ball.BallState.Free){
                    ball.transform.rotation = Quaternion.LookRotation(transform.position - ball.transform.position);
                    ball.GetComponent<Rigidbody>().AddExplosionForce(5f,transform.position, 2f, 2f, ForceMode.Impulse);
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
            //<Edit later> Shoot all the witches in the range, randomly
            // <Edit later> moveup the shooter if the duration is availablestart shooting to random witch but the closest witch get 50% higher chance of getting shot
            if(_typeDelay <= 0){
                GameObject canon = GameObject.CreatePrimitive(PrimitiveType.Cube);
                canon.AddComponent<Rigidbody>();
                canon.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                //Vector3 targetDir = GameObject.Find("Red Witch").transform.position - transform.position;
                // <edit later> can be changed to y = 0; below.
                //canon.transform.rotation = Quaternion.LookRotation(new Vector3(targetDir.x, -targetDir.y / 2, targetDir.z));
                // Check the direction and targets first
                canon.transform.LookAt(GameObject.Find("Red Witch").transform);
                canon.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                //canon.GetComponent<BoxCollider>().enabled = false;
                canon.GetComponent<Rigidbody>().AddForce(canon.transform.forward * 10f, ForceMode.Impulse);
                _typeDelay = typeMaxDelay;
                
            }
            _typeDelay -= Time.deltaTime;
            

        }
        else if(tileType == TileType.Spiky){
            // <Edit later> put trap as global var?
            Transform trap = gameObject.transform.Find("Trap");
            if(_typePerformed){
                // <Edit later> if trap != null
                if(trap.GetComponent<MeshRenderer>().enabled){
                    trap.GetComponent<MeshRenderer>().enabled = false;  
                    trap.GetComponent<BoxCollider>().enabled  = false;   
                } else {
                    _typeDelay = _typeDelay + 1 * Time.deltaTime;
                    if(_typeDelay > typeMaxDelay){
                        _typePerformed = false;
                        _typeDelay = 0;
                        trap.GetComponent<MeshRenderer>().enabled = true;
                        trap.GetComponent<BoxCollider>().enabled  = true;

                        // <Edit later> move these to witch, collidewitch should be an array
                        if(_collided) {
                            // Need to check the ball possesion too 
                            _collideWith.GetComponent<WitchController>().ExplosionDamaged(1f, 1f); 
                            _collideWith.transform.rotation = Quaternion.LookRotation(transform.position - _collideWith.transform.position);
                            _collideWith.GetComponent<Rigidbody>().AddExplosionForce(5f,transform.position, 2f, 2f, ForceMode.Impulse);
                            Debug.Log("Collide with:"+_collideWith.name+" Direction:"+_collideWith.transform.rotation);
                        }
                    }
                }
            }
            if(!_typePerformed){
                if(trap.GetComponent<MeshRenderer>().enabled){
                    _typeDuration = _typeDuration - 1 * Time.deltaTime;
                    if(_typeDuration < 0){
                        _typePerformed = true;
                        _typeDuration    = typeMaxDuration;
                        trap.GetComponent<MeshRenderer>().enabled = false;
                        trap.GetComponent<BoxCollider>().enabled  = false;
                    }
                } else {
                    trap.GetComponent<MeshRenderer>().enabled = true;
                    trap.GetComponent<BoxCollider>().enabled  = true;
                    // <Edit later> move these to witch, collidewitch should be an array
                        if(_collided) {
                            // Need to check the ball possesion too 
                            _collideWith.GetComponent<WitchController>().ExplosionDamaged(1f, 1f); 
                            _collideWith.transform.rotation = Quaternion.LookRotation(transform.position - _collideWith.transform.position);
                            _collideWith.GetComponent<Rigidbody>().AddExplosionForce(5f,transform.position, 2f, 2f, ForceMode.Impulse);
                            Debug.Log("Collide with:"+_collideWith.name+" Direction:"+_collideWith.transform.rotation);
                        }
                }
            }  
            // <Edit later> if not collided, animation off immediatelly. 
        }
        else if (tileType == TileType.FallingRock) {
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
    }
    //<Edit later> delete the second if. Just use else instead. Change the static Y value. Add effect speed. 
    public void PerformEffect(){
        if(tileEffect == TileEffect.None){}
        else if(tileEffect == TileEffect.RisingUP){
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
            GameObject[] Witches = GameObject.FindGameObjectsWithTag("Witch"); 
            if((Witches != null) && (Witches.Length > 0)){
                GameObject closestWitch = Witches[0];
                float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z),
                                                    new Vector2(closestWitch.transform.position.x, closestWitch.transform.position.z));
                //Mathf.Abs(Vector3.Distance(transform.position, closestWitch.transform.position));

                foreach (GameObject w in Witches) {
                    //<edit later> use vector2.distance instead
                    float distanceNext = (new Vector2(transform.position.x, transform.position.z) - 
                                                    new Vector2(w.transform.position.x, w.transform.position.z)).magnitude;
                    if(distance > distanceNext){
                        distance = distanceNext;
                    }
                }
                return (distance <= triggerDistance);
            }
            return false;
        } else if (_collided && triggerType == TriggerType.Collide) {
            return true;
        }
        return false;
    }
    void Update(){
        if(ValidateTrigger()){
            PerformEffect();
            PerformType();
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Witch"){
            _collided = true;
            _collideWith = other.gameObject;
        }
    }
    void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "Witch"){
            _collided = false;
            _collideWith = null;
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
