using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum BallState {
        // <Edit later> For now, only use free and possessed
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

    //<Edit later> use this to replace in witchcontroller
    //<Edit later> change the method below with general method i.e., ChangeState(Witch, State);
    public void Possessed(WitchController witch){
        possesingWitch  = witch;
        ballState       = BallState.Possessed;
    }
    public void Released(BallState ballState){
        this.possesingWitch  = null;
        this.ballState       = ballState;
    }
}