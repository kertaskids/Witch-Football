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

    void Start() {
        ballState       = BallState.Free;
        possesingWitch  = null;
    }

    public void ChangeState(WitchController witch, BallState state){
        possesingWitch  = witch;
        ballState       = state;
    }
    public void Possessed(WitchController witch){
        possesingWitch  = witch;
        ballState       = BallState.Possessed;
    }
    public void Released(BallState ballState){
        this.possesingWitch  = null;
        this.ballState       = ballState;
    }
}