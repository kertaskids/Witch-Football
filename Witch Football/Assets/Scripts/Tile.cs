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
    public float maxDuration;
    public float maxDelay;
    private float _duration; 
    private float _delay;
    private bool _effectPerformed; 
    private bool _collided;

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
        _duration           = 0f;
        _delay              = 0f;
        _effectPerformed    = false;
        _collided           = false;
        maxDelay            = 0f;
        maxDuration         = 0f;
    }
    public Tile(TileType tileType, TileEffect tileEffect, TriggerType triggerType, GridPosition gridPos, 
                float triggerDistance, float maxDuration, float maxDelay){
        this.tileType    = tileType;
        this.tileEffect  = tileEffect;
        this.triggerType = triggerType;
        this.gridPos     = new GridPosition(gridPos);
        this.triggerDistance    = triggerDistance;
        this._duration          = 0f;
        this._delay             = 0f;
        this._effectPerformed   = false;
        this._collided          = false;
        this.maxDelay           = _delay;
        this.maxDuration        = _duration;
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
        else if(tileType == TileType.Exploding){}
        else if(tileType == TileType.MysteryBox){}
        else if(tileType == TileType.Shooting){}
        else if(tileType == TileType.Spiky){}
    }
    //<Edit later> delete the second if. Just use else instead. Change the static Y value. Add effect speed. 
    public void PerformEffect(){
        if(tileEffect == TileEffect.None){}
        else if(tileEffect == TileEffect.RisingUP){
            if(_effectPerformed){
                if(transform.position.y >= 1){
                    transform.position = new Vector3 (transform.position.x, 1, transform.position.z);
                    _duration = _duration + 1 * Time.deltaTime;
                    if(_duration > maxDuration){
                        _effectPerformed = false;
                        _duration = 0;
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
                    _delay = _delay + 1 * Time.deltaTime;
                    if(_delay > maxDelay){
                        _effectPerformed = true;
                        _delay    = 0f;
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
                    _duration = _duration + 1 * Time.deltaTime;
                    if(_duration > maxDuration){
                        _effectPerformed = false;
                        _duration = 0;
                        gameObject.GetComponent<MeshRenderer>().enabled = true;
                        gameObject.GetComponent<BoxCollider>().enabled  = true;
                    }
                }
            }
            if(!_effectPerformed){
                if(gameObject.GetComponent<MeshRenderer>().enabled){
                    _delay = _delay + 1 * Time.deltaTime;
                    if(_delay > maxDelay){
                        _effectPerformed = true;
                        _delay    = 0f;
                        gameObject.GetComponent<MeshRenderer>().enabled = false;
                        gameObject.GetComponent<BoxCollider>().enabled  = false;
                    }
                } else {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
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
                float distance = Mathf.Abs(Vector3.Distance(transform.position, closestWitch.transform.position));

                foreach (GameObject w in Witches) {
                    float distanceNext = Mathf.Abs(Vector3.Distance(transform.position, w.transform.position));
                    if(distance > distanceNext){
                        distance = distanceNext;
                    }
                }
                return (distance <= triggerDistance);
            }
            return false;
        } else if (_collided) {
            return true;
        }
        return false;
    }
    void Update(){
        if(ValidateTrigger()){
            PerformEffect();
        }
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Witch"){
            _collided = true;
        }
    }
    void OnCollisionExit(Collision other) {
        if(other.gameObject.tag == "Witch"){
            _collided = false;
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
