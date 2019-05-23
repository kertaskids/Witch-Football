using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    public int Score;
    public WitchController[] witches;
    public void Initialize(){
        Score = 0;
    }
}
