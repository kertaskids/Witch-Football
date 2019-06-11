using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum BallState {
        Free, 
        Possessed, 
        Passed, 
        Shot
    }
    public BallState ballState; 
    public WitchController possesingWitch;
    public WitchController lastToucher;

    void Start(){
        ballState       = BallState.Free;
        possesingWitch  = null;
        lastToucher     = null;
    }
    public void Possessed(WitchController witch){
        possesingWitch  = witch;
        ballState       = BallState.Possessed;
        lastToucher     = witch;
        // Delete this if we want the ball movee wildly
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.parent = witch.gameObject.transform;
        gameObject.transform.localRotation = Quaternion.identity;
        // When the ball is posessed by player, the collider is trough.
        gameObject.GetComponent<SphereCollider>().enabled = false;
    }
    public void Released(BallState ballState){
        // When the ball is posessed by player, the collider is trough.
        gameObject.GetComponent<SphereCollider>().enabled = true;
        Debug.Log("Ball released from: "+lastToucher.gameObject.name);
        this.possesingWitch  = null;
        this.ballState       = ballState;
        gameObject.transform.parent = null;
    }
}