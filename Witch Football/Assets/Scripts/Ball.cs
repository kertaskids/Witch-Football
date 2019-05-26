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


    void Start() {
        ballState       = BallState.Free;
        possesingWitch  = null;
        lastToucher     = null;
    }

    public void ChangeState(WitchController witch, BallState state){
        possesingWitch  = witch;
        ballState       = state;
        lastToucher     = witch;
    }
    public void Possessed(WitchController witch){
        possesingWitch  = witch;
        ballState       = BallState.Possessed;
        lastToucher     = witch;
    }
    public void Released(BallState ballState){
        this.possesingWitch  = null;
        this.ballState       = ballState;
        Debug.Log("Ball released from: "+lastToucher.gameObject.name);
    }
}